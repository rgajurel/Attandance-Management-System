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
    public class GradeMasterController : Controller
    {
        private readonly IGradeMasterRepository gradeMasterRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/GradeMaster
        public GradeMasterController(IGradeMasterRepository gradeMasterRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.gradeMasterRepo = gradeMasterRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult LoadPartialView()
        {
            return PartialView("View");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveGradeMaster(GradeMaster grademaster)
        {
            try
            {
                if (grademaster != null)
                {
                    grademaster.Grade = grademaster.Grade.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (grademaster.ID > 0)
                        {
                            var savechange = gradeMasterRepo.AddUpdateGradeMaster(grademaster);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = gradeMasterRepo.AddUpdateGradeMaster(grademaster);
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {


                var gradeMasterInfo = gradeMasterRepo.GetAllGradeMaster();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

                if (gradeMasterInfo != null)
                {
                    return new JsonResult()
                    {
                        Data = gradeMasterInfo.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };

                }
                else
                {
                    return new JsonResult()
                    {
                        Data = gradeMasterInfo,
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

        public JsonResult GetAllGrade()
        {

            var gradeMasterInfo = gradeMasterRepo.GetAllGradeMaster();
          
                return new JsonResult()
                {
                    Data = gradeMasterInfo,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

           
        

        }

        public JsonResult GetAllSubSubject()
        {

            var SubSubject = gradeMasterRepo.GetAllSubSubject();

            return new JsonResult()
            {
                Data = SubSubject,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,

            };




        }

        public JsonResult DeleteGradeMaster(string grade)
        {
            try
            {
                if (!string.IsNullOrEmpty(grade))
                {
                    var savechanges = gradeMasterRepo.DeleteGradeMaster(grade);
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
        public JsonResult EditGradeMaster(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editGradeMaster = gradeMasterRepo.EditGrademaster(id);
                    return new JsonResult()
                    {
                        Data = editGradeMaster,
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

    }
}