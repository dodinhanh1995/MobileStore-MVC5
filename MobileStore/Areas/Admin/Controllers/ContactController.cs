using System.Web.Mvc;
using static Common.Enum.EDatabase;
using System.Collections.Generic;
using System;
using System.Linq;
using Models.DAO.Repository;

namespace MobileStore.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index(string sortOrder, string currentKey, string key, EContactColumnsName columnName = EContactColumnsName.All)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : null;
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            if (key == null)
                key = currentKey;

            ViewBag.CurrentKey = key;
            ViewBag.CurrentColumnName = columnName;

            ColumnsNameDropDownList(columnName);

            return View(_unitOfWork._ContactRepo.GetListByFilterAndSort(sortOrder, key, columnName));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(FormCollection form)
        {
            string[] contactIds = form["contactId"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in contactIds)
            {
                try
                {
                    _unitOfWork._ContactRepo.Delete(int.Parse(item));
                    _unitOfWork.Save();
                }
                catch { }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detail(int? id)
        {
            var contact = _unitOfWork._ContactRepo.SelectById(id);
            if (contact == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy chi tiết liên hệ. Vui lòng thử lại.";
                return View("Error");
            }

            return View(contact);
        }

        public bool ChangingStatus(int id)
        {
            try
            {
                return _unitOfWork._ContactRepo.ChangingStatus(id);
            }
            catch
            {
                return false;
            }
            
        }

        private void ColumnsNameDropDownList(EContactColumnsName selected)
        {
            IEnumerable<EContactColumnsName> values = Enum.GetValues(typeof(EContactColumnsName)).Cast<EContactColumnsName>();

            var items = from value in values
                        select new SelectListItem
                        {
                            Text = value.ToString(),
                            Value = value.ToString(),
                            Selected = value == selected
                        };
            ViewBag.ColumnName = items;
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}