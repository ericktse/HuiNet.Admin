using System.Web;
using System.Web.Optimization;

namespace HuiNet.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/hui").Include(
                       "~/Content/hui/lib/jquery/1.10.2/jquery.min.js",
                       "~/Content/hui/lib/layer/2.4/layer.js",
                       "~/Content/hui/static/h-ui/js/H-ui.js",
                       "~/Content/hui/static/h-ui.admin/js/H-ui.admin.js",
                       "~/Content/hui/lib/My97DatePicker/4.8/WdatePicker.js"));
        }
    }
}
