using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class StatisticsRepository : GenericRepository<Order>
    {
        public StatisticsRepository(EntitiesDbContext db) : base(db) { } 
        UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<SummarySatisticsViewModel> SummaryAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new object[]
                {
                    new SqlParameter("@action", "Summary"),
                    new SqlParameter("@fromDate", fromDate.ToString("MM-dd-yyyy 0:00")),
                    new SqlParameter("@toDate", toDate.ToString("MM-dd-yyyy 23:59"))
                 };
            return await _db.Database.SqlQuery<SummarySatisticsViewModel>("sp_StatisticsInfo @action, @fromDate, @toDate", parameters).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TopSellersViewModel>> TopSellersAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new object[]
                {
                    new SqlParameter("@action", "TopSellers"),
                    new SqlParameter("@fromDate", fromDate.ToString("MM-dd-yyyy 0:00")),
                    new SqlParameter("@toDate", toDate.ToString("MM-dd-yyyy 23:59"))
                 };
            return await _db.Database.SqlQuery<TopSellersViewModel>("sp_StatisticsInfo @action, @fromDate, @toDate", parameters).ToListAsync();
        }

        public async Task<IEnumerable<TopCustomersViewModel>> TopCustomersAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new object[]
                {
                    new SqlParameter("@action", "TopCustomers"),
                    new SqlParameter("@fromDate", fromDate.ToString("MM-dd-yyyy 0:00")),
                    new SqlParameter("@toDate", toDate.ToString("MM-dd-yyyy 23:59"))
                 };
            return await _db.Database.SqlQuery<TopCustomersViewModel>("sp_StatisticsInfo @action, @fromDate, @toDate", parameters).ToListAsync();
        }
    }
}
