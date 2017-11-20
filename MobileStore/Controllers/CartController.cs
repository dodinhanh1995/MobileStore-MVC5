using Models.ViewModels;
using Models.DAO.Repository;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    public class CartController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<ActionResult> Index()
        {
            var cart = _unitOfWork._CartRepo.GetCart(this);

            var ViewModel = new CartViewModel
            {
                CartItems = await cart.GetCartItemsAsync(),
                CartTotal = await cart.GetTotalAsync()
            };

            return View(ViewModel);
        }

        public async Task<ActionResult> AddToCart(int productId)
        {
            var cart = _unitOfWork._CartRepo.GetCart(this);
            await cart.AddToCart(productId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var cart = _unitOfWork._CartRepo.GetCart(this);

            var results = new CartRemoveViewModel
            {
                Status = await cart.RemoveFromCart(productId),
                CartItems = await cart.GetCartItemsAsync(),
                ItemCount = await cart.GetCountAsync(),
                CartTotal = await cart.GetTotalAsync()
            };

            var list = JsonConvert.SerializeObject(results, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }

        [HttpPost]
        public async Task<ActionResult> ChangeQuantityFromCart(int productId, bool flag, int quantity)
        {
            var cart = _unitOfWork._CartRepo.GetCart(this);
            CartRemoveViewModel results;
            if (flag && await _unitOfWork._ProductRepo.CheckCurrentQuantity(productId, quantity))
            {
                results = new CartRemoveViewModel
                {
                    Status = true
                };
            }
            else
            {
                results = new CartRemoveViewModel
                {
                    ItemCount = await cart.ChangeQuantity(productId, flag),
                    CartItems = await cart.GetCartItemsAsync(),
                    CartTotal = await cart.GetTotalAsync(),
                };
            }

            var list = JsonConvert.SerializeObject(results, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Content(list, "application/json");
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}