using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js",
                         "~/Scripts/dateFormat.js",
                         "~/Scripts/jquery-dateFormat.js",
                         "~/Scripts/jquery-ui-{version}.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
 
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-table.js",
                      "~/Scripts/extensions/editable/bootstrap-table-editable.js",
                      "~/Scripts/extensions/export/bootstrap-table-export.js",
                      "~/Scripts/extensions/filter/bootstrap-table-filter.js",
                      "~/Scripts/extensions/flatJSON/bootstrap-table-flatJSON.js",
                      "~/Scripts/extensions/sorting/bootstrap-table-natural-sorting.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       "~/Content/bootstrap-table.css",
                      "~/Content/sb-admin.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
