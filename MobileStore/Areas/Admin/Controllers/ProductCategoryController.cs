using System.Web.Mvc;
using Models.EF;
using System.Net;
using Common;
using Common.Enum;
using Models.DAO.Repository;

namespace MobileStore.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View(_unitOfWork._ProductCategoryRepo.Select());
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] categoryIds = form["categoryId"].Split(',');

            foreach (string item in categoryIds)
            {
                try
                {
                    foreach (var product in _unitOfWork._ProductRepo.Select(p=>p.CategoryID.ToString() == item))
                    {
                        _unitOfWork._ProductRepo.Delete(product);
                    }
                    _unitOfWork._ProductCategoryRepo.Delete(int.Parse(item));
                }
                catch { }
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            ParentIdsDropDownList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, ParentID, DisplayOrder, MetaTitle, Status")] ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork._ProductCategoryRepo.CheckNameIsExists(category.Name))
                    ModelState.AddModelError("Name", "Tên danh mục sản phẩm đã tồn tại");
                else
                {
                    try
                    {
                        _unitOfWork._ProductCategoryRepo.Insert(category);
                        _unitOfWork.Save();
                        SetAlert("Tạo mới danh mục <b>" + category.Name + "</b> thành công.", EAlertMessage.Success);
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi tạo mới danh mục! Vui lòng thử lại.");
                    }
                }
            }
            ParentIdsDropDownList(category.ParentID);
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ProductCategory category = _unitOfWork._ProductCategoryRepo.SelectById(id.Value);
            if (category == null)
            {
                ViewBag.ErrorMessage = "Danh mục sản phẩm không tồn tại.";
                return View("Error");
            }

            ParentIdsDropDownList(category.ParentID, category.Id);
            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, FormCollection form)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ProductCategory category = _unitOfWork._ProductCategoryRepo.SelectById(id.Value);
            if (category == null)
            {
                ViewBag.ErrorMessage = "Danh mục sản phẩm không tồn tại.";
                return View("Error");
            }
            string oldName = category.Name.ToUpper();

            if (TryUpdateModel(category, new string[] { "Id", "Name", "ParentID", "DisplayOrder", "MetaTitle", "Status" }))
            {
                if (_unitOfWork._ProductCategoryRepo.CheckNameIsExists(category.Name) && oldName != category.Name.Trim().ToUpper())
                {
                    ModelState.AddModelError("Name", "Tên danh mục sản phẩm đã tồn tại");
                }
                else
                {
                    try
                    {
                        _unitOfWork._ProductCategoryRepo.Update(category);
                        _unitOfWork.Save();
                        SetAlert("Cập nhật thông tin danh mục <b>" + category.Name + "</b> thành công.", EAlertMessage.Success);
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin! Vui lòng thử lại.");
                    }
                }
            }
            ParentIdsDropDownList(category.ParentID, category.Id);
            return View(category);
        }

        public string ConvertMetaTitle(string name)
        {
            return Convertion.ToUnsignString(name);
        }

        public void ParentIdsDropDownList(int? selected = null, int? id = null)
        {
            ViewBag.ParentID = new SelectList(_unitOfWork._ProductCategoryRepo.GetDropDownLists(id), "Id", "Name", selected);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}