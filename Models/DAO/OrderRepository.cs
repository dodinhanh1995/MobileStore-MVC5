using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(EntitiesDbContext db) : base(db) { }

        private UnitOfWork _unitOW = new UnitOfWork();

        public IPagedList<OrderViewModel> GetListAll(string sortOrder, string searchString, int? page)
        {
            var model = _table.Select(o => new OrderViewModel
            {
                OrderId = o.Id,
                Gender = o.ShipGender,
                CustomerName = o.ShipName,
                Email = o.ShipEmail,
                Phone = o.ShipMobile,
                Address = o.ShipAddress,
                ProvinceId = o.ProvinceId,
                DistrictId = o.DistrictId,
                Amount = o.OrderDetails.Sum(x => x.Quantity),
                Total = o.Total.Value,
                CreatedDate = o.CreatedDate.Value,
                Status = o.Status
            }).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Address.Contains(searchString) || x.CustomerName.Contains(searchString) || x.Email.Contains(searchString) || x.Phone.Contains(searchString));
            }
            model = sortOrder == "date" ? model.OrderBy(x => x.CreatedDate)
                : sortOrder == "status_desc" ? model.OrderByDescending(x => x.Status)
                : sortOrder == "status" ? model.OrderBy(x => x.Status)
                : sortOrder == "qt_desc" ? model.OrderByDescending(x => x.Amount)
                : sortOrder == "QT" ? model.OrderBy(x => x.Amount)
                : sortOrder == "total" ? model.OrderBy(x => x.Total)
                : sortOrder == "total_desc" ? model.OrderByDescending(x => x.Total)
                : model.OrderByDescending(x => x.CreatedDate);

            return model.ToPagedList(page ?? 1, 10);
        }

        public bool PaidOrdering(int orderId)
        {
            try
            {
                var order = SelectById(orderId);
                foreach (OrderDetail item in _unitOW._OrderDetailRepo.SelectListByOrderId(orderId))
                {
                    _unitOW._ProductRepo.SubtractQuantityForOrder(item.ProductId, item.Quantity.Value);
                }
                order.Status = 1;
                _unitOW.Save();
                Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateAsync(Order order)
        {
            try
            {
                Insert(order);
                await SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTotalById(int id, decimal total)
        {
            var order = SelectById(id);
            try
            {
                order.Total = total;
                Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateCustomerOwnsOrder(int id, string userName)
        {
            return await _table.AnyAsync(o => o.Id == id && o.CustomerId == userName);
        }
    }
}
