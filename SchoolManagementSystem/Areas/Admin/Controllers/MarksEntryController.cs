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
    public class MarksEntryController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMarksEntryRepository marksRepo;
        private string message = "";
        // GET: Admin/MarksEntry
        public MarksEntryController(IMessageHandlerRepository messageHandlerRepo, IMarksEntryRepository marksRepo, IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.marksRepo = marksRepo;
        }
      // [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }


        public PartialViewResult LoadPartialView()
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
        public JsonResult GetFullMarksPassMarks(MarksEntry marksEntry)
        {
            try
            {
                if (marksEntry != null)
                {
                    var fullMarksPassMarks = marksRepo.GetFullMarksPassMaeks(marksEntry);
                    if (fullMarksPassMarks != null)
                    {
                        return new JsonResult()
                        {
                            Data = fullMarksPassMarks,
                            ContentType = "application/json",
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                        };
                    }
                    else
                    {
                        return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSubjectBasedOnClass(string ID)
        {
            try
            {
                if (ID != null)
                {
                    var subjects = marksRepo.GetSubjectBasedOnClass(ID);
                    return new JsonResult()
                    {
                        Data = subjects,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveMarksEntry(string data)
        {
            try
            {
                List<MarksEntry> ListWithError = new List<MarksEntry>();
            var results = JsonConvert.DeserializeObject<List<MarksEntry>>(data);

          
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        result.ObtainedMarksTheory = Math.Round(result.ObtainedMarksTheory);
                        if (result.ObtainedMarksTheory > result.FullMarksTheory || result.ObtainedMarksPractical > result.FullMarksPractical)
                        {
                            result.ObtainedGradeTheory = "";
                            result.ObtaindedGradePractical = "";
                            result.FinalGrade = "";
                            result.GradePoint = "";
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
                           marksRepo.DeleteData(datarequiredfordelete);
                      
                         int markscount = marksRepo.MarksEntryBatchUpload(results.Where(model=>model.IsAdmin==true).ToList());
                        if (markscount > 0)
                        {
                            message = (markscount > 0) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
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
            catch(Exception ex)
            {
                return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError,null), JsonRequestBehavior.AllowGet);
            }
            return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMarksEntry(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = marksRepo.DeleteMarksEntryInfo(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                }
                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, MarksEntry search)
        {
            try {

                var marksList = marksRepo.GetAllMarksEntry(search);

                if (marksList != null || marksList.Count() > 0)
                {
                    return new JsonResult()
                    {
                        Data = marksList.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = marksList,
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
    }
}