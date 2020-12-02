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
    public class YearlyHolidayEntryController : Controller
    {       
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IYearlyHolidaysEntryRepository yearlyHolidayRepo;
        private string message = "";
        // GET: Admin/YearlyHolidayEntry

         public YearlyHolidayEntryController(IYearlyHolidaysEntryRepository yearlyHolidayRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.yearlyHolidayRepo = yearlyHolidayRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
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
        public JsonResult SaveHolidayEntry(YearlyHolidaysEntry yearlyholiday)
        {
            try
            {
                if (yearlyholiday != null)
                {
                    yearlyholiday.Title = yearlyholiday.Title.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (yearlyholiday.ID > 0)
                        {
                            var savechange = yearlyHolidayRepo.AddUpdateYearlyHolidaysEntry(yearlyholiday);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = yearlyHolidayRepo.AddUpdateYearlyHolidaysEntry(yearlyholiday);
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
        public JsonResult DeleteHolidayEntry(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = yearlyHolidayRepo.DeleteYearlyHolidaysEntry(id);
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
        public JsonResult EditHolidayEntry(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editDesignation = yearlyHolidayRepo.EditYearlyHolidaysEntry(id);
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

                var allHolidays = yearlyHolidayRepo.GetAllYearlyHolidaysEntry();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
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