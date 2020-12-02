using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class MonthlySummaryReportController : Controller
    {
        private readonly IReportRepository reportsRepo;
        private readonly IDropDownRepository dropDownRepo;
        // GET: Admin/MonthlySummaryReport

        public MonthlySummaryReportController(IReportRepository reportsRepo, IDropDownRepository dropDownRepo)
        {
            this.reportsRepo = reportsRepo;
            this.dropDownRepo = dropDownRepo;      
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerateMonthlySummaryReport(MonthlyAttandanceSummaryReport attandanceSummary)
        {
            try
            {
                var summaryAttandanceDetails = reportsRepo.GetEmployeeMonthlySummaryAttandanceDetails(attandanceSummary);
                return new JsonResult()
                {
                    Data = summaryAttandanceDetails,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
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