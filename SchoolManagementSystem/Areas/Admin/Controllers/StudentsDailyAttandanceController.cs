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
    public class StudentsDailyAttandanceController : Controller
    {

        private readonly IDropDownRepository dropDownRepo;
        private readonly IStudentsDailyAttandanceRepository attandanceDailyRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/StudentsDailyAttandance

        public StudentsDailyAttandanceController(IDropDownRepository dropDownRepo, IStudentsDailyAttandanceRepository attandanceDailyRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.attandanceDailyRepo = attandanceDailyRepo;
            this.messageHandlerRepo = messageHandlerRepo;

        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
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
            var sessionList = dropDownRepo.GetActiveSessionDropDown();
            if (sessionList != null)
            {
                ViewBag.sessionList = new SelectList(sessionList, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var classList = dropDownRepo.GetClasswDropDown();
            if (classList != null)
            {
                ViewBag.classList = new SelectList(classList, "ID", "Name");
            }
            else
            {
                ViewBag.classList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


            var termList = dropDownRepo.GetTermDropDown();
            if (termList != null)
            {
                ViewBag.termList = new SelectList(termList, "ID", "Name");
            }
            else
            {
                ViewBag.termList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
        }


        [HttpPost]
        public JsonResult SaveStudentsDailyAttandance(string data,DateTime eng,DateTime nep)
        {
            try
            {
               
                var results = JsonConvert.DeserializeObject<List<StudentsDailyAttandance>>(data);
                foreach(var result in results)
                {
                    result.Date = eng;
                    result.NepaliDate = nep;
                }

                if (results != null)
                {
                                      
                        var datarequiredfordelete = results.FirstOrDefault();
                        attandanceDailyRepo.DeleteData(datarequiredfordelete);

                        int attandancecount = attandanceDailyRepo.AttandanceEntryBatchUpload(results);
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, StudentsDailyAttandance search)
        {
            try
            {
                var studetentAttandanceList = attandanceDailyRepo.GetDailyAttandance(search);

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
    }
}