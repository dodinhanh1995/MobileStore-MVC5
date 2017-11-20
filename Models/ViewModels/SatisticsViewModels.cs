using System.Collections.Generic;

namespace Models.ViewModels
{
    public class SummarySatisticsViewModel
    {
        public int TotalOrder { get; set; }
        public int TotalProduct { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? RevenueAverage { get; set; }
    }

    public class TopSellersViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public int Amount { get; set; }
        public decimal? Revenue { get; set; }
    }

    public class TopCustomersViewModel
    {
        internal bool ShipGender { get; set; }
        internal string FullName { get; set; }

        public string CustomerName
        {
            get
            {
                return (ShipGender ? "anh " : "chị ") + FullName;
            }
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalOrder { get; set; }
        public decimal? Revenue { get; set; }
    }

    public class StatisticsViewModel
    {
        public SummarySatisticsViewModel Summary { get; set; }
        public IEnumerable<TopSellersViewModel> TopSellers { get; set; }
        public IEnumerable<TopCustomersViewModel> TopCustomers { get; set; }
    }
}
