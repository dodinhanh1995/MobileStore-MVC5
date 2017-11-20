using Models.DAO;
using Models.DAO.Repository;
using Models.EF;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    public class CommentController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateParam = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.StatusParam = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var list = _unitOfWork._CommentRepo.ListAllCommentPaging(sortOrder, searchString, page);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListComment", list);
            }

            return View(list);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] commentIds = form["commentId"].Split(',');

            foreach (string item in commentIds)
            {
                try
                {
                    _unitOfWork._CommentRepo.Delete(int.Parse(item));
                }
                catch { }
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CreateReply(int id, int productId, string content)
        {
           bool status = _unitOfWork._CommentRepo.sp_InsertReply(new Comment
            {
                ParentId = id,
                Content = content,
                ProductId = productId,
                UserId = UserDAO.Instance.GetIdByUserName(User.Identity.Name)
            });
            return Json("Trả lời phản hồi " + (status ? "thành công" : "thất bại\n Vui lòng thử lại sau."));
        }

        public ActionResult ChangeStatus(int id)
        {
            _unitOfWork._CommentRepo.ChangeStatus(id);
            return RedirectToAction("Index");
        }
       
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}