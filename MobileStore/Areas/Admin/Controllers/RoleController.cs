using Microsoft.AspNet.Identity.EntityFramework;
using Models.DAO;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        public ActionResult Index()
        {
            return View(RoleDAO.Instance.ListAll());
        }

        [HttpPost]
        public JsonResult Create(string name)
        {
            int flag = -1;
            if (RoleDAO.Instance.CheckNameIsExists(name))
                flag = 2;
            else if (RoleDAO.Instance.Insert(new IdentityRole { Name = name }))
            {
                flag = 1;
            }
            return Json(flag);
        }

        [HttpPost]
        public JsonResult Edit(string id, string name)
        {
            var role = RoleDAO.Instance.SelectById(id);
            sbyte flag = -1;
            if (RoleDAO.Instance.CheckNameIsExists(name) && role.Name.ToUpper() != name.Trim().ToUpper())
                flag = 2;
            else
            {
                try
                {
                    role.Name = name.Trim();
                    RoleDAO.Instance.Update(role);
                    flag = 1;
                }
                catch
                {
                }
            }
            return Json(flag);
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] roleIds = form["roleId"].Split(',');

            foreach (string item in roleIds)
            {
                RoleDAO.Instance.Delete(item);
            }
            return RedirectToAction("Index");
        }
    }
}
