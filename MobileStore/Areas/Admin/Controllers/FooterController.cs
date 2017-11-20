using Models.DAO.Repository;
using Models.EF;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: Admin/Footer
        public ActionResult Index()
        {
            return View(_unitOfWork._FooterRepo.SelectById("MAIN"));
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id, Content, Status")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork._FooterRepo.Update(footer);
                    _unitOfWork.Save();
                    SetAlert("Cập nhật thành công.", Common.Enum.EAlertMessage.Success);
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Có lỗi xảy ra khi cập nhật! Vui lòng thử lại.", Common.Enum.EAlertMessage.Danger);
                }
            }
            return View("Index", footer);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}