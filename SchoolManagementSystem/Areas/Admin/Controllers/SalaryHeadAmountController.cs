using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class SalaryHeadAmountController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        // GET: Admin/SalaryHeadAmount
        private readonly ISalaryHeadAmountRepository salaryHeadAmountRepo;

        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";

        public SalaryHeadAmountController(IDropDownRepository dropDownRepo, ISalaryHeadAmountRepository salaryHeadAmountRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.salaryHeadAmountRepo = salaryHeadAmountRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public void LoadDropDown()
        {
                  
            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allSalaryHead = dropDownRepo.GetAllSalaryHead();
            if (allSalaryHead != null)
            {
                ViewBag.allSalaryHead = new SelectList(allSalaryHead, "ID", "Name");
            }
            else
            {
                ViewBag.allSalaryHead = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }


        [HttpPost]
        public JsonResult SaveSalaryHeadAmount(string data,int SalaryHeadID)
        {
            try
            {
                var results = JsonConvert.DeserializeObject<List<SalaryHeadAmount>>(data);

                if (results != null)
                {
                    foreach (var result in results)
                    {
                        if (result.Amount > 0)
                        {
                            result.IsAdded = true;
                        }
                    } 
                        var datarequiredfordelete = results.FirstOrDefault();
                    salaryHeadAmountRepo.DeleteData(datarequiredfordelete, SalaryHeadID);

                        int leaveEntry = salaryHeadAmountRepo.SalaryHeadBatchUpload(results, SalaryHeadID);
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
        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, SalaryHeadAmount salaryHeadAmount)
        {

            var allleaveEntry = salaryHeadAmountRepo.GetAllSalaryHeadAmount(salaryHeadAmount);// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allleaveEntry != null)
            {
                return new JsonResult()
                {
                    Data = allleaveEntry.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allleaveEntry,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }
        public ActionResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }
    }
}