using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));

            bundles.Add(new ProperStyleBundle("~/content/smartadmin").Include(
                    "~/Content/css/bootstrap.css",
                    "~/Content/css/font-awesome.css",
                    "~/Content/css/smartadmin-production-plugins.css",
                    "~/Content/css/smartadmin-production.css",
                    "~/Content/css/smartadmin-skins.css",
                    "~/Content/css/demo.css",
                    "~/Content/css/site.css",
                    "~/Content/css/smartadmin-rtl.css",
                    "~/Content/css/invoice.css",
                    "~/Content/css/lockscreen.css",
                    "~/Content/css/your_style.css" ));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/app.config.js",
                "~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.js",
                "~/scripts/bootstrap/bootstrap.js",
                "~/scripts/notification/SmartNotification.js",
                "~/scripts/smartwidgets/jarvis.widget.js",
                "~/scripts/plugin/jquery-form/jquery-form.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.js",
                "~/scripts/plugin/masked-input/jquery.maskedinput.js",
                "~/scripts/plugin/select2/select2.js",
                "~/scripts/plugin/bootstrap-slider/bootstrap-slider.js",
                "~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.js",
                "~/scripts/plugin/msie-fix/jquery.mb.browser.js",
                "~/scripts/plugin/fastclick/fastclick.js",
                "~/Scripts/plugin/jquery.serializejson/jquery.serializejson.js",
                "~/scripts/app.js"));
            //bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
            //    "~/scripts/app.config.js",
            //    "~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
            //    "~/scripts/bootstrap/bootstrap.min.js",
            //    "~/scripts/plugin/moment/moment.js",
            //    "~/scripts/notification/SmartNotification.min.js",
            //    "~/scripts/smartwidgets/jarvis.widget.min.js",
            //    "~/scripts/plugin/jquery-validate/jquery.validate.min.js",
            //    "~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
            //    "~/scripts/plugin/select2/select2.min.js",
            //    "~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
            //    "~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
            //    "~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
            //    "~/scripts/plugin/fastclick/fastclick.min.js",
            //    "~/scripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
                    "~/scripts/plugin/fullcalendar/fullcalendar.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/plugin/morris/morris.min.js",
                "~/scripts/plugin/morris/raphael.min.js",
                "~/scripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/plugin/chartjs/chart.min.js",
                "~/scripts/plugin/highChartCore/highcharts-custom.min.js",
                "~/scripts/plugin/highchartTable/jquery.highchartTable.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/scripts/plugin/datatables/jquery.dataTables.min.js",
                "~/scripts/plugin/datatables/dataTables.colVis.min.js",
                "~/scripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/scripts/plugin/datatable-responsive/datatables.responsive.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/plugin/jqgrid/grid.locale-en.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/scripts/plugin/jquery-form/jquery-form.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.js",
                "~/scripts/plugin/jquery-validate/additional-methods.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.unobtrusive.js",
                "~/scripts/plugin/jquery-form/jquery-form.js"
                
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/scripts/smart-chat-ui/smart.chat.ui.js",
                "~/scripts/smart-chat-ui/smart.chat.manager.js",
                "~/Scripts/smart-chat-ui/signalr.chat.client.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));






            //EasyUI style
            bundles.Add(new ProperStyleBundle("~/plugins/easyuiStyles").Include(
                        "~/Scripts/easyui/themes/insdep/easyui.css"
                         
                      ));
            //EasyUI Script
            //bundles.Add(new ScriptBundle("~/plugins/easyuijs").Include(
            //          "~/Scripts/easyui/jquery.easyui.min.js"));
            //EasyUI plugins Script
            bundles.Add(new ScriptBundle("~/plugins/easyuipluginsjs").Include(
                      "~/Scripts/easyui/plugins/datagrid-filter.js",
                      "~/Scripts/easyui/plugins/jquery.edatagrid.js"));


            //moment Script
            //bundles.Add(new ScriptBundle("~/plugins/momentjs").Include(
            //          "~/Scripts/moment-with-locales.js"));

            //daterangepicker

            bundles.Add(new ScriptBundle("~/plugins/daterangepickerjs").Include(
                      "~/Scripts/plugin/daterangepicker/daterangepicker.js"));
            bundles.Add(new ProperStyleBundle("~/plugins/daterangepickerStyles").Include(
                      "~/Scripts/plugin/daterangepicker/daterangepicker.css"));

            //inputpicker

            //bundles.Add(new ScriptBundle("~/plugins/inputpickerjs").Include(
            //          "~/Scripts/plugin/inputpicker/jquery.inputpicker.js"));
            //bundles.Add(new ProperStyleBundle("~/plugins/inputpickerStyles").Include(
            //          "~/Scripts/plugin/inputpicker/jquery.inputpicker.css"));

            //Bootstrap Search Suggest
            bundles.Add(new ScriptBundle("~/plugins/bootstrapsuggestjs").Include(
                      "~/Scripts/plugin/bootstrap-suggest/bootstrap-suggest.js"));

            //jquery.filer Script
            bundles.Add(new ProperStyleBundle("~/plugins/jqueryfilerStyles").Include(
                      "~/Scripts/plugin/jquery-filer/css/jquery.filer.css",
                      "~/Scripts/plugin/jquery-filer/css/themes/jquery.filer-dragdropbox-theme.css"));
            bundles.Add(new ScriptBundle("~/plugins/jqueryfilerjs").Include(
                      "~/Scripts/plugin/jquery-filer/js/jquery.filer.min.js"));
            //FileSaver Script
            bundles.Add(new ScriptBundle("~/plugins/filejs").Include(
                      "~/Scripts/FileSaver.js",
                      "~/Scripts/jquery.fileDownload.js"
                      ));
            //signalR 2.2.3 Script
            bundles.Add(new ScriptBundle("~/plugins/signalrjs").Include(
                      "~/Scripts/jquery.signalR-2.2.3.js"
                      ));


            //format enum json
            bundles.Add(new ScriptBundle("~/plugins/jqueryeasyuiextendjs").Include(
                      "~/scripts/plugin/moment/moment.js",
                      "~/Scripts/jquery.extend.js",
                      "~/Scripts/jquery.easyui.extend.js",
                      "~/Scripts/jquery.extend.formatter.js"));





            BundleTable.EnableOptimizations = false;

        }
    }


    public class ProperStyleBundle : StyleBundle
    {
        public override IBundleOrderer Orderer
        {
            get { return new NonOrderingBundleOrderer(); }
            set { throw new Exception("Unable to override Non-Ordered bundler"); }
        }

        public ProperStyleBundle(string virtualPath) : base(virtualPath) { }

        public ProperStyleBundle(string virtualPath, string cdnPath) : base(virtualPath, cdnPath) { }

        public override Bundle Include(params string[] virtualPaths)
        {
            foreach (var virtualPath in virtualPaths)
            {
                this.Include(virtualPath);
            }
            return this;
        }

        public override Bundle Include(string virtualPath, params IItemTransform[] transforms)
        {
            var realPath = System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
            if (!File.Exists(realPath))
            {
                throw new FileNotFoundException("Virtual path not found: " + virtualPath);
            }
            var trans = new List<IItemTransform>(transforms).Union(new[] { new ProperCssRewriteUrlTransform(virtualPath) }).ToArray();
            return base.Include(virtualPath, trans);
        }

        // This provides files in the same order as they have been added. 
        private class NonOrderingBundleOrderer : IBundleOrderer
        {
            public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
            {
                return files;
            }
        }

        private class ProperCssRewriteUrlTransform : IItemTransform
        {
            private readonly string _basePath;

            public ProperCssRewriteUrlTransform(string basePath)
            {
                _basePath = basePath.EndsWith("/") ? basePath : VirtualPathUtility.GetDirectory(basePath);
            }

            public string Process(string includedVirtualPath, string input)
            {
                if (includedVirtualPath == null)
                {
                    throw new ArgumentNullException("includedVirtualPath");
                }
                return ConvertUrlsToAbsolute(_basePath, input);
            }

            private static string RebaseUrlToAbsolute(string baseUrl, string url)
            {
                if (string.IsNullOrWhiteSpace(url)
                     || string.IsNullOrWhiteSpace(baseUrl)
                     || url.StartsWith("/", StringComparison.OrdinalIgnoreCase)
                     || url.StartsWith("data:", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    return url;
                }
                if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl = baseUrl + "/";
                }
                if(url.StartsWith("../"))
                {
                    Console.WriteLine(baseUrl, url);
                }
                return VirtualPathUtility.ToAbsolute(baseUrl + url);
            }

            private static string ConvertUrlsToAbsolute(string baseUrl, string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return content;
                }
                //url\\((?!['\"]?(?:data|http):)['\"]?([^'\"\\)]*)['\"]?\\)
                //url\\(['\"]?(?<url>[^)]+?)['\"]?\\)
                return new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)")
                    .Replace(content, (match =>
                                       "url(" + RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value) + ")"));
            }
        }
    }

}
