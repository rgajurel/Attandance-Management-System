using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class SalarySlipController : Controller
    {

        private readonly IDropDownRepository dropDownRepo;
        private readonly IReportRepository reportsRepo;
        // GET: Admin/SalarySlip
        public SalarySlipController(IDropDownRepository dropDownRepo, IReportRepository reportsRepo)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GenerateSalarySlip(SalarySlip salarySlip)
        {
            try
            {
               
                var salaryDetails = reportsRepo.GetEmployeeSalaryDetails(salarySlip);
                return new JsonResult()
                {
                    Data = salaryDetails,
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