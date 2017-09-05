using System.Web;
using System.Web.Optimization;

namespace CoGov
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/history.js",
                        "~/Scripts/retina-1.1.0.js",
                        "~/Scripts/jquery.hoverdir.js",
                        "~/Scripts/jquery.hoverex.min.js",
                        "~/Scripts/jquery.prettyPhoto.js",
                        "~/Scripts/jquery.isotope.min.js",
                        "~/Scripts/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/css").Include(
                      "~/css/bootstrap.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/css/prettyPhoto.css",
                      "~/css/hoverex-all.css",
                      "~/css/site.css",
                      "~/css/font-awesome.min.css"));
        }
    }
}
