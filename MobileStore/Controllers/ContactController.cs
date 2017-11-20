using BotDetect.Web.Mvc;
using Models.DAO.Repository;
using Models.EF;
using System.Web.Mvc;

namespace MobileStore.Controllers
{
    public class ContactController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Contact()
        {
            ViewBag.Maps = _unitOfWork._MapRepo.SelectById(1);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, CaptchaValidation("CaptchaCode", "ContactCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Contact([Bind(Include = "Name, Phone, Email, Content")] Contact contact)
        {
            contact.CreatedDate = System.DateTime.Now;
            contact.Status = false;
            if (ModelState.IsValid)
            {
                if (_unitOfWork._ContactRepo.Create(contact))
                {
                    TempData["Success"] = "Gửi yêu cầu liên hệ thành công.";
                    return RedirectToAction("Contact");
                }
            }
            ViewBag.Maps = _unitOfWork._MapRepo.SelectById(1);
            return View(contact);
        }
    }
}