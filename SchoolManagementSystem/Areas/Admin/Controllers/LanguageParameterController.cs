using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class LanguageParameterController : Controller
    {

        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly ILanguageParameterRepository languageParameterRepository;
        private string message = "";

        public LanguageParameterController(IMessageHandlerRepository messageHandlerRepo, ILanguageParameterRepository languageParameterRepository)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.languageParameterRepository = languageParameterRepository;
        }
        // GET: Admin/LanguageParameter
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadPartialView()
        {
            return PartialView("View");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLanguageParameter(LangaugeParameter languageParameter)
        {
            try
            {
                if (languageParameter != null)
                {

                    if (ModelState.IsValid)
                    {
                        if (languageParameter.ID > 0)
                        {
                            
                            var savechange = languageParameterRepository.AddUpdateLanguageParameter(languageParameter);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                        
                            var savechange = languageParameterRepository.AddUpdateLanguageParameter(languageParameter);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;
                        // return Json(messageHandlerRepo.GetMessage(message));
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
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        [HttpPost]
        public JsonResult DeleteLanguageEntry(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = languageParameterRepository.DeleteLanguageParameter(id);
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

        [HttpPost]
        public JsonResult EditLanguageParameter(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editDesignation = languageParameterRepository.EditLanguageParameter(id);
                    return new JsonResult()
                    {
                        Data = editDesignation,
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


        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {

                var allHolidays = languageParameterRepository.GetAllLanguageParameter();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allHolidays != null)
                {
                    return new JsonResult()
                    {
                        Data = allHolidays.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allHolidays,
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