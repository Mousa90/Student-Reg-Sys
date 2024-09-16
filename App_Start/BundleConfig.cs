using System.Web;
using System.Web.Optimization;

namespace StudentRegSys
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/default-css/bootstrap.css",
                      "~/Content/default-css/site.css"));


            #region Template Desing

            bundles.Add(new ScriptBundle("~/template/js").Include(
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/jquery.easing.js",
                      "~/Scripts/google-code-prettify/prettify.js",
                      "~/Scripts/modernizr.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery.elastislide.js",
                      "~/Scripts/sequence/sequence.jquery-min.js",
                      "~/Scripts/sequence/setting.js",
                      "~/Scripts/jquery.prettyPhoto.js",
                      "~/Scripts/application.js",
                      "~/Scripts/jquery.flexslider.js",
                      "~/Scripts/hover/jquery-hover-effect.js",
                      "~/Scripts/hover/setting.js",
                      "~/Scripts/custom.js",
                      "~/Scripts/datatables/jquery.datatables.js",
                      "~/Scripts/datatables/datatables.bootstrap.js",
                      "~/Scripts/bootbox.js"));

            bundles.Add(new StyleBundle("~/template/css").Include(
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/bootstrap-responsive.css",
                      "~/Content/css/docs.css",
                      "~/Content/css/prettyPhoto.css",
                      "~/Scripts/google-code-prettify/prettify.css",
                      "~/Content/css/flexslider.css",
                      "~/Content/css/sequence.css",
                      "~/Content/css/style.css",
                      "~/Content/color/default.css"));
            #endregion
        }
    }
}
