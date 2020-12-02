using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DomainEntities
{
    public class ApplicationAuthorizeAttribute : AuthorizeAttribute
    {               
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
       {
            var url = HttpContext.Current.Request.Url.ToString();
              var httpContext = filterContext.HttpContext;
                var request = httpContext.Request;
                var response = httpContext.Response;
                var user = httpContext.User;

                if (request.IsAjaxRequest() && !user.Identity.IsAuthenticated)
                {

                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            status = "302"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.HttpContext.Response.StatusCode = 302;
                    if (user.Identity.IsAuthenticated == false)
                    {
                        filterContext.HttpContext.Response.StatusCode = 302;
                    }

                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }


                    response.SuppressFormsAuthenticationRedirect = true;
                    response.End();
                //  return;
                 // httpContext.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery.ToString()));
            }
                else
                {
                    if (request.IsAuthenticated == false)
                    {
                        httpContext.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery.ToString()));

                    }

                


                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        

        // protected override bool AuthorizeCore(HttpContextBase httpContext)
        // {
        //    var url = httpContext.Request.Url.ToString();
        //    if (url.ToLower().Contains("login"))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (!httpContext.Request.IsAjaxRequest())
        //        {
        //            if (httpContext.Request.IsAuthenticated == false)
        //                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //            else
        //                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        //            httpContext.Response.SuppressFormsAuthenticationRedirect = true;
        //            httpContext.Response.End();
        //        }
        //        else
        //        {
        //            if (httpContext.Request.IsAuthenticated == false)
        //            {
        //                httpContext.Response.Redirect("~/Login/Index");
        //                // return true;
        //            }
        //            else
        //            {
        //                return true;
        //            }

        //        }
        //        httpContext.Response.Redirect("~/Login/Index");

        //    }

        //    return false;
        //}

        //}

        //  protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        // {
        //var httpContext = filterContext.HttpContext;
        //var request = httpContext.Request;
        //var response = httpContext.Response;
        //var user = httpContext.User;

        //if (request.IsAjaxRequest())
        //{
        //    if (user.Identity.IsAuthenticated == false)
        //        response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //    else
        //        response.StatusCode = (int)HttpStatusCode.Forbidden;

        //    response.SuppressFormsAuthenticationRedirect = true;
        //    response.End();
        //}
        //else
        //{

        //}

        //base.HandleUnauthorizedRequest(filterContext);
        // }
    }
}
