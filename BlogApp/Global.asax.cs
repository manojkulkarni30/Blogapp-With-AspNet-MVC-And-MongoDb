using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BlogApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            EnsureAuthIndexes.Exists();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcHandler.DisableMvcResponseHeader = true;

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}