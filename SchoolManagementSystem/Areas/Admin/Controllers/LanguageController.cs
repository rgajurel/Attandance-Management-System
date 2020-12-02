using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class LanguageController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly ILanguageRepository languageRepository;
        private string message = "";

        public LanguageController(IMessageHandlerRepository messageHandlerRepo, ILanguageRepository languageRepository)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.languageRepository = languageRepository;
        }
        // GET: Admin/Language
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPartialView()
        {
            return PartialView("View");
        }

        private string UploadImageSave(HttpPostedFileBase file)
        {
            var SchoolPicImage = "";

            if (file != null)
            {
                string schoolPicExtension = Path.GetExtension(file.FileName);
                //var ImageName = Guid.NewGuid();
                file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/Language/" + file.FileName));
                SchoolPicImage = "/Content/Images/Language/" + file.FileName;
            }
            return SchoolPicImage;

        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLanguage(Language language)
        {
            try
            {
                if (language != null)
                {
                   
                    if (ModelState.IsValid)
                    {
                        if (language.ID > 0)
                        {
                            language.Image = UploadImageSave(language.ImageFile);
                            var savechange = languageRepository.AddUpdateLanguage(language);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            language.Image = UploadImageSave(language.ImageFile);
                            var savechange = languageRepository.AddUpdateLanguage(language);
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
                    var savechanges = languageRepository.DeleteLanguage(id);
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
        public JsonResult EditLanguageEntry(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editDesignation = languageRepository.EditLanguage(id);
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

                var allHolidays = languageRepository.GetAllLanguage();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
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