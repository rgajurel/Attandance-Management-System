using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class SavingsReportController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IReportRepository reportsRepo;
        SalarySavingList downlaoddata = new SalarySavingList();
        // GET: Admin/SalarySavingList

        public SavingsReportController(IDropDownRepository dropDownRepo, IReportRepository reportsRepo)
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

            var allSavingType = dropDownRepo.GetSalartTypeDropDown();
            if (allSavingType != null)
            {
                ViewBag.allSavingType = new SelectList(allSavingType, "ID", "Name");
            }
            else
            {
                ViewBag.allSavingType = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

           

        }
        public ActionResult GetSalaryReport(string employeeid, string organisationid,string savingstypeid, string Employee, string Organisation, string SavingsType)
        {
            var report = new SavingsReport()
            {
                EmployeeID = employeeid,
                OrganisationID = organisationid,
                SavingsTypeID=savingstypeid
            };
            var model = reportsRepo.GetSalarySavingsReport(report);
            model.Organisation = Organisation;
            model.Employee = Employee;
            model.SavingType = SavingsType;
            TempData["Result"] = model;
            return PartialView("~/Areas/Admin/Views/SavingsReport/SalarySavingsList.cshtml", model);

        }

        public ActionResult SavingsReportDownload()
        {
            downlaoddata = (SalarySavingList)TempData["Result"];
            downlaoddata.IsExport = true;
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=SalarySavingReport.xls");
            this.Response.ContentType = "application/vnd.ms-excel";

            return View("~/Areas/Admin/Views/SavingsReport/SalarySavingsList.cshtml", downlaoddata);

        }
    }
}