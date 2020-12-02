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
    public class JobTypeController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IJobTypeRepository jobTypeRepo;
        private string message = "";
        // GET: Admin/JobType
        public JobTypeController(IMessageHandlerRepository messageHandlerRepo, IJobTypeRepository jobTypeRepo, IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.jobTypeRepo = jobTypeRepo;
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
        public JsonResult SaveJobType(JobType jobType)
        {
            try
            {
                if (jobType != null)
                {
                    jobType.JobTypeName = jobType.JobTypeName.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (jobType.ID > 0)
                        {
                            var savechange = jobTypeRepo.AddUpdateJobType(jobType);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = jobTypeRepo.AddUpdateJobType(jobType);
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
        public JsonResult EditJobType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editJobType = jobTypeRepo.EditJobType(id);
                    return new JsonResult()
                    {
                        Data = editJobType,
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

            var allJobType = jobTypeRepo.GetAllJobType();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allJobType != null)
            {
                return new JsonResult()
                {
                    Data = allJobType.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allJobType,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult DeleteJobType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = jobTypeRepo.DeleteJobType(id);
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