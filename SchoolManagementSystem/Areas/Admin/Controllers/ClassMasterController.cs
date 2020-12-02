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
    public class ClassMasterController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = String.Empty;
        private IClassMasterRepository classmasterRepo;
        // GET: Admin/ClassMaster
        public ClassMasterController(IMessageHandlerRepository messageHandlerRepo, IClassMasterRepository classmasterRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.classmasterRepo = classmasterRepo;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
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
        public JsonResult SaveClassMaster(List<ClassMaster> classmaster)
        {
            try
            {
                if (classmaster.Count() > 0)
                {
                    foreach (var classs in classmaster)
                    {
                          if (classs.ID > 0)
                            {
                            classs.Name = classs.Name.ToUpper();
                                var savechange = classmasterRepo.AddUpdateClassMaster(classs);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            }
                            else
                            {
                            classs.Name = classs.Name.ToUpper();
                                var savechange = classmasterRepo.AddUpdateClassMaster(classs);
                                message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;

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
            try
            {



                var allClassMasterInfo = classmasterRepo.GetAllClassMaster();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

                if (allClassMasterInfo != null)
                {
                    return new JsonResult()
                    {
                        Data = allClassMasterInfo.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };

                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allClassMasterInfo,
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
        public JsonResult EditClassMaster(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editClassMaster = classmasterRepo.EditClassMaster(id);
                    return new JsonResult()
                    {
                        Data = editClassMaster,
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

        public JsonResult DeleteClassMaster(int ID)
        {
            try
            {
                if (ID!=0)
                {
                    var savechanges = classmasterRepo.DeleteClassMaster(ID);
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