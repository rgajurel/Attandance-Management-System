using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    
    public class DashBoardController : Controller
    {
        private readonly IOrganisationEventsRepository organisationEventsRepo;
        private readonly IDashBoardRepository dashBoardRepo;
        private readonly IUserRole iRole;
        List<Menu> DashBoardMenu = new List<Menu>();

        // GET: Admin/DashBoard


        public DashBoardController(IOrganisationEventsRepository organisationEventsRepo, IUserRole iRole, IDashBoardRepository dashBoardRepo)
        {
            this.organisationEventsRepo = organisationEventsRepo;
            this.dashBoardRepo = dashBoardRepo;
            this.iRole = iRole;
        }
     
        public ActionResult LoadPartialView()
        {
            return PartialView("View");
        }
        public ActionResult Index()
        {
            //var menuID = DomainEntities.Common.GetAllAdminMenuIDs();

            //string loggedInUserName = new LoginUser().UserName;
            //var roles = iRole.MenuGetBasedOnLoggedInUserRole(loggedInUserName);
            //foreach (var role in roles)
            //{
            //    if (menuID.Contains(role.MenuID))
            //    {
            //        DashBoardMenu.Add(role);
            //    }
            //}
            return View();
        }

        public JsonResult GetAllOrganisationEvents()
        {
            var organisationEvents = dashBoardRepo.GetAllOrganisationEvents().OrderByDescending(model => model.ID);
            if (organisationEvents != null || organisationEvents.Count() > 0)
            {
                return Json(organisationEvents);
            }
            else
            {
                return null;
            }
           
        }

        public JsonResult GetAllYearlyHolidayList()
        {
            var yearlyHolidayList = dashBoardRepo.GetAllOrganisationHolidaysList().ToList();
            if (yearlyHolidayList != null || yearlyHolidayList.Count() > 0)
            {
                return Json(yearlyHolidayList);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetAllStudentByClass()
        {
            var classTotal = dashBoardRepo.GetAllTotalStudentbyClass(); ;
            if (classTotal != null || classTotal.Count() > 0)
            {
                return Json(classTotal);
            }
            else
            {
                return null;
            }

        }
        public JsonResult GetAllStudentsAttendance()
        {
            var organisationTotalAttandance = dashBoardRepo.GetAllDailyAttandanceCount().OrderByDescending(model => model.Date);
            if (organisationTotalAttandance != null || organisationTotalAttandance.Count() > 0)
            {
                return Json(organisationTotalAttandance);
            }
            else
            {
                return null;
            }

        }
        public JsonResult GetAllUpComingBirthdays()
        {
            var upComingBirthdays = dashBoardRepo.GetAllUpcomingBirthdays()?.OrderBy(model => model.Date);
            if (upComingBirthdays != null)
            {
                return Json(upComingBirthdays);
            }
            else
            {
                return null;
            }

        }

        public void LoadDashBoard()
        {
            var organisationEvents = dashBoardRepo.GetAllOrganisationEvents().OrderByDescending(model=>model.ID);
            if(organisationEvents!=null || organisationEvents.Count() > 0)
            {
                ViewBag.organisationEvents = organisationEvents;
            }
        }
        
    }
}