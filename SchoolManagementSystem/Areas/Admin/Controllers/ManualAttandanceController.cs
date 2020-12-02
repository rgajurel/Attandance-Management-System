using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class ManualAttandanceController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IManualAttandanceRepository manualAttandanceRepo;
        private readonly ISettingsRepository settingRepo;
        private string message = "";
        private readonly string generalSettingGroup = SettingsGroupName.GeneralGroup;
        // GET: Admin/ManualAttandance
        public ManualAttandanceController(IDropDownRepository dropDownRepo, IManualAttandanceRepository manualAttandanceRepo, IMessageHandlerRepository messageHandlerRepo, ISettingsRepository settingRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.manualAttandanceRepo = manualAttandanceRepo;
            this.settingRepo = settingRepo;
        }

        public PartialViewResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }
        public void LoadDropDown()
        {

            var allActiveSession = dropDownRepo.GetSessionDropDown();
            if (allActiveSession != null)
            {
                ViewBag.sessionList = new SelectList(allActiveSession, "ID", "Name");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveManualAttandance(Attandance attandance)
        {
            try
            {
                if (attandance != null)
                {
                    var days = (attandance.DateTo.Date - attandance.DateFrom.Date).Days + 1;
                    if (ModelState.IsValid)
                    {                      
                       
                            if (attandance.ID > 0)
                            {
                                var savechange = manualAttandanceRepo.AddUpdateManualAttandance(attandance);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                                //update
                            }
                            else
                            {
                                var isalreadyattend = manualAttandanceRepo.ChekAttandanceAlreadydone(attandance);

                                if (isalreadyattend)
                                {
                                    message = MassageDescription.AttandanceALreadyDone +"-"+ attandance.DateFrom.Date;
                                }
                                else if (attandance.DateTo.Date.Month != attandance.DateFrom.Date.Month)
                                {
                                message = "Cannot Select Date of two different months";
                               }
                                else
                                {
                                    var savechange = manualAttandanceRepo.AddUpdateManualAttandance(attandance);
                                    message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                                }

                                //add
                            }                       
                      

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;

                        //model error occured
                    }
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    //null error occured
                }
                return Json(messageHandlerRepo.GetMessage(message));
            }
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, Attandance search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var employeeList = manualAttandanceRepo.GetAllManualAttandance(search);

            if (employeeList != null)
            {
                var result = new DataSourceResult()
                {
                    Data = employeeList,
                    Total = employeeList.Select(model => model.Total).FirstOrDefault()
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var result = new DataSourceResult()
                {
                    Data = employeeList,
                    Total = 0
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }







        }
    }
}