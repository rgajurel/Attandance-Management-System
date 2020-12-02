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
    public class ManageCalendarController : Controller
    {
        private readonly IManageCalendarRepository manageCalendarRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/ManageCalendar
        public ManageCalendarController(IManageCalendarRepository manageCalendarRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.manageCalendarRepo = manageCalendarRepo;
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
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
            var allActiveSession = dropDownRepo.GetActiveSessionDropDown();
            if (allActiveSession != null)
            {
                ViewBag.sessionList = new SelectList(allActiveSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
                      

            var allMonth = dropDownRepo.GetAllMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allcalendar = manageCalendarRepo.GetAllManageCalendar();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allcalendar != null)
            {
                return new JsonResult()
                {
                    Data = allcalendar.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allcalendar,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveManageCalendar(ManageCalendar manageCalendar)
        {
            try
            {
                if (manageCalendar != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (manageCalendar.ID > 0)
                        {
                            var savechange = manageCalendarRepo.AddUpdateManageCalendar(manageCalendar);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = manageCalendarRepo.AddUpdateManageCalendar(manageCalendar);
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

        [HttpPost]
        public JsonResult EditManageCalendar(int id)
        {
            try
            {
                if (id != 0)
                {
                    var taxMaster = manageCalendarRepo.EditManageCalendar(id);
                    return new JsonResult()
                    {
                        Data = taxMaster,
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
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteManageCalendar(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = manageCalendarRepo.DeleteManageCalendar(id);
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
                throw ex;
            }
        }
    }
}