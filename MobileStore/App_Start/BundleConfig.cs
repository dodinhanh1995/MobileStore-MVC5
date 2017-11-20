using System.Web.Optimization;

namespace MobileStore
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Scripts
            bundles.Add(new ScriptBundle("~/bundles/sc-site-core").Include(
                        "~/Scripts/jquery-{version}.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/plugins/mustache.min.js",
                       "~/Assets/Client/js/client.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/sc-mobile").Include(
                        "~/Assets/Client/controller/mobile.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/sc-detailproduct").Include(
                        "~/Assets/Client/controller/productdetail.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/sc-cart").Include(
                        "~/Assets/Client/controller/cart.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/sc-checkout").Include(
                        "~/Assets/Client/controller/checkout.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/sc-site-account-core").Include(
                       "~/Assets/Admin/vendor/metisMenu/metisMenu.min.js",
                       "~/Assets/Admin/dist/js/sb-admin-2.js"
                      ));

            // Styles
            bundles.Add(new StyleBundle("~/bundles/st-site-core").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Assets/Client/css/style.css",
                        "~/Assets/Client/css/responsive.css"));

            bundles.Add(new StyleBundle("~/bundles/st-site-account-core").Include(
                        "~/Assets/Admin/vendor/metisMenu/metisMenu.min.css",
                        "~/Assets/Admin/dist/css/sb-admin-2.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
