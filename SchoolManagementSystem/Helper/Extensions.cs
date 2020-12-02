using DomainEntities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.Helper
{
    public static class Extensions
    {       
        public static GeneralViewModel<T>ToService<T>(this T model)
        {
            var geocodingkey = ConfigurationManager.AppSettings["GeoCodingKey"];
            var deviceKey = HttpContext.Current.Request.Headers["DeviceKey"];
            var latlong = HttpContext.Current.Request.Headers["LatLong"];
            var appid = "d8t43SG8JoWzV8IZAkXd";
            var appcode = "YVbkGcd6aZHo4VZ1NRsJ2A";
           
            CommonRepository common = new CommonRepository();
            var data = new GeneralViewModel<T>()
            {
                Model = model,
                LatLong = latlong,
                LocationDetails = !string.IsNullOrEmpty(latlong) ? new GeoCodingHelper().GetAddressDetailsByLatLong(latlong, appid,appcode): null
            };
            data.LoginInfo = common.GetLoginInfo(Crypto.Decrypt(deviceKey));
            if (data.LoginInfo == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "UnAuthorized");
            }
            data.LoginInfo.devicetype = DeviceType.Mobile;
            return data;
            
        }

    }
}