using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        private readonly ISettingsRepository settingRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly string generalSettingGroup = SettingsGroupName.GeneralGroup;
        private readonly IUserRole userRoleRepo;
        private static List<Menu> AdminMenus;
        // GET: Admin/Menu

        public MenuController(IUserRole userRoleRepo, ISettingsRepository settingRepo, IDropDownRepository dropDownRepo)
        {
            this.userRoleRepo = userRoleRepo;
            this.settingRepo = settingRepo;
            this.dropDownRepo = dropDownRepo;
        }        
        public ActionResult AdminMenu()
        {
            string loggedInUserName = new LoginUser().UserName;
            //if (AdminMenus == null)
            //{
                //AdminMenus = new List<Menu>();
                AdminMenus = userRoleRepo.MenuGetBasedOnLoggedInUserRole(loggedInUserName).ToList();
            //  }
                       

            return View(AdminMenus);
        }

        // GET: Admin/ClientMenu
        public ActionResult ClientMenu()
        {
            string loggedInUserName = new LoginUser().UserName;
            var tt = userRoleRepo.MenuGetBasedOnLoggedInUserRole(loggedInUserName);
            return View(userRoleRepo.MenuGetBasedOnLoggedInUserRole(loggedInUserName));
        }
        [HttpPost]
        public JsonResult GetLoggedInUserName()
        {
            var activesession = dropDownRepo.GetActiveSessionDropDown();
            string activeYear = activesession==null?"":$"Active Economic Year({activesession.FirstOrDefault().Name})";
            bool isNepaliDate = settingRepo.GetSettingByIDandGroup("1010", generalSettingGroup) == "1" ? true : false;            
            return Json(new { isNepaliDate = isNepaliDate, UserName= new LoginUser().UserName,ActiveYear=activeYear }, JsonRequestBehavior.AllowGet);
        }
    }
}