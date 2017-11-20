
using Models.DAO;
using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobileStore.Controllers
{
    public class CheckoutController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Order order)
        {
            if (ModelState.IsValid)
            {
                order.CreatedDate = DateTime.Now;
                order.CustomerId = await UserDAO.Instance.GetIdByUserNameAsync(User.Identity.Name);
                order.Status = 0;

                if (await _unitOfWork._OrderRepo.CreateAsync(order))
                {
                    int orderId = await _unitOfWork._CartRepo.GetCart(this).CheckoutOrder(order);

                    try
                    {
                        EmailService es = new EmailService();
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/NewOrder.html"));
                        content = content.Replace("{{CustomerName}}", order.ShipName);
                        content = content.Replace("{{Phone}}", order.ShipMobile);
                        content = content.Replace("{{Email}}", order.ShipEmail);
                        string address = string.Format("{0}, {1}, {2}", order.ShipAddress, DistrictDAO.Instance.GetNameById(order.DistrictId), ProvinceDAO.Instance.GetNameById(order.ProvinceId));
                        content = content.Replace("{{Address}}", address);
                        content = content.Replace("{{Total}}", order.Total.Value.ToString("N0") + "đ");
                        await es.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage
                        {
                            Destination = order.ShipEmail,
                            Subject = "Đơn hàng mới từ AnhDo Mobile",
                            Body = content
                        });

                        //Common.MailHelper.SendMail(order.ShipEmail, "Đơn hàng mới từ AnhDo Mobile", content);
                    }
                    catch
                    {
                        throw new Exception("Gửi mail thất bại.");
                    }
                    return RedirectToAction("Complete", new { orderId = orderId });
                }
                ModelState.AddModelError("", "Có lỗi khi xử lý đơn hàng. Vui lòng thử lại sau.");
            }
            return View(order);
        }

        [Authorize, OutputCache(Duration = int.MaxValue, VaryByParam = "orderId")]
        public async Task<ViewResult> Complete(int orderId)
        {
            if (await _unitOfWork._OrderRepo.ValidateCustomerOwnsOrder(orderId, await UserDAO.Instance.GetIdByUserNameAsync(User.Identity.Name)))
            {
                var order = await _unitOfWork._OrderRepo.SelectByIdAsync(orderId);
                return View(new CompleteOrderViewModel
                {
                    OrderItems = await _unitOfWork._OrderDetailRepo.SelectListByOrderIdAsync(orderId),
                    ShipGender = order.ShipGender,
                    ShipName = order.ShipName,
                    ShipAddress = string.Format("{0}, {1}, {2}", order.ShipAddress, DistrictDAO.Instance.GetNameById(order.DistrictId), ProvinceDAO.Instance.GetNameById(order.ProvinceId)),
                OrderTotal = order.Total.Value
                });
            }
            ViewBag.Error = "Có lỗi xảy ra trong quá trình đặt hàng. Vui lòng thử lại.";
            return View("Error");
        }

        public ActionResult LoadProvince()
        {
            var list = JsonConvert.SerializeObject(ProvinceDAO.Instance.ListAll(),
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }

        public ActionResult LoadDistrict(int provinceId)
        {
            var list = JsonConvert.SerializeObject(DistrictDAO.Instance.ListByProvinceId(provinceId),
            Formatting.None,
            new JsonSerializerSettings()
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