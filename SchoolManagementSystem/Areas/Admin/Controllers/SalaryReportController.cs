using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class SalaryReportController : Controller
    {
       
        private readonly IDropDownRepository dropDownRepo;
        private readonly IReportRepository reportsRepo;
        SalaryList downlaoddata = new SalaryList();
        // GET: Admin/SalaryRep
        public SalaryReportController(IDropDownRepository dropDownRepo, IReportRepository reportsRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.reportsRepo = reportsRepo;
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

       
        public ActionResult GetSalaryReport(int year,int month,string Years,string Months)
        {
            var report = new Report()
            {
                Year=year,
                Month=month
            };
            var model = reportsRepo.GetAllSalaryReport(report);
            model.Month = Months;
            model.Year = Years;
            TempData["Result"] = model;
            return PartialView("~/Areas/Admin/Views/SalaryReport/SalaryReportList.cshtml",model);
         
        }

        public ActionResult SalaryReportDownload()
        {

            downlaoddata = (SalaryList)TempData["Result"];
            downlaoddata.IsExport = true;
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=SalaryReport.xls");

            this.Response.ContentType = "application/vnd.ms-excel";

            return View("~/Areas/Admin/Views/SalaryReport/SalaryReportList.cshtml", downlaoddata);

        }

      
    }
}