using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Diagnostics;


namespace Vendor
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            // Add a simple test to see if the application is properly configured
            Debug.WriteLine("Application started successfully!");
        }
    }
}