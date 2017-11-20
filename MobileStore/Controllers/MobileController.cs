using Models.DAO.Repository;
using System.Web.Mvc;
using Models.ViewModels;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    public class MobileController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        [OutputCache(Duration = 600, VaryByParam ="*")]
        public async Task<ActionResult> Index(string metaTitle = "", int page = 1, string orderBy = "")
        {
            ViewBag.SortOrder = orderBy;
            ViewBag.MetaTitle = metaTitle;
            if (Request.IsAjaxRequest())
            {
                return Json(await LoadProducts(metaTitle, orderBy, page, 10), JsonRequestBehavior.AllowGet);
            }
            ViewBag.HasPaging = string.IsNullOrEmpty(metaTitle) ? true : false;
            ViewBag.BrandList = await _unitOfWork._ProductCategoryRepo.sp_GetListCategoryNameAsync();
            return View(await LoadProducts(metaTitle, orderBy, page));
        }

        [ChildActionOnly, OutputCache(Duration = 86400, VaryByParam = "None")]
        public PartialViewResult _Banner()
        {
            return PartialView(_unitOfWork._SlideRepo.GetSlideByPosition(Common.Enum.EDatabase.ESlidePosition.PhoneTop));
        }

        public async Task<IEnumerable<ProductBriefViewModel>> LoadProducts(string metaTitle, string orderBy, int page, int pageSize = 9)
        {
            return await _unitOfWork._ProductRepo.SelectProductByFilterHasPagingAsync(metaTitle, orderBy, page, pageSize);
        }

        [OutputCache(Duration = 600, VaryByParam ="key")]
        public async Task<ActionResult> Search(string key)
        {
            ViewBag.Key = key;
            return View(await _unitOfWork._ProductRepo.ListProductByKeywordAsync(key));
        }

        public async Task<ActionResult> LoadProductName(string key)
        {
            var list = JsonConvert.SerializeObject(await _unitOfWork._ProductRepo.ListNameByKeywordAsync(key), Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}