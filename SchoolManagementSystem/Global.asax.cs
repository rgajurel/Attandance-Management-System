using DomainEntities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;


namespace SchoolManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()

        {
            DBManager.CS = ConfigurationManager.ConnectionStrings["SchoolDB"].ConnectionString;
            GlobalConfiguration.Configure(WebApiConfig.Register);
           

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        
            UnityConfig.RegisterComponents();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
             GlobalFilters.Filters.Add(new ApplicationAuthorizeAttribute());
           GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());          
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;       



        }

       

    // protected void Application_BeginRequest()
    //  {
    //    if (!Context.Request.IsSecureConnection)
    //    {
    //        // This is an insecure connection, so redirect to the secure version
    //        UriBuilder uri = new UriBuilder(Context.Request.Url);            

    //        if (uri.Host.Equals("localhost"))
    //        {
    //            uri.Port = 44300;
    //            uri.Scheme = "https";
    //            Response.Redirect(uri.ToString());
    //        }

    //    }

    //}


}
}
