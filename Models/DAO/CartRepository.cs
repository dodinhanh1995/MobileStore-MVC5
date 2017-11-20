using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class CartRepository : GenericRepository<Cart>
    {
        public CartRepository(EntitiesDbContext db) : base(db) { }

        private UnitOfWork _unitOW = new UnitOfWork();

        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        private CartRepository GetCart(HttpContextBase context)
        {
            var cart = new CartRepository(new EntitiesDbContext());
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public CartRepository GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public async Task<Cart> GetRecordByProductIdAsync(int productId)
        {
            var parameter = new object[] {
                new SqlParameter("@action", "GetRecordByProductId"),
                new SqlParameter("@cartId", ShoppingCartId),
                new SqlParameter("@productId", productId)
            };
            return await _table.SqlQuery("sp_Cart @action, @cartId, @productId", parameter).SingleOrDefaultAsync();
        }

        public async Task AddToCart(int productId)
        {
            var cartItem = await GetRecordByProductIdAsync(productId);

            if (cartItem == null)
            {
                decimal price = await _unitOW._ProductRepo.GetPriceOrPromotionPriceById(productId);

                var parameter = new object[] 
                {
                    new SqlParameter("@action", "AddToCart"),
                    new SqlParameter("@cartId", ShoppingCartId),
                    new SqlParameter("@productId", productId),
                    new SqlParameter("@price", price)
                };
                await _db.Database.ExecuteSqlCommandAsync("sp_Cart @action, @cartId, @productId, @price", parameter);
            }
            else
            {
                await ChangeQuantity(cartItem.ProductId, true);
            }
        }

        public async Task<bool> RemoveFromCart(int productId)
        {
            var parameter = new object[]
                {
                    new SqlParameter("@action", "Delete"),
                    new SqlParameter("@cartId", ShoppingCartId),
                    new SqlParameter("@productId", productId)
                };
            return await _db.Database.ExecuteSqlCommandAsync("sp_Cart @action, @cartId, @productId", parameter) > 0;
        }

        public async Task<int> ChangeQuantity(int productId, bool isIncrement)
        {
            var parameter = new object[] {
                new SqlParameter("@action", "UpdateQuantity"),
                new SqlParameter("@cartId", ShoppingCartId),
                new SqlParameter("@productId", productId),
                new SqlParameter("@price", 1),
                new SqlParameter("@count", isIncrement ? 1 : -1),
            };
            return await _db.Database.SqlQuery<int>("sp_Cart @action, @cartId, @productId, @price, @count", parameter).SingleOrDefaultAsync();
        }

        public async Task EmptyCart()
        {
            var parameter = new object[] {
                new SqlParameter("@action", "EmptyCart"),
                new SqlParameter("@cartId", ShoppingCartId),
                new SqlParameter("@productId", 1)
            };
            await _db.Database.ExecuteSqlCommandAsync("sp_Cart @action, @cartId, @productId", parameter);
        }

        public async Task<IEnumerable<CartItemsViewModel>> GetCartItemsAsync()
        {
            var parameter = new object[] {
                new SqlParameter("@action", "GetCartItems"),
                new SqlParameter("@cartId", ShoppingCartId),
                new SqlParameter("@productId", 1),
            };
            return await _db.Database.SqlQuery<CartItemsViewModel>("sp_Cart @action, @cartId, @productId", parameter).ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            var parameter = new object[] {
                new SqlParameter("@action", "GetCount"),
                new SqlParameter("@cartId", ShoppingCartId),
                new SqlParameter("@productId", 1),
            };
            return await _db.Database.SqlQuery<int>("sp_Cart @action, @cartId, @productId", parameter).SingleAsync();
        }

        public async Task<decimal> GetTotalAsync()
        {
            var parameter = new object[] {
                new SqlParameter("@action", "GetCartTotal"),
                new SqlParameter("@cartId", ShoppingCartId),
                new SqlParameter("@productId", 1),
            };
            return await _db.Database.SqlQuery<decimal?>("sp_Cart @action, @cartId, @productId", parameter).SingleOrDefaultAsync() ?? 0;
        }

        public async Task<int> CheckoutOrder(Order order)
        {
            decimal orderTotal = 0;
            foreach (CartItemsViewModel cartItem in await GetCartItemsAsync())
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Count,
                    Price = cartItem.Price,
                    OrderId = order.Id
                };
                orderTotal += (cartItem.Count * cartItem.Price);

                _unitOW._OrderDetailRepo.Insert(orderDetail);
            }
            order.Total = orderTotal;

            _unitOW._OrderRepo.Update(order);

            await _unitOW.SaveAsync();

            await EmptyCart();

            return order.Id;
        }

        private string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrEmpty(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId;
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public async Task MigrateCart(string userName)
        {
            var shoppingCart = await SelectAsync(x => x.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            await _unitOW.SaveAsync();
        }
    }
}