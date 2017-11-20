using Common.Enum;
using Models.DAO.Repository;
using Models.EF;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View(_unitOfWork._AboutRepo.SelectById(1));
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Image, Detail, Status")] About about)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork._AboutRepo.Update(about);
                    _unitOfWork.Save();
                    SetAlert("Cập nhật thông tin thành công!", EAlertMessage.Success);
                    return RedirectToAction("Index");
                }
                catch
                {
                    SetAlert("Có lỗi xảy ra khi cập nhật! Vui lòng thử lại.", EAlertMessage.Danger);
                }
            }
            return View("Index", about);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}