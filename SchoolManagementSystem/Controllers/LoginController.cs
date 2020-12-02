using DomainEntities;
using DomainInterface;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILoginRepository loginRepo;
        private readonly IUserRepository userRepo;
        private readonly ISettingsRepository settingRepo;
        private readonly string generalSettingGroup = SettingsGroupName.GeneralGroup;
        // GET: Login

        public LoginController(ILoginRepository loginRepo, IUserRepository userRepo, ISettingsRepository settingRepo)
        {
            this.loginRepo = loginRepo;
            this.userRepo = userRepo;
            this.settingRepo = settingRepo;
        }
        public ActionResult Index(string returnUrl)
        {               
            ViewBag.ReturnUrl = HttpUtility.UrlEncode(returnUrl);
            return View();
        }

      
        
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private void SignIn(List<Claim> claims)//Mind!!! This is System.Security.Claims not WIF claims
        {
            var claimsIdentity = new ClaimsIdentity(claims,
            DefaultAuthenticationTypes.ApplicationCookie);         
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) }, claimsIdentity);          


        }

        [HttpPost]
        public ActionResult Login(UserLogin login,string ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userLogin = userRepo.GetUserByUserNameAndPassword(login.UserName, Crypto.OneWayEncryter(login.Password),null,DeviceType.Web);
                    if (userLogin != null)
                    {
                        var claims = new List<Claim>();
                        bool isSuperAdmin = userLogin.IsSuperAdmin;
                        bool isAdmin = userLogin.IsAdmin;
                        bool isClient = userLogin.IsClientUser;
                        bool isParent = userLogin.IsParentUser;
                        bool isStudent = userLogin.IsStudentUser;
                        var employeeID = userLogin.EmployeeID;
                        var Image = "";

                        if (string.IsNullOrEmpty(employeeID))
                        {
                            if (userLogin.Name.Contains('-'))
                            {
                                employeeID = userLogin.Name.Split('-')[1];
                                Image = loginRepo.GetUserImage(Convert.ToDouble(employeeID))==null?"": loginRepo.GetUserImage(Convert.ToDouble(employeeID));
                            }                           
                           
                        }
                        else
                        {
                           Image= loginRepo.GetUserImage(Convert.ToDouble(employeeID)) == null ? "" : loginRepo.GetUserImage(Convert.ToDouble(employeeID));
                           // Image = loginRepo.GetUserImage(employeeID);
                        }
                     
                       // claims.Add(new Claim(ClaimTypes.Email, userLogin.Email));
                        claims.Add(new Claim(ClaimTypes.Name, login.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, login.UserName));
                        claims.Add(new Claim("Image", Image==null?"":Image));
                        claims.Add(new Claim("isSuperAdmin",userLogin.IsSuperAdmin.ToString()));
                        claims.Add(new Claim("isAdmin", userLogin.IsAdmin.ToString()));
                        claims.Add(new Claim("isClient", userLogin.IsClientUser.ToString()));
                        claims.Add(new Claim("isStudent", userLogin.IsStudentUser.ToString()));
                        claims.Add(new Claim("isParent", userLogin.IsParentUser.ToString()));
                        claims.Add(new Claim("employeeID", employeeID==null?"":employeeID.ToString()));
                        claims.Add(new Claim("ID", userLogin.ID.ToString()));                      
                        
                        SignIn(claims);
                        Session["CheckNepaliDate"] = settingRepo.GetSettingByIDandGroup("1010",generalSettingGroup)=="1"?true:false;
                       Session.Timeout = 50;

                        if (isSuperAdmin == true || isAdmin == true)
                        {
                            if (!String.IsNullOrEmpty((ReturnUrl)))
                            {
                                return Redirect(HttpUtility.UrlDecode(ReturnUrl));
                            }
                            else
                            {
                                return RedirectToAction("Index", "DashBoard", new { Area = "Admin" });
                            }

                           
                        }
                       else if (isClient == true || isParent == true || isStudent == true)
                        {
                            if (!String.IsNullOrEmpty(ReturnUrl))
                            {
                                return Redirect(HttpUtility.UrlDecode(ReturnUrl));
                            }
                            else
                            {
                                return RedirectToAction("Index", "DashBoard", new { Area = "Client" });
                            }
                            
                        }
                        else
                        {
                            return View("Index");

                        }                     
                       
                       

                      
                    }
                    else
                    {
                        TempData["login"] = "Please Enter Valid Credentials";
                        return RedirectToAction("Index");
                    }
                   
                }
                else
                {
                    TempData["Error"] = "Error Occured";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Error Occured";
                return RedirectToAction("Index");
            }
                    
                
          
        }
         
      
     public ActionResult LogOff()
    {
           AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        //public ActionResult ChangePassword()
        //{          
        //    return RedirectToAction("Index", "ChangePassword", new { area = "" });
        //}
    }
}