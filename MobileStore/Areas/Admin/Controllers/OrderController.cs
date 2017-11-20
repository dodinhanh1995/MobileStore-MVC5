using Models.DAO.Repository;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.QTSortParm = sortOrder == "QT" ? "qt_desc" : "QT";
            ViewBag.TotalSortParm = sortOrder == "total" ? "total_desc" : "total";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var list = _unitOfWork._OrderRepo.GetListAll(sortOrder, searchString, page);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListOrder", list);
            }

            return View(list);
        }

        public ActionResult Detail(int id)
        {
            var detail = _unitOfWork._OrderDetailRepo.GetDetailByOrderId(id);
            if (detail == null)
                return HttpNotFound();
            return View(detail);
        }

        [HttpPost]
        public ActionResult ChangeQuantity(int productId, int orderId, int quantity)
        {
            bool status = _unitOfWork._OrderDetailRepo.UpdateQuantity(productId, orderId, quantity);
            var order = _unitOfWork._OrderDetailRepo.GetDetailByOrderId(orderId);

            return Json(new
            {
                Status = status,
                Amount = order.Amount,
                OrderTotal = order.TotalOrder,
                TotalPrice = order.TotalPrice
            });
        }

        [HttpPost]
        public JsonResult PaidOrdering(int orderId)
        {
            StringBuilder str = new StringBuilder();
            List<string> listName = _unitOfWork._ProductRepo.CheckQuantityIsEnough(orderId);
            bool status = false;
            if (listName.Count > 0)
            {
                foreach (string name in listName)
                {
                    str.Append(name + ", ");
                }
                str = new StringBuilder("Số lượng sản phẩm " + str.ToString() + " hiện tại không đủ để thanh toán. Vui lòng kiểm tra lại");
            }
            else
            {
                status = _unitOfWork._OrderRepo.PaidOrdering(orderId);
            }

            return Json(new { Status = status, ListName = str.ToString() });
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }

   
}