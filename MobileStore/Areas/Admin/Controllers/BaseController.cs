using Common.Enum;
using System.Web.Mvc;

namespace MobileStore.Areas.Admin.Controllers
{
    [Authorize(Roles ="Administrators")]
    public class BaseController : Controller
    {
        protected void SetAlert(string message, EAlertMessage? type)
        {
            TempData["AlertMessage"] = message;
            switch (type)
            {
                case EAlertMessage.Danger :
                    TempData["AlertType"] = "danger";
                    break;
                case EAlertMessage.Warning:
                    TempData["AlertType"] = "warning";
                    break;
                case EAlertMessage.Success:
                    TempData["AlertType"] = "success";
                    break;
            }
        }
    }
}