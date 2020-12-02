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
    public class LeaveDaysController : Controller
    {
        private readonly ILeaveDaysRepository leaveDaysRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/LeaveDays
        public LeaveDaysController(ILeaveDaysRepository leaveDaysRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.leaveDaysRepo = leaveDaysRepo;
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
        public JsonResult SaveLeaveDays(LeaveDays leaveDays)
        {
            try
            {
                if (leaveDays != null)
                {
                    if (ModelState.IsValid)
                    {
                        leaveDays.Name = leaveDays.Name.ToUpper();
                        if (leaveDays.ID > 0)
                        {
                            var savechange = leaveDaysRepo.AddUpdateLeaveType(leaveDays);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = leaveDaysRepo.AddUpdateLeaveType(leaveDays);
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
                throw ex;

            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allLeaveType = leaveDaysRepo.GetAllLeaveType();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allLeaveType != null)
            {
                return new JsonResult()
                {
                    Data = allLeaveType.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allLeaveType,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult DeleteLeaveDays(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = leaveDaysRepo.DeleteLeaveType(id);
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
        public JsonResult EditLeaveDays(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editLeaveDays = leaveDaysRepo.EditLeaveType(id);
                    return new JsonResult()
                    {
                        Data = editLeaveDays,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}