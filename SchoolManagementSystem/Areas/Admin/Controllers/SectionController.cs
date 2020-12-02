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
   // [OutputCache(Duration = 300, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]

    public class SectionController : Controller
    {
        private readonly ISectionRepository sectionRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = String.Empty;
        // GET: Admin/Section
       public SectionController(ISectionRepository sectionRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.sectionRepo = sectionRepo;
        }
        [OutputCache(Duration =3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
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
        public JsonResult SaveSection(List<Section> section)
        {
            try
            {
                if (section.Count() > 0)
                {
                    foreach(var  sect in section)
                    {
                        int sectioncount = sectionRepo.GetSectionCount(sect.Name);
                        if (sectioncount > 0)
                        {
                            message = MassageDescription.AlreadyExist;
                            continue;
                        }
                        else
                        {


                            if (sect.ID > 0)
                            {
                                sect.Name = sect.Name.ToUpper();
                                var savechange = sectionRepo.AddUpdateSection(sect);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            }
                            else
                            {
                                sect.Name = sect.Name.ToUpper();
                                var savechange = sectionRepo.AddUpdateSection(sect);
                                message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;

                            }
                        }
                        
                    }
                   
                    return Json(messageHandlerRepo.GetMessage(message));
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    return Json(messageHandlerRepo.GetMessage(message));
                }
            }
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }

            
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allSectionInfo = sectionRepo.GetAllSection();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allSectionInfo != null)
            {
                return new JsonResult()
                {
                    Data = allSectionInfo.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allSectionInfo,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditSection(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSectionType = sectionRepo.EditSection(id);
                    return new JsonResult()
                    {
                        Data = editSectionType,
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

        public JsonResult DeleteSection(string section)
        {
            try
            {
                if (!String.IsNullOrEmpty(section))
                {
                    var savechanges = sectionRepo.DeleteSection(section);
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

        public JsonResult SectionCount(string section)
        {
            try
            {
                if (!String.IsNullOrEmpty(section))
                {
                    var duplicatesection = sectionRepo.GetSectionCount(section);

                    return new JsonResult()
                    {
                        Data = duplicatesection,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    return Json(messageHandlerRepo.GetMessage(message));
                }
            }
            catch(Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
           
        }

    }
}