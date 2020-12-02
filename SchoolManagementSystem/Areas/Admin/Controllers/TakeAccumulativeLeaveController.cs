using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class TakeAccumulativeLeaveController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly ITakeAccumulativeLeaveRepository takeAccumulativeLeaveRepo;
        
       
        private string message = "";
        // GET: Admin/TakeAccumulativeLeave
        public TakeAccumulativeLeaveController(IMessageHandlerRepository messageHandlerRepo, ITakeAccumulativeLeaveRepository takeAccumulativeLeaveRepo, IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.takeAccumulativeLeaveRepo = takeAccumulativeLeaveRepo;
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }
        public PartialViewResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }
        [HttpPost]
        public JsonResult CalculateRemainingLeave(TakeLeave takeleave)
        {
            try
            {
                if (takeleave != null)
                {
                    var leaveType = takeAccumulativeLeaveRepo.CalculateRemainingLeave(takeleave);
                    return new JsonResult()
                    {
                        Data = leaveType,
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
        public void LoadDropDown()
        {
            var allActiveSession = dropDownRepo.GetSessionDropDown();
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, TakeLeave search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var employeeList = takeAccumulativeLeaveRepo.GetAllAccumulativeLeave(search);

            if (employeeList != null)
            {
                var result = new DataSourceResult()
                {
                    Data = employeeList,
                    Total = employeeList.Select(model => model.Total).FirstOrDefault()
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var result = new DataSourceResult()
                {
                    Data = employeeList,
                    Total = 0
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }







        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveTakeAccumulativeLeave(TakeLeave leave)
        {
            try
            {
                if (leave != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (leave.ID > 0)
                        {

                            var savechange = takeAccumulativeLeaveRepo.AddUpdateTakeAccumulativeLeave(leave);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = takeAccumulativeLeaveRepo.AddUpdateTakeAccumulativeLeave(leave);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;

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
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        public JsonResult DeleteTakeaccumulativeleave(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = takeAccumulativeLeaveRepo.DeleteTakeAccumulativeLeave(id);
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