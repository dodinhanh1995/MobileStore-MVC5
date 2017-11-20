using Models.DAO;
using Models.DAO.Repository;
using Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Common.Enum.EDatabase;

namespace MobileStore.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();  

        public async Task<ActionResult> Index()
        {
            return View(new BannerViewModel {
                SlideShows = await _unitOfWork._SlideRepo.GetSlideByPositionAsync(ESlidePosition.SlideShow),
                HomeBanners = await _unitOfWork._SlideRepo.GetSlideByPositionAsync(ESlidePosition.HomeBanner),
                HomeAsides = await _unitOfWork._SlideRepo.GetSlideByPositionAsync(ESlidePosition.HomeAside)
            });
        }

        public PartialViewResult _AdminArea()
        {
            if (User.IsInRole("Administrators"))
            {
                return PartialView(_unitOfWork._MenuRepo.sp_SelectMenyByTypeId(31));
            }
            return PartialView(null);
        }

        [ChildActionOnly, OutputCache(Duration = 86400, VaryByParam = "None")]
        public PartialViewResult _Navigation()
        {
            return PartialView(_unitOfWork._MenuRepo.sp_SelectMenyByTypeId(4));
        }

        public PartialViewResult _Header()
        {
            var cart = _unitOfWork._CartRepo.GetCart(this);
            ViewBag.TopMenu = _unitOfWork._MenuRepo.sp_SelectMenyByTypeId(6);

            return PartialView(new CartStatusToHeaderViewModel
            {
                CartItems = Task.Run(() => cart.GetCartItemsAsync()).Result,
                ItemCount = Task.Run(() => cart.GetCountAsync()).Result,
                CartTotal = Task.Run(() => cart.GetTotalAsync()).Result,
            });
        }

        [ChildActionOnly, OutputCache(Duration = 3600, VaryByParam = "None")]
        public PartialViewResult _ProductsNew()
        {
            return PartialView(_unitOfWork._ProductRepo.sp_ListProductByAction("GetProductsNew"));
        }

        [ChildActionOnly, OutputCache(Duration = 3600, VaryByParam = "None")]
        public PartialViewResult _ProductsFeature()
        {
            return PartialView(_unitOfWork._ProductRepo.sp_ListProductByAction("GetProductsFeature"));
        }

        [ChildActionOnly, OutputCache(Duration = 604800, VaryByParam = "None")]
        public PartialViewResult _Footer()
        {
            return PartialView("_Footer", _unitOfWork._FooterRepo.GetFooter());
        }

        [OutputCache(Duration = 86400, VaryByParam = "None")]
        public ActionResult About()
        {
            return View(_unitOfWork._AboutRepo.SelectById(1));
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}