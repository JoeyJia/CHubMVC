using System.Web;
using System.Web.Optimization;

namespace CHubMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.form.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                        "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/progressbar").Include(
                        "~/Scripts/bootstrap-progressbar.js"));

            //bundles.Add(new ScriptBundle("~/bundles/boostrapvalidate").Include(
            //            "~/Scripts/bootstrap3-validation.js"));
            bundles.Add(new ScriptBundle("~/bundles/boostrapvalidate").Include(
            "~/Scripts/validator.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-treeview.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-treeview.css"));

            bundles.Add(new StyleBundle("~/Content/datatimecss").Include(
                      "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/progressbarcss").Include(
                      "~/Content/bootstrap-progressbar-3.3.4.css"));

        }
    }
}
