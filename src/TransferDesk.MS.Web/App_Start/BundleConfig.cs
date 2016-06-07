using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;

namespace TransferDesk.MS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/bundles/styles")
            .Include("~/Content/jquery-ui-themes.css")
            .Include("~/Content/jquery-ui.css")
                );
            bundles.Add(new StyleBundle("~/Content/jQueryTheme", "http://ajax.microsoft.com/ajax/jquery.ui/1.8.6/themes/smoothness/jquery-ui.css"));
            bundles.Add(new StyleBundle("~/Content/jQueryUI", "//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/modernizr-2.6.2.js")
                .Include("~/Scripts/jquery-1.11.0.js")
                .Include("~/Scripts/bootstrap.min.js")
                .Include("~/Scripts/jquery-ui-1.11.4.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Manuscript")
            .Include("~/Scripts/ExternalScript/jquery-ddlslick.js")
                .Include("~/Scripts/CustomeScript/SearchScript.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/ManuscriptLogin")
        .Include("~/Scripts/CustomeScript/LoginPage.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/ReviewerSuggestion")
        .Include("~/Scripts/CustomeScript/ReviewerSuggestionScript.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/UserRole")
        .Include("~/Scripts/CustomeScript/UserRoleScript.js")
            );


            bundles.Add(new ScriptBundle("~/bundles/JournalScript")
        .Include("~/Scripts/CustomeScript/JournalScript.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/JournalArticleType")
        .Include("~/Scripts/CustomeScript/JournalArticleType.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/JournalSectionType")
      .Include("~/Scripts/CustomeScript/JournalSectionType.js")
          );

            bundles.Add(new ScriptBundle("~/bundles/AdminDashBoard")
   .Include("~/Scripts/CustomeScript/AdminDashBoard.js")
       );

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
        }
    }   
}
