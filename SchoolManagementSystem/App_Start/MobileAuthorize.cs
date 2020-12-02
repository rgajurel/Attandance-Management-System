using DomainEntities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SchoolManagementSystem.App_Start
{
  public class MobileAuthorize:AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            CommonRepository common = new CommonRepository();
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                return;

            var deviceKey = HttpContext.Current.Request.Headers["DeviceKey"];
           
            if (string.IsNullOrEmpty(deviceKey))
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "UnAuthorized");                

            }
            var LoginInfo = common.GetLoginInfo(Crypto.Decrypt(deviceKey));
            if (LoginInfo == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "UnAuthorized");
            }

            return;
            

        }
    }
}