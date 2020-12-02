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
    public class LeaveEntryController : Controller
    {
        // GET: Admin/LeaveEntry
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly ILeavEntryRepository leaveEntryRepo;
        private string message = "";
        public LeaveEntryController(IMessageHandlerRepository messageHandlerRepo, ILeavEntryRepository leaveEntryRepo, IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.leaveEntryRepo = leaveEntryRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public void LoadDropDown()
        {

            var sessionList = dropDownRepo.GetActiveSessionDropDown();
            if (sessionList != null)
            {
                ViewBag.sessionList = new SelectList(sessionList, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allLeaveType = dropDownRepo.GetAllLeaveType();
            if (allLeaveType != null)
            {
                ViewBag.allLeaveType = new SelectList(allLeaveType, "ID", "Name");
            }
            else
            {
                ViewBag.allLeaveType = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

        }
        public ActionResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }

        [HttpPost]
        public JsonResult GetLeaveTypeBasedOnOrganisation(string ID)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    var leaveType = leaveEntryRepo.GetLeaveTypeBasedOnOrganisation(Convert.ToString(ID));
                    return new JsonResult()
                    {
                        Data = leaveType,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request,LeaveEntry leaveEntry)
        {

            var allleaveEntry =leaveEntryRepo.GetAllLeaveEntry(leaveEntry);// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
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

        [HttpPost]
        public JsonResult SaveLeaveEntry(string data,int Year)
        {
            try
            {               
                var results = JsonConvert.DeserializeObject<List<LeaveEntry>>(data);

                if (results != null)
                {
                    if(results.Any(model => model.TotalDays < 0|| model.TotalDayInMonth<0))
                   {
                        throw new Exception(MassageDescription.ExceptionOrNullError);
                    }             
                     
                    else
                    {
                        var datarequiredfordelete = results.FirstOrDefault();
                        leaveEntryRepo.DeleteData(datarequiredfordelete, Year);

                        int leaveEntry = leaveEntryRepo.LeaveEntryBatchUpload(results,Year);
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
    }
}