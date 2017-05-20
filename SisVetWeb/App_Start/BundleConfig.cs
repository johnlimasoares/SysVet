using System.Web;
using System.Web.Optimization;

namespace SisVetWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.globalize/globalize.js",
                        "~/Scripts/jquery.globalize/cultures/globalize.culture.pt-BR.js",
                        "~/Scripts/jquery.maskedinput*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/methods_pt.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                "~/Scripts/modalform.js"));

            bundles.Add(new ScriptBundle("~/bundles/modalexclusao").Include(
               "~/Scripts/modalexclusao.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/mascaras").Include(
                "~/Scripts/mascaras/mascaras.js"));

            bundles.Add(new ScriptBundle("~/bundles/validacao").Include(
                "~/Scripts/validacao/validacaoCaracteresEspeciais.js"));

            bundles.Add(new ScriptBundle("~/bundles/utils").Include(
                "~/Scripts/utils/utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/utilsMath").Include(
                "~/Scripts/utilsMath/calculaIdade.js"));
        }
    }
}
