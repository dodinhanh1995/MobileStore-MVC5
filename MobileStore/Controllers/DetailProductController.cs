using Models.DAO;
using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    public class DetailProductController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<ActionResult> Index(int id)
        {
            var _product = _unitOfWork._ProductRepo;
            await _product.IncrementViewCountAsync(id);

            var productDetail = await _product.SelectProductDetailByIdAsync(id);

            var result = new DetailPageViewModel
            {
                Product = productDetail,
                ProductSpecification = await _unitOfWork._ProductSpecificationRepo.sp_SelectByIdAsync(id),
                ProductRelated = await _product.SelectProductRelatedAsync(id, productDetail.Price)
            };

            return View(result);
        }

        [ChildActionOnly, OutputCache(Duration = 86400, VaryByParam = "moreImages")]
        public PartialViewResult _Images(string moreImages)
        {
            return PartialView(_unitOfWork._ProductRepo.ConvertListImages(moreImages));
        }

        public PartialViewResult _Comment(int productId)
        {
            ViewBag.ProductId = productId;
            return PartialView(LoadProductComment(productId, 1));
        }

        public JsonResult LoadComment(int productId, int page)
        {
            var comment = LoadProductComment(productId, page);
            var paging = comment.Paging("/dien-thoai");

            return Json(new
            {
                comment = comment,
                paging = paging
            }, JsonRequestBehavior.AllowGet);
        }

        public PaginatedList<LoadProductCommentViewModel> LoadProductComment(int productId, int page)
        {
            var source = _unitOfWork._CommentRepo.ListByProductId(productId).AsQueryable();
            return new PaginatedList<LoadProductCommentViewModel>(source, page, 7);
        }

        [HttpPost]
        public JsonResult CreateComment(int productId, string content)
        {
            bool isAuth = false;
            Comment comment = new Comment();
            if (User.Identity.IsAuthenticated)
            {
                comment = new Comment
                {
                    ProductId = productId,
                    Content = content.Trim(),
                    UserId = UserDAO.Instance.GetIdByUserName(User.Identity.Name)
                };
                isAuth = true;
            }
            return Json(new { isAuth = isAuth, commentId = _unitOfWork._CommentRepo.sp_InsertComment(comment) });
        }

        [HttpPost]
        public JsonResult CreateCommentReply(int productId, int commentId, string content)
        {
            bool isAuth = false;
            Comment reply = new Comment();
            if (User.Identity.IsAuthenticated)
            {
                reply = new Comment
                {
                    ParentId = commentId,
                    ProductId = productId,
                    Content = content.Trim(),
                    UserId = UserDAO.Instance.GetIdByUserName(User.Identity.Name)
                };
                isAuth = true;
            }
            return Json(new { isAuth = isAuth, commentId = _unitOfWork._CommentRepo.sp_InsertReply(reply) });
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}