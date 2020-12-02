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
    public class MonthsController : Controller
    {

        private readonly IMonthsRepository monthsRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/Months

        public MonthsController(IMessageHandlerRepository messageHandlerRepo, IMonthsRepository monthsRepo)
        {
            this.monthsRepo = monthsRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult LoadPartialView()
        {
            return PartialView("View");
        }


        [HttpPost]
        public JsonResult CheckIfSessionAlreadyActive(string IsActive)
        {
            try
            {
                if (!String.IsNullOrEmpty(IsActive) && IsActive == "1")
                {
                    var mainbranchcount = monthsRepo.GetAllMonthsInfo().Where(model => model.IsActive == IsActive).Count();
                    if (mainbranchcount != 0)
                    {
                        return Json(messageHandlerRepo.GetMessage("Cannot be Multiple Active Months"));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }
        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allSessionInfo = monthsRepo.GetAllMonthsInfo();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allSessionInfo != null)
            {
                return new JsonResult()
                {
                    Data = allSessionInfo.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allSessionInfo,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditSessionInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSessionInfo = monthsRepo.EditMonthsInfo(id);
                    return new JsonResult()
                    {
                        Data = editSessionInfo,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
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
                // return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSessionInfo(Months monthInfo)
        {
            try
            {
                if (monthInfo != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (monthInfo.IsActive == "1")
                        {
                            if (monthsRepo.GetAllMonthsInfo().Any(model => model.IsActive == ""))
                            {
                                return Json(messageHandlerRepo.GetMessage("Active Academic Year Already Exist "));
                            }
                            if (monthInfo.ID > 0)
                            {
                                var savechange = monthsRepo.AddUpdateMonthsInfo(monthInfo);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                                //update
                            }
                            else
                            {
                                var savechange = monthsRepo.AddUpdateMonthsInfo(monthInfo);
                                message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;

                            }
                        }
                        else
                        {
                            if (monthInfo.ID > 0)
                            {
                                var savechange = monthsRepo.AddUpdateMonthsInfo(monthInfo);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                                //update
                            }
                            else
                            {
                                var savechange = monthsRepo.AddUpdateMonthsInfo(monthInfo);
                                message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                                //add
                            }
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

        public JsonResult DeleteSessionInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = monthsRepo.DeleteMonthsInfo(id);
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

        [HttpPost]
        public JsonResult IsActiveSessionExist(string Session)
        {
            return Json(!monthsRepo.GetAllMonthsInfo().Any(model => model.IsActive == "1"), JsonRequestBehavior.AllowGet);
        }
    }
}