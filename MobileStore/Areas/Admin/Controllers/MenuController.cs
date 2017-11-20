using Common.Enum;
using Models.DAO.Repository;
using Models.EF;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace MobileStore.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ViewResult Index()
        {
            return View(_unitOfWork._MenuRepo.Select(orderBy: m => m.OrderByDescending(x => x.Id)));
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] menuIds = form["menuId"].Split(',');

            foreach (string item in menuIds)
            {
                try
                {
                    _unitOfWork._MenuRepo.Delete(int.Parse(item));
                }
                catch { }

            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            LoadDropDownList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Text, Link, DisplayOrder, Target, Status, TypeID, Icon")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork._MenuRepo.CheckNameIsExists(menu.Text, menu.TypeID.Value))
                {
                    ModelState.AddModelError("Text", "Tên menu đã tồn tại");
                    LoadDropDownList(menu.TypeID, menu.Target);
                    return View(menu);
                }

                try
                {
                    _unitOfWork._MenuRepo.Insert(menu);
                    _unitOfWork.Save();
                    SetAlert("Tạo mới menu <b>" + menu.Text + "</b> thành công.", EAlertMessage.Success);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo mới menu! Vui lòng thử lại.");
                }
            }
            LoadDropDownList(menu.TypeID, menu.Target);
            return View(menu);
        }

        public ActionResult Edit(int? id)
        {
            Menu menu = _unitOfWork._MenuRepo.SelectById(id);
            if (menu == null)
            {
                ViewBag.ErrorMessage = "Thông tin menu không tồn tại. Vui lòng thử lại.";
                return View("Error");
            }

            LoadDropDownList(menu.TypeID, menu.Target);
            return View(menu);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(int? id, FormCollection form)
        {
            Menu menu = _unitOfWork._MenuRepo.SelectById(id);

            if (menu == null)
            {
                ViewBag.ErrorMessage = "Thông tin menu không tồn tại. Vui lòng thử lại.";
                return View("Error");
            }
            string oldText = menu.Text;

            if (TryUpdateModel(menu, new string[] { "Text", "Link", "DisplayOrder", "Target", "Status", "TypeID", "Icon" }))
            {
                if (_unitOfWork._MenuRepo.CheckNameIsExists(menu.Text, menu.TypeID.Value) && menu.Text != oldText)
                {
                    ModelState.AddModelError("Text", "Tên menu đã tồn tại");
                    LoadDropDownList(menu.TypeID, menu.Target);
                    return View(menu);
                }

                try
                {
                    _unitOfWork._MenuRepo.Update(menu);
                    _unitOfWork.Save();
                    SetAlert("Cập nhật thông tin menu <b>" + menu.Text + "</b> thành công.", EAlertMessage.Success);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin! Vui lòng thử lại.");
                }
            }
            LoadDropDownList(menu.TypeID, menu.Target);
            return View(menu);
        }

        public void MenuTypeList(object selected = null)
        {
            ViewBag.TypeId = new SelectList(_unitOfWork._MenuTypeRepo.Select(), "Id", "Name", selected);
        }

        public void TargetList(string selected = null)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Mở trong trang", Value = "_self", Selected = selected == "_self" });
            items.Add(new SelectListItem { Text = "Mở trang con", Value = "_blank", Selected = selected == "_blank" });
            ViewBag.Target = items;
        }

        public void LoadDropDownList(object menuTypeSelected = null, string targetSelected = null)
        {
            MenuTypeList(menuTypeSelected);
            TargetList(targetSelected);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}