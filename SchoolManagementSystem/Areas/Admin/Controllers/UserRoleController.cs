using DomainEntities;
using DomainInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public struct RoleObject
    {
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Menus { get; set; }
    }
    public class UserRoleController : Controller
    {


        private readonly IUserRole userRoleRepo;
        // GET: Admin/UserRole

         public UserRoleController(IUserRole userRoleRepo)
        {
            this.userRoleRepo = userRoleRepo;

        }

        #region SaveRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRole(RoleObject obj)
        {
            var xml = JsonConvert.DeserializeXmlNode("{\"Menu\":" + obj.Menus + "}", "root");
            Role oRole = new Role();
            oRole.RoleID = obj.RoleID;
            oRole.Name = obj.Name;
           // LoginUser oLoginUser = new LoginUser();
            return Json(userRoleRepo.RoleMenuSave(oRole, xml.InnerXml, new LoginUser().UserName));
        }
        #endregion
             
        public ActionResult Index()
        {
            IEnumerable<MenuRole> model = userRoleRepo.RoleMenuGet();
            var roles = model.GroupBy(p => p.RoleID).Select(lst => lst.First())
                .Select(x => new { x.Name, x.RoleID, x.Options }).ToList();
            var mnu = new List<dynamic>();
            foreach (var role in roles)
            {
                var menu = model.Where(x => x.RoleID == role.RoleID && x.MenuID > 0).Select(lst => new { lst.MenuID, lst.Options }).ToList();
                dynamic a = new System.Dynamic.ExpandoObject();
                a.Name = role.Name;
                a.RoleID = role.RoleID;
                a.Options = menu?.Count > 0 ? HttpUtility.UrlEncode(JsonConvert.SerializeObject(menu)) : string.Empty;
                mnu.Add(a);
            }
            ViewBag.AdminMenu = userRoleRepo.MenuGet(true);
            ViewBag.ClientMenu = userRoleRepo.MenuGet(false);
            ViewBag.RoleMenu = mnu;
            return View();
        }

        public ActionResult LoadPartialView()
        {
            IEnumerable<MenuRole> model = userRoleRepo.RoleMenuGet();
            var roles = model.GroupBy(p => p.RoleID).Select(lst => lst.First())
                .Select(x => new { x.Name, x.RoleID, x.Options }).ToList();
            var mnu = new List<dynamic>();
            foreach (var role in roles)
            {
                var menu = model.Where(x => x.RoleID == role.RoleID && x.MenuID > 0).Select(lst => new { lst.MenuID, lst.Options }).ToList();
                dynamic a = new System.Dynamic.ExpandoObject();
                a.Name = role.Name;
                a.RoleID = role.RoleID;
                a.Options = menu?.Count > 0 ? HttpUtility.UrlEncode(JsonConvert.SerializeObject(menu)) : string.Empty;
                mnu.Add(a);
            }
            ViewBag.AdminMenu = userRoleRepo.MenuGet(true);
            ViewBag.ClientMenu = userRoleRepo.MenuGet(false);
            ViewBag.RoleMenu = mnu;
            return PartialView("View");
        }
    }
}