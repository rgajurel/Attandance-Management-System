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
    public class ClassTypeController : Controller
    {
        private readonly IClassTypeRepository classTypeRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";

        public ClassTypeController(IClassTypeRepository classTypeRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.classTypeRepo = classTypeRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        // GET: Admin/ClassType

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
        public JsonResult SaveClassType(ClassType classType)
        {
            try
            {
                if (classType != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (classType.ID > 0)
                        {
                            var savechange = classTypeRepo.AddUpdateClassType(classType);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = classTypeRepo.AddUpdateClassType(classType);
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


                var allClassType = classTypeRepo.GetAllClassType();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allClassType != null)
                {
                    return new JsonResult()
                    {
                        Data = allClassType.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allClassType,
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

        [HttpPost]
        public JsonResult EditClassType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSchoolType = classTypeRepo.EditClassType(id);
                    return new JsonResult()
                    {
                        Data = editSchoolType,
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
        public JsonResult DeleteClassType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = classTypeRepo.DeleteClassType(id);
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
    }
}