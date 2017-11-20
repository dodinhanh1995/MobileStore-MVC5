using Common.Enum;
using Models.DAO.Repository;
using Models.EF;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace MobileStore.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameParam = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.VATParam = sortOrder == "vat" ? "vat_desc" : "vat";
            ViewBag.QuantityParam = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            ViewBag.CategoryParam = sortOrder == "category" ? "category_desc" : "category";
            ViewBag.DateParam = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.TopHotParam = sortOrder == "topHot" ? "topHot_desc" : "topHot";
            ViewBag.StatusParam = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var list = _unitOfWork._ProductRepo.ListAllProductPaging(sortOrder, searchString, page);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListProduct", list);
            }

            return View(list);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] productIds = form["productId"].Split(',');

            foreach (string item in productIds)
            {
                try
                {
                    _unitOfWork._ProductSpecificationRepo.Delete(int.Parse(item));
                    _unitOfWork._ProductRepo.Delete(int.Parse(item));
                }
                catch { }
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            CategoryList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Code, Name, Detail, Description, Image, Price, PromotionPrice, IncludeVAT, Quantity, CategoryID, MetaTitle, Status, TopHot")] Product product)
        {
            product.CreatedBy = User.Identity.Name;
            if (ModelState.IsValid)
            {
                if (_unitOfWork._ProductRepo.CheckNameIsExists(product.Name))
                    ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                else
                {
                    try
                    {
                        if (_unitOfWork._ProductRepo.sp_InsertByStoreProcedure(product))
                        {
                            SetAlert("Tạo mới sản phẩm <b>" + product.Name + "</b> thành công.", EAlertMessage.Success);
                            return RedirectToAction("Index");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi tạo mới sản phẩm! Vui lòng thử lại.");
                    }

                }
            }
            CategoryList(product.CategoryID);
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            Product product = FindProductById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Thông tin sản phẩm không tồn tại.";
                return View("Error");
            }

            CategoryList(product.CategoryID);
            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection form)
        {
            Product product = FindProductById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Thông tin sản phẩm không tồn tại.";
                return View("Error");
            }
            string oldName = product.Name;
            var includes = new string[]
            {
                "Name", "Code", "Detail", "Description", "Image", "Price",
                "PromotionPrice", "IncludeVAT", "Quantity", "CategoryID",
                "CreatedBy", "MetaTitle", "TopHot", "Status"
            };

            if (TryUpdateModel(product, includes))
            {
                if (_unitOfWork._ProductRepo.CheckNameIsExists(product.Name) && oldName != product.Name)
                    ModelState.AddModelError("Name", "Tên sản phẩm đã tồn tại");
                else
                {
                    try
                    {
                        if (_unitOfWork._ProductRepo.sp_UpdateByStoreProcedure(product))
                        {
                            SetAlert("Cập nhật thông tin sản phẩm <b>" + product.Name + "</b> thành công.", EAlertMessage.Success);
                            return RedirectToAction("Index");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin! Vui lòng thử lại.");
                    }

                }
            }
            CategoryList(product.CategoryID);
            return View(product);
        }

        public ActionResult Specification(int id)
        {
            var specification = FindSpecificationById(id);
            if (specification == null)
            {
                ViewBag.ErrorMessage = "Thông số kỹ thuật của sản phẩm không tồn tại.";
                return View("Error");
            }
            ViewBag.ProductName = _unitOfWork._ProductRepo.SelectNameById(id);
            return View(specification);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Specification(int id, FormCollection form)
        {
            var specification = FindSpecificationById(id);
            if (specification == null)
            {
                ViewBag.ErrorMessage = "Thông số kỹ thuật của sản phẩm không tồn tại.";
                return View("Error");
            }

            var includes = new string[] { "Screen", "OperatingSystem", "CameraAfter", "CameraBefore", "CPU", "RAM", "InternalMemory", "Sim", "PinCapacity" };

            if (TryUpdateModel(specification, includes))
            {
                try
                {
                    _unitOfWork._ProductSpecificationRepo.Update(specification);
                    _unitOfWork.Save();
                    SetAlert("Cập nhật thông số kỹ thuật <b>" + FindProductById(id).Name + "</b> thành công.", EAlertMessage.Success);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông số kỹ thuật! Vui lòng thử lại.");
                }
            }
            return View(specification);
        }

        public ActionResult ImageManager(int id)
        {
            var product = _unitOfWork._ProductRepo.SelectById(id);
            ViewBag.ProductId = id;
            ViewBag.ProductName = _unitOfWork._ProductRepo.SelectNameById(id);
            return View(_unitOfWork._ProductRepo.ConvertListImages(_unitOfWork._ProductRepo.SelectMoreImagesById(id)));
        }

        public JsonResult SaveImages(int id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var imageList = serializer.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");
            
            foreach (var item in imageList)
            {
                xElement.Add(new XElement("Image", item));
            }

            return Json(new { status = _unitOfWork._ProductRepo.UpdateMoreImages(id, xElement.ToString()) });
        }

        #region Helpers
        public Product FindProductById(int id)
        {
            var product = _unitOfWork._ProductRepo.SelectById(id);
            return product == null ? null : product;
        }

        public ProductSpecification FindSpecificationById(int id)
        {
            var specification = _unitOfWork._ProductSpecificationRepo.SelectById(id);
            return specification == null ? null : specification;
        }

        public string ConvertMetaTitle(string name)
        {
            return Common.Convertion.ToUnsignString(name);
        }

        public string ChangeImage(int? id, string image)
        {
            if (id == null)
                return "Mã sản phẩm không tồn tại!";
            return _unitOfWork._ProductRepo.ChangingImage(id.Value, image);
        }

        public void CategoryList(object selected = null)
        {
            ViewBag.CategoryId = new SelectList(_unitOfWork._ProductCategoryRepo.GetDropDownLists(), "Id", "Name", selected);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}