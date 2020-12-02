using DomainEntities;
using DomainInterface;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SchoolManagementSystem.App_Start;
using SchoolManagementSystem.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace SchoolManagementSystem.Controllers.API
{
     [MobileAuthorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly ILoginRepository loginRepo;
        private readonly IUserRepository userRepo;
        // GET: Login

        public AccountController(ILoginRepository loginRepo, IUserRepository userRepo)
        {
            this.loginRepo = loginRepo;
            this.userRepo = userRepo;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IHttpActionResult Login(UserLogin login)
        {
            try
            {
                var url = ConfigurationManager.AppSettings["Url"];
                if (string.IsNullOrEmpty(login.DeviceIdentifier))
                {
                    throw new ApiException(HttpStatusCode.BadRequest,"UnAusthorized");
                }
                else
                {                                    
                  var userLogin = userRepo.GetUserByUserNameAndPassword(login.UserName, Crypto.OneWayEncryter(login.Password),login.DeviceIdentifier,DeviceType.Mobile);                   var claims = new List<Claim>();
                                        
                if (userLogin == null)
                {
                    throw new ArgumentException("Incorrect UserName or Password");
                }
                if (!string.IsNullOrWhiteSpace(userLogin.ImageUrl) || !string.IsNullOrEmpty(userLogin.ImageUrl))
                {
                    userLogin.ImageUrl = string.Concat(url, userLogin.ImageUrl);
                }
                else
                {
                    userLogin.ImageUrl = "";
                }

                    return Ok(new {DeviceKey = userLogin.DeviceAuthToken,ImageUrl=userLogin.ImageUrl,UserName=login.UserName});
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }           

        }


        [HttpPost]        
        [Route("changepassword")]
        public IHttpActionResult ChangePassword(ChangePassword changePassword)
        {
            try
            {
                var requestModel = "".ToService();
              
                var data = userRepo.ChangePassword(changePassword,requestModel.LoginInfo.ID);
                if (!data)
                {
                    throw new ArgumentException("Old Password Is InCorrect");
                }
                else
                {
                    return Ok(new { Message = "Pasword Changed Successfully" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




    }
}
