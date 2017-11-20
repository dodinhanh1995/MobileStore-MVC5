using Models.DAO;

namespace Models.ViewModels
{
    public class OrderViewModel
    {
        public int? OrderId { get; set; }
        public bool Gender { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int ProvinceId { private get; set; }
        public int DistrictId { private get; set; }
        public string Address { get; set; }

        public string ShipAddress
        {
            get
            {
                return $"{Address}, {DistrictDAO.Instance.GetNameById(DistrictId)}, {ProvinceDAO.Instance.GetNameById(ProvinceId)}";
            }
        }

        public int? Amount { get; set; }
        public decimal Total { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}