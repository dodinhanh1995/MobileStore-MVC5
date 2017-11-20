using Common.Enum;
using System.Web.Mvc;
using Models.DAO;
using Models.ViewModels;
using System.Linq;

namespace MobileStore.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        public ViewResult Index()
        {
            return View( UserDAO.Instance.ListAllAsync());
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] userIds = form["userId"].Split(',');

            foreach (string item in userIds)
            {
                UserDAO.Instance.Delete(item);

            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser user = UserDAO.Instance.SelectById(id);
            
            if (user == null)
            {
                ViewBag.ErrorMessage = "Thông tin người dùng không tồn tại. Vui lòng thử lại.";
                return View("Error");
            }
            RoleDropDownList(user);
            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string RoleId)
        {
            ApplicationUser user = UserDAO.Instance.SelectById(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Thông tin người dùng không tồn tại. Vui lòng thử lại.";
                return View("Error");
            }

            string oldUsername = user.UserName;
            string oldEmail = user.Email;

            if (TryUpdateModel(user, new string[] { "UserName", "FullName", "PhoneNumber", "Email" }))
            {
                if (UserDAO.Instance.CheckUsernameIsExists(user.UserName) && user.UserName != oldUsername)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                }

                if (UserDAO.Instance.CheckEmailIsExists(user.Email) && user.Email != oldEmail)
                {
                    ModelState.AddModelError("Email", "Địa chỉ email đã tồn tại");
                }
                else
                {
                    try
                    {
                        if (UserDAO.Instance.Update(user, RoleId))
                        {
                            SetAlert("Cập nhật thông tin tài khoản <b>" + user.UserName + "</b> thành công.", EAlertMessage.Success);
                            return RedirectToAction("Index");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin! Vui lòng thử lại.");
                    }
                }
            }
            RoleDropDownList(user);
            return View(user);
        }

        public void RoleDropDownList(ApplicationUser user)
        {
            ViewBag.RoleId = new SelectList(RoleDAO.Instance.ListAll(), "Id", "Name", user.Roles.FirstOrDefault().RoleId);
        }
    }
}