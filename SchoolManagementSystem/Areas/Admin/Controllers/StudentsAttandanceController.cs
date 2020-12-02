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
    public class StudentsAttandanceController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IStudentsAttandanceRepository attandanceRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
       
        private string message = "";

        // GET: Admin/StudentsAttandance
        public StudentsAttandanceController(IDropDownRepository dropDownRepo, IStudentsAttandanceRepository attandanceRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.attandanceRepo = attandanceRepo;
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, StudentsAttandance search)
        {
            try {
                var studetentAttandanceList = attandanceRepo.GetAllMarksStudentsAttandacne(search);

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
            catch(Exception ex)
            {
                return null;
            }

          
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
        public JsonResult SaveStudentsAttandance(string data)
        {
            try
            {
                List<StudentsAttandance> ListWithError = new List<StudentsAttandance>();
                var results = JsonConvert.DeserializeObject<List<StudentsAttandance>>(data);


                if (results != null)
                {
                    foreach (var result in results)
                    {
                        if (result.PresentDays>result.TotalDays|| result.PresentDays<0)
                        {                                                     
                            ListWithError.Add(result);
                        }


                    }
                    if (ListWithError.Count() > 0)
                    {

                        return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.ExceptionOrNullError, ListWithError.OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var datarequiredfordelete = results.FirstOrDefault();
                        attandanceRepo.DeleteData(datarequiredfordelete);

                        int attandancecount = attandanceRepo.AttandanceEntryBatchUpload(results);
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

                }

            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
            }
            return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
        }
    }
}