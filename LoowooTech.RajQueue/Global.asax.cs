using LoowooTech.RajQueue.Managers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LoowooTech.RajQueue
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected virtual void Application_BeginRequest()
        {
            OneContext.Begin();
        }

        protected virtual void Application_EndRequest()
        {
            OneContext.End();
        }
    }
}
