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
    public class EmployeeDailyAttandanceController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IEmployeeDailyAttandanceRepository employeeDailyAttandanceRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";

        // GET: Admin/DailyAttandance
        public EmployeeDailyAttandanceController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, IEmployeeDailyAttandanceRepository employeeDailyAttandanceRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.employeeDailyAttandanceRepo = employeeDailyAttandanceRepo;
            this.messageHandlerRepo = messageHandlerRepo;

        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, EmployeeDailyAttandance search)
        {
            try
            {
                var studetentAttandanceList = employeeDailyAttandanceRepo.GetDailyAttandance(search);

              foreach(var student in studetentAttandanceList)
                {
                    if (student.Days == 1)
                    {
                        student.IsAttend = true;
                    }
                }
                if (studetentAttandanceList != null || studetentAttandanceList.Count() > 0)
                {
                    return new JsonResult()
                    {
                        Data = studetentAttandanceList.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = studetentAttandanceList,
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
        public JsonResult SaveEmployeeDailyAttandance(string data,DateTime nepfrom,DateTime nepto,DateTime engfrom,DateTime engto)
        {
            try
            {

                var results = JsonConvert.DeserializeObject<List<EmployeeDailyAttandance>>(data);
                foreach (var result in results)
                {
                    result.NepaliDateFrom=nepfrom;
                    result.DateFrom = engfrom;
                    result.NepaliDateTo = nepto;
                    result.DateTo = engto;
                    result.IsDailyAttandance = true;
                    result.IsManualAttandance = false;
                    result.IsKaaj = false;
                   // result.Hours = ((TimeSpan)result.EntryimeString - result.EntryTime).TotalHours;
                    result.ExtraHours = 0;
                    
                }

                if (results != null)
                {

                    var datarequiredfordelete = results.FirstOrDefault();
                    employeeDailyAttandanceRepo.DeleteData(datarequiredfordelete);

                    int attandancecount = employeeDailyAttandanceRepo.AttandanceEntryBatchUpload(results);
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

    }
}