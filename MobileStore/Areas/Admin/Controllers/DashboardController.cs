using Models.DAO.Repository;
using Models.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<ActionResult> Index(string date, DateTime? from = null, DateTime? to = null)
        {
            DateTime now = DateTime.Now;
            DateTime fromDate, toDate;
            switch (date)
            {
                case "thisMonth":
                    fromDate = new DateTime(now.Year, now.Month, 1);
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    break;
                case "lastMonth":
                    fromDate = new DateTime(now.Year, now.Month - 1, 1);
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    break;
                case "last7Day":
                    fromDate = now.AddDays(-7);
                    toDate = now;
                    break;
                case "options":
                    fromDate = from.GetValueOrDefault();
                    toDate = to.GetValueOrDefault();
                    break;
                default:
                    fromDate = new DateTime(now.Year, 1, 1);
                    toDate = fromDate.AddYears(1).AddDays(-1);
                    break;
            }

            var list = new StatisticsViewModel
            {
                Summary = await _unitOfWork._StatisticsRepo.SummaryAsync(fromDate, toDate),
                TopSellers = await _unitOfWork._StatisticsRepo.TopSellersAsync(fromDate, toDate),
                TopCustomers = await _unitOfWork._StatisticsRepo.TopCustomersAsync(fromDate, toDate)
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListData", list);
            }

            return View(list);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}