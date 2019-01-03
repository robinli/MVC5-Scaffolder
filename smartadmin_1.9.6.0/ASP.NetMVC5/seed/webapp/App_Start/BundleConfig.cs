#region Using

using System.Web.Optimization;

#endregion

namespace SmartAdminMvc
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/app.config.seed.min.js",
                "~/scripts/bootstrap/bootstrap.min.js",
                "~/scripts/app.seed.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}