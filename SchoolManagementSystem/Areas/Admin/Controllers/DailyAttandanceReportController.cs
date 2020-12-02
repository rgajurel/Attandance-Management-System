using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers.Reports
{
    public class DailyAttandanceReportController : Controller
    {
        private readonly IReportRepository reportsRepo;
        private readonly IDropDownRepository dropDownRepo;
        DailyAttandanceListViewModel downlaoddata = new DailyAttandanceListViewModel();

        public DailyAttandanceReportController(IReportRepository reportsRepo, IDropDownRepository dropDownRepo)
        {
            this.reportsRepo = reportsRepo;
            this.dropDownRepo = dropDownRepo;
        }
        // GET: Admin/DailyAttandanceReport
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

        public ActionResult GetDailyAttandanceReport(int organisationid, string Organisation, DateTime? Date, string NepaliDate, int monthid, string Month, int yearid, string Year)
        {
            var report = new DailyAttandanceReport()
            {
                OrganisationID = organisationid,
                Date = Date,
                Year = yearid,
                Month = monthid


            };
            var model = reportsRepo.GetDailyAttandanceReport(report);
            model.Organisation = Organisation;
            model.Year = Year;
            model.Month = Month;
            model.Date = (bool)Session["CheckNepaliDate"] == true ? NepaliDate : Date?.ToString("yyyy/MM/dd");
            TempData["Result"] = model;
            return PartialView("~/Areas/Admin/Views/DailyAttandanceReport/DailyAttandanceList.cshtml", model);

        }

        public ActionResult DailyAttandanceReportDownload()
        {
            downlaoddata = (DailyAttandanceListViewModel)TempData["Result"];
            downlaoddata.IsExport = true;
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=DailyAttandanceReport.xls");
            this.Response.ContentType = "application/vnd.ms-excel";

            return View("~/Areas/Admin/Views/DailyAttandanceReport/DailyAttandanceList.cshtml", downlaoddata);

        }

        public void LoadDropDown()
        {

            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionList = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
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


        }
    }
}