using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly IUserRepository userRepo;
        // GET: ChangePassword

         public ChangePasswordController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changepass )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var changePass = userRepo.ChangePassword(changepass,null);
                    if(changePass == true)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut();
                        TempData["Success"] = "Password Changed Successfully";                        
                        return RedirectToAction("Index", "Login", new { area = "" });
                    }
                    else
                    {
                        TempData["Failure"] = "Password Cannot Be Changed";
                        return View("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Error Occured";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Occured";
                return RedirectToAction("Index");
            }
        }
    }
}