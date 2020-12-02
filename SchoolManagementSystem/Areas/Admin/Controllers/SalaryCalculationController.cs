using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class SalaryCalculationController : Controller
    {
        private readonly ISalarCalculationRepository salaryCalculationRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        string message = "";

        public SalaryCalculationController(ISalarCalculationRepository salaryCalculationRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.salaryCalculationRepo = salaryCalculationRepo;
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        // GET: Admin/SalaryCalculation
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

            var allMonth = dropDownRepo.GetMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }

        [HttpPost]
        public JsonResult GetEmployeeLeaveInformation(string id,string Year,string Month)
        {
            try
            {
                var employeeLeaveList = salaryCalculationRepo.GetEmployeeLeaveList(id, Year, Month);
                    return new JsonResult()
                    {
                        Data = employeeLeaveList,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    };              


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetEmployeeSalaryList(string id)
        {
            try
            {
                var employeeLeaveList = salaryCalculationRepo.GetEmployeeSalaryInfo(id);
                return new JsonResult()
                {
                    Data = employeeLeaveList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult CalculateTax(string taxableamount,string Employeeid)
        {
            try
            {
                var employeeLeaveList = salaryCalculationRepo.CalculateTax(Convert.ToDecimal(taxableamount), Employeeid);
                return new JsonResult()
                {
                    Data = employeeLeaveList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        
        public JsonResult SaveCalculatedSalary(List<SalaryCalculate> salarycalculate)
        {
            try
            {

                salarycalculate = salarycalculate.Where(x => x.SalHeadingName != "Total Saving").ToList();
                if (salarycalculate != null)
                {
                   
                    var datarequiredfordelete = salarycalculate.FirstOrDefault();
                    salaryCalculationRepo.DeleteData(datarequiredfordelete);

                    int leaveEntry = salaryCalculationRepo.SalaryBatchUpload(salarycalculate);
                    if (leaveEntry > 0)
                    {
                        message = (leaveEntry > 0) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                        return Json(messageHandlerRepo.GetMessage(message));
                    }
                    else
                    {
                        throw new Exception();
                    }
                }


                else
                {
                    throw new Exception(MassageDescription.ExceptionOrNullError);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult GetEmployeeAttandanceInformation(string id, string Year, string Month)
        {
            try
            {
                var employeeAttandanceList = salaryCalculationRepo.AttandanceInformation(id, Year, Month);
                return new JsonResult()
                {
                    Data = employeeAttandanceList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}