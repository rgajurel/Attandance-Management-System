using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class AttandanceReportController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IReportRepository reportRepo;
        // GET: Client/AttandanceReport

        public AttandanceReportController(IDropDownRepository dropDownRepo, IReportRepository reportRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.reportRepo = reportRepo;
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public ActionResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }

        public ActionResult GetAttandanceReport(Report report)

        {
            var allreports = reportRepo.GetAllAttandanceReports(report);
            var allleaveholiday = reportRepo.GetAllTakeLeaveAndPublicHoliday(report);
          
            TempData["Organisation"] = allreports.Select(o => o.Organisation).FirstOrDefault();
            var monthyear = allreports.Select(model => new { model.Years, model.Months, model.EmployeeName }).FirstOrDefault();
            TempData["MonthYear"] = "Attandance Report for " + monthyear.Years + " - " + monthyear.Months + " of " + monthyear.EmployeeName;
            ViewBag.dailyattandance = allreports.Where(model => model.IsDailyAttandance == true && model.IsManualAttandance==true).ToList().OrderBy(model=>model.NepaliDateFrom);
            ViewBag.manualattandance = allreports.Where(model => model.IsDailyAttandance == false && model.IsKaaj==false).ToList().OrderBy(model => model.NepaliDateFrom);
            ViewBag.iskaalattandance = allreports.Where(model => model.IsKaaj == true).ToList().OrderBy(model => model.NepaliDateFrom);
            ViewBag.Leave = allleaveholiday.Where(model => model.IsLeave == true).ToList().OrderBy(model => model.NepaliDateFrom);
            ViewBag.PublicHoliday = allleaveholiday.Where(model => model.IsLeave == false).ToList().OrderBy(model => model.NepaliDateFrom);
            
            return View("AttandanceReport");
        }

        public void LoadDropDown()
        {
            var allActiveSession = dropDownRepo.GetActiveSessionDropDown();
            if (allActiveSession != null)
            {
                ViewBag.sessionList = new SelectList(allActiveSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionListAll = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionListAll = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allMonth = dropDownRepo.GetAllMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var notificationTypes = dropDownRepo.GetNotificationTypes();
            if (notificationTypes != null)
            {
                ViewBag.allNotificationTypes = new SelectList(notificationTypes, "NoficationTypeID", "NotificationType");
            }
            else
            {
                ViewBag.allNotificationTypes = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }
    }
}