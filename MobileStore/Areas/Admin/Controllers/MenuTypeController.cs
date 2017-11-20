using Models.DAO.Repository;
using Models.EF;
using System.Web.Mvc;
using System.Linq;

namespace MobileStore.Areas.Admin.Controllers
{
    public class MenuTypeController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View(_unitOfWork._MenuTypeRepo.Select(orderBy: x=>x.OrderByDescending(mt=>mt.Id)));
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] menuTypeIds = form["menuTypeId"].Split(',');

            foreach (string item in menuTypeIds)
            {
                try
                {
                    foreach (var menu in _unitOfWork._MenuRepo.SelectMenuByTypeId(int.Parse(item)))
                    {
                        _unitOfWork._MenuRepo.Delete(menu);
                    }
                    _unitOfWork._MenuTypeRepo.Delete(int.Parse(item));
                }
                catch
                {

                }
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Create(string name)
        {
            int flag = -1;
            if (_unitOfWork._MenuTypeRepo.CheckNameIsExists(name))
                flag = 2;
            else
            {
                try
                {
                    _unitOfWork._MenuTypeRepo.Insert(new MenuType { Name = name.Trim() });
                    _unitOfWork.Save();
                    flag = 1;
                }
                catch
                {
                }
            }
            return Json(flag);
        }

        [HttpPost]
        public JsonResult Edit(int id, string name)
        {
            var menuType = _unitOfWork._MenuTypeRepo.SelectById(id);
            sbyte flag = -1;
            if (_unitOfWork._MenuTypeRepo.CheckNameIsExists(name) && menuType.Name.ToUpper() != name.Trim().ToUpper())
                flag = 2;
            else
            {
                try
                {
                    menuType.Name = name.Trim();
                    _unitOfWork._MenuTypeRepo.Update(menuType);
                    _unitOfWork.Save();
                    flag = 1;
                }
                catch
                {
                }
            }
            return Json(flag);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}