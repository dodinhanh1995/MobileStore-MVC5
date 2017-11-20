using System.Web.Mvc;
using Models.EF;
using Common.Enum;
using Models.DAO.Repository;

namespace MobileStore.Areas.Admin.Controllers
{
    public class MapController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            Map map = _unitOfWork._MapRepo.SelectById(1);
            if (map == null)
            {
                ViewBag.ErrorMessage = "Thông tin bản đồ không tồn tại. Vui lòng thử lại.";
                return View("Error");
            }
            return View(map);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Index")]
        public ActionResult Edit()
        {
            Map map = _unitOfWork._MapRepo.SelectById(1);
            if (TryUpdateModel(map, new string[] { "Name", "APIKey", "Latitude", "Longitude", "Description", "Status" }))
            {
                try
                {
                    _unitOfWork._MapRepo.Update(map);
                    _unitOfWork.Save();
                    SetAlert("Cập nhật thông tin bản đồ <b>" + map.Name + "</b> thành công.", EAlertMessage.Success);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin! Vui lòng thử lại.");
                }
            }
            return View(map);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
