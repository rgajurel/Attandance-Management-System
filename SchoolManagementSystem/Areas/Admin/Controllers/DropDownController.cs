using DomainEntities;
using DomainInterface;
using SchoolManagementSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class DropDownController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        // GET: Admin/DropDown

        public DropDownController(IDropDownRepository dropDownRepo)
        {
            this.dropDownRepo = dropDownRepo;
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOrganisationDropDown()
        {
            var organisation = dropDownRepo.GetAllOrganisation();
            if (organisation != null)
            {
                return Json(organisation, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetYearDropDown()
        {
            var organisation = dropDownRepo.GetSessionDropDown();
            if (organisation != null)
            {
                return Json(organisation, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }
        public JsonResult GetLoginEmployee()
        {
            var organisation = dropDownRepo.GetLoginEmployeeName();
            if (organisation != null)
            {
                return Json(organisation, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetSuperAdminAndAdmin()
        {
            var superAdminAndAdminNames = dropDownRepo.GetSuperAdminAndAdminNames();
            if (superAdminAndAdminNames != null)
            {
                return Json(superAdminAndAdminNames, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetAllLanguage()
        {
            var allLanguage = dropDownRepo.GetAllLanguage();
            if (allLanguage != null)
            {
                return Json(allLanguage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetLeaveTypeBasedOnEmployee(string employeeid)
        {            
            if(employeeid==null || employeeid == null)
            {
                employeeid = Convert.ToString(new LoginUser().LoggedInEmployeeID);
            }
            var organisation = dropDownRepo.GetLeaveTypeBasedOnEmployee(employeeid);
            if (organisation != null)
            {
                return Json(organisation, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        

        public JsonResult GetLeaveDaysMaster()
        {
            var leavedaysmaster = dropDownRepo.GetTakeLeaveDaysMaster();
            if (leavedaysmaster != null)
            {
                return Json(leavedaysmaster, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }
    }
}