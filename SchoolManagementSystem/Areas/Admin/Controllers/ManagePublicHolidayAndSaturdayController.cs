using DomainEntities;
using DomainInterface;
using Infrastructure;
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
    public class ManagePublicHolidayAndSaturdayController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IManagePublicHolidayAndSaturday managePublicHolidayRepo;
        private string message = "";
        // GET: Admin/ManagePublicHolidayAndSaturday

            public ManagePublicHolidayAndSaturdayController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, IManagePublicHolidayAndSaturday managePublicHolidayRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.managePublicHolidayRepo = managePublicHolidayRepo;
            this.messageHandlerRepo = messageHandlerRepo;

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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, ManagePublicHoliday search)
        {
            try
            {
                var employeeAttandanceList = managePublicHolidayRepo.GetDailyAttandance(search);
                foreach(var emp in employeeAttandanceList)
                {
                    if (emp.Days == 1)
                    {
                        emp.IsAttend = true;
                    }
                    emp.NepaliDateFrom = DateConversionHelper.GetEnglsihTimeToNepaliDateTime(emp.DateFrom);
                    emp.NepaliDateTo = DateConversionHelper.GetEnglsihTimeToNepaliDateTime(emp.DateTo);
                }

                if (employeeAttandanceList != null || employeeAttandanceList.Count() > 0)
                {
                    return new JsonResult()
                    {
                        Data = employeeAttandanceList.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = employeeAttandanceList,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        [HttpPost]
        public JsonResult SaveManagePublicHoliday(string data,string description,DateTime datefrom,DateTime dateto)
        {
            try
            {
                var results = JsonConvert.DeserializeObject<List<ManagePublicHoliday>>(data);
                foreach (var result in results)
                {
                    result.Description = description;
                    result.DateFrom = datefrom;
                    result.DateTo = dateto;
                   result.NepaliDateFrom = DateConversionHelper.GetEnglsihTimeToNepaliDateTime(datefrom);
                   result.NepaliDateTo = DateConversionHelper.GetEnglsihTimeToNepaliDateTime(dateto);
                    if (result.IsAttend == true)
                    {
                        if (datefrom.Date == dateto.Date)
                        {
                            result.Days = 1;
                        }

                        else
                        {
                            result.Days = dateto.Date.Day - datefrom.Date.Day+1;
                        }
                        
                       
                    }
                               

                }

                if (results != null)
                {
                    if (datefrom.Date > dateto.Date)
                    {
                        message ="Date To Must be Equal Or Greater Than Date From";
                        return Json(messageHandlerRepo.GetMessage(message));
                    }                   

                    var datarequiredfordelete = results.FirstOrDefault();
                    managePublicHolidayRepo.DeleteData(datarequiredfordelete);

                    int attandancecount = managePublicHolidayRepo.AttandanceEntryBatchUpload(results);
                    if (attandancecount > 0)
                    {
                        message = (attandancecount > 0) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                        return Json(messageHandlerRepo.GetMessage(message));
                    }
                    else
                    {
                        message = MassageDescription.ExceptionOrNullError;
                        return Json(messageHandlerRepo.GetMessage(message));
                    }


                }

                return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult GetDescription(ManagePublicHoliday search)
        {
            try
            {
                if (search != null)
                {
                    var getDescription = managePublicHolidayRepo.GetDescription(search);
                    return new JsonResult()
                    {
                        Data = getDescription,
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
    }
}