using System.Web.Mvc;
using System.Web.Routing;

namespace MobileStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            routes.MapRoute(
                name: "Search Page",
                url: "tim-kiem",
                defaults: new { controller = "Mobile", action = "Search", key = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Complete Order Page",
                url: "thong-tin-don-hang-{orderId}",
                defaults: new { controller = "Checkout", action = "Complete", orderId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Checkout Order Page",
                url: "thong-tin-dat-hang",
                defaults: new { controller = "Checkout", action = "Index" }
            );

            routes.MapRoute(
                name: "Cart Cart Page",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index" }
            );

            routes.MapRoute(
                name: "Account Management Page",
                url: "thong-tin-ca-nhan",
                defaults: new { controller = "Manage", action = "Index"}
            );

            routes.MapRoute(
                name: "Detail Page",
                url: "dien-thoai-{metaTitle}-{id}",
                defaults: new { controller = "DetailProduct", action = "Index", id = UrlParameter.Optional, metaTitle = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Mobile Page",
                url: "dien-thoai/{metaTitle}",
                defaults: new { controller = "Mobile", action = "Index", metaTitle = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Register Page",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "Login Page",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "About Page",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Contact Page",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Contact" },
                namespaces: new[] { "MobileStore.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MobileStore.Controllers" }
            );
        }
    }
}
