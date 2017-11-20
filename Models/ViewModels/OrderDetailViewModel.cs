using Models.DAO;
using Models.EF;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public virtual Order Order { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
        public int ProvinceId { private get; set; }
        public int DistrictId { private get; set; }
        public string Address { private get; set; }

        public string ShipAddress
        {
            get
            {
                return string.Format("{0}, {1}, {2}",
                Address, DistrictDAO.Instance.GetNameById(DistrictId), ProvinceDAO.Instance.GetNameById(ProvinceId));
            }
        }


        public int? Amount { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal TotalOrder { get; set; }
        public bool Gender { get; set; }
        public int StatusOrder { get; set; }
    }
}
