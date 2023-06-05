using System.Web;
using System.Web.Optimization;

namespace Sneaker
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bundle cho trang khách hàng
            //bundles.Add(new StyleBundle("~/bundles/customer-css").Include(
            //    "~/Assets/Customer/css/open-iconic-bootstrap.min.css",
            //    "~/Assets/Customer/css/animate.css",
            //    "~/Assets/Customer/css/owl.carousel.min.css",
            //    "~/Assets/Customer/css/owl.theme.default.min.css",
            //    "~/Assets/Customer/css/magnific-popup.css",
            //    "~/Assets/Customer/css/aos.css",
            //    "~/Assets/Customer/css/ionicons.min.css",
            //    "~/Assets/Customer/css/bootstrap-datepicker.css",
            //    "~/Assets/Customer/css/jquery.timepicker.css",
            //    "~/Assets/Customer/css/flaticon.css",
            //    "~/Assets/Customer/css/icomoon.css",
            //    "~/Assets/Customer/css/style.css",
            //    "~/Assets/Customer/css/bootstrap.min.css"
            //));

            bundles.Add(new ScriptBundle("~/bundles/customer-js").Include(
               "~/Assets/Customer/js/jquery.min.js",
               "~/Assets/Customer/js/jquery-migrate-3.0.1.min.js",
               "~/Assets/Customer/js/popper.min.js",
               "~/Assets/Customer/js/bootstrap.min.js",
               "~/Assets/Customer/js/jquery.easing.1.3.js",
               "~/Assets/Customer/js/jquery.waypoints.min.js",
               "~/Assets/Customer/js/jquery.stellar.min.js",
               "~/Assets/Customer/js/owl.carousel.min.js",
               "~/Assets/Customer/js/jquery.magnific-popup.min.js",
               "~/Assets/Customer/js/aos.js",
               "~/Assets/Customer/js/jquery.animateNumber.min.js",
               "~/Assets/Customer/js/bootstrap-datepicker.js",
               "~/Assets/Customer/js/scrollax.min.js",
               "~/Assets/Customer/js/google-map.js",
               "~/Assets/Customer/js/main.js",
               "~/Assets/Customer/js/range.js",
               "~/Assets/Customer/js/jquery-3.2.1.min.js"
            ));

            // Bundle cho trang quản lý
            //bundles.Add(new StyleBundle("~/bundles/admin-css").Include(
            //    "~/Assets/admin-style1.css",
            //    "~/Assets/admin-style2.css"
            //));
        }
    }
}
