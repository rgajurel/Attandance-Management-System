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
    public class LeaveTypeController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly ILeaveTypeRepository leaveTypeRepo;
        private string message = "";
        // GET: Admin/LeaveType
        public LeaveTypeController(IMessageHandlerRepository messageHandlerRepo, IDropDownRepository dropDownRepo, ILeaveTypeRepository leaveTypeRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.leaveTypeRepo = leaveTypeRepo;
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


        public void LoadDropDown()
        {
            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

        }
        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allleaveType = leaveTypeRepo.GetAllLeaveType();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allleaveType != null)
            {
                return new JsonResult()
                {
                    Data = allleaveType.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allleaveType,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLeaveType(LeaveType leaveType)
        {
            try
            {
                if (leaveType != null)
                {
                    leaveType.LeaveTypeName = leaveType.LeaveTypeName.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (leaveType.ID > 0)
                        {
                            var savechange = leaveTypeRepo.AddUpdateLeaveType(leaveType);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = leaveTypeRepo.AddUpdateLeaveType(leaveType);
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
        public JsonResult EditLeaveType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editLeaveType = leaveTypeRepo.EditLeaveType(id);
                    return new JsonResult()
                    {
                        Data = editLeaveType,
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

        public JsonResult DeleteLeaveType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = leaveTypeRepo.DeleteLeaveType(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                }
                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
        }
    }
}