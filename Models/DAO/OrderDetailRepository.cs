using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository(EntitiesDbContext db) : base(db) { }
        private UnitOfWork _unitOW = new UnitOfWork();

        public bool Create(OrderDetail orderDetail)
        {
            try
            {
                Insert(orderDetail);
                Save();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public IEnumerable<OrderDetail> SelectListByOrderId(int orderId)
        {
            return Select(od => od.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderDetail>> SelectListByOrderIdAsync(int orderId)
        {
            return await SelectAsync(od => od.OrderId == orderId);
        }

        public OrderDetailViewModel GetDetailByOrderId(int orderId)
        {
            return Select(o => o.OrderId == orderId).Select(x => new OrderDetailViewModel
            {
                Order = x.Order,
                OrderDetail = x.Order.OrderDetails.ToList(),
                ProvinceId = x.Order.ProvinceId,
                DistrictId = x.Order.DistrictId,
                Address = x.Order.ShipAddress,
                Amount = x.Order.OrderDetails.Sum(od => od.Quantity),
                TotalPrice = (x.Quantity * x.Price),
                TotalOrder = x.Order.Total.Value,
                Gender = x.Order.ShipGender,
                StatusOrder = x.Order.Status
            }).Distinct().FirstOrDefault();
        }

        public bool UpdateQuantity(int productId, int orderId, int quantity)
        {
            try
            {
                var detail = Select(x => x.ProductId == productId && x.OrderId == orderId).Single();
                detail.Quantity = quantity;
                Save();
                decimal total = (decimal)Select(x => x.OrderId == orderId).Select(o => o.Quantity * o.Price).Sum();
                return _unitOW._OrderRepo.UpdateTotalById(orderId, total);
            }
            catch { return false; }
        }
    }
}
