using System.Web;
using System.Web.Optimization;

namespace TaskManagmentRoleBased
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //made by me for login/signup
            bundles.Add(new StyleBundle("~/Content/themecss").Include(
                      "~/Content/vendor/fontawesome-free/css/all.min.css",
                      "~/Content/css/sb-admin-2.min.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //made by me for login/signup
            bundles.Add(new ScriptBundle("~/bundles/themejs").Include(
                        "~/Content/vendor/jquery/jquery.min.js",
                        "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Content/vendor/jquery-easing/jquery.easing.min.js",
                        "~/Content/js/sb-admin-2.min.js"));

            //made by me for homethemejs
            bundles.Add(new ScriptBundle("~/bundles/Homethemejs").Include(
                "~/Content/vendor/jquery/jquery.min.js",
                "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/vendor/jquery-easing/jquery.easing.min.js",
                "~/Content/js/sb-admin-2.min.js",
                "~/Content/vendor/chart.js/Chart.min.js",
                "~/Content/js/demo/chart-area-demo.js",
                "~/Content/js/demo/chart-pie-demo.js"));

            // made by me for homethemecss
            bundles.Add(new StyleBundle("~/Content/Homethemecss").Include(
                      "~/Content/vendor/fontawesome-free/css/all.min.css",
                      "~/Content/css/sb-admin-2.min.css"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
