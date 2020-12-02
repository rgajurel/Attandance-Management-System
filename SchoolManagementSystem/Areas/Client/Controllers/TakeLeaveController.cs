using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class TakeLeaveController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly ITakeLeaveRepository takeLeaveRepo;
        private readonly INotificationRepository notificationRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        public TakeLeaveController(INotificationRepository notificationRepo,IDropDownRepository dropDownRepo, ITakeLeaveRepository takeLeaveRepo,IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.takeLeaveRepo = takeLeaveRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.notificationRepo = notificationRepo;
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

            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionListAll = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionListAll = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allMonth = dropDownRepo.GetMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var notificationTypes = dropDownRepo.GetNotificationTypes();
            if (notificationTypes != null)
            {
                ViewBag.allNotificationTypes = new SelectList(notificationTypes, "NoficationTypeID", "NotificationType");
            }
            else
            {
                ViewBag.allNotificationTypes = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveTakeLeave(ClientTakeLeave leave)
        {
            try
            {
                if (leave != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (leave.ID > 0)
                        {
                            var savechange = takeLeaveRepo.AddUpdateTakeLeave(leave);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = takeLeaveRepo.AddUpdateTakeLeave(leave);
                            if (savechange)
                            {
                                var savenotification = SendNotification(Convert.ToString(leave.NotificationType),Convert.ToString(leave.ApprovedBy), Convert.ToString(leave.OrganisationID), "2");
                                message = (savechange == true) ? MassageDescription.ApproveSuccess : MassageDescription.ApproveFailure;
                            }
                            else
                            {
                                message = MassageDescription.SaveFailure;
                            }
                            
                            
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
        private bool SendNotification(string notificationtype, string employeeid, string organisationid, string status)
        {
            Notification notification = new Notification();
            
            if (status == "2")
            {
                notification.Title = "<strong style='color:brown'>You Have Pending Leave Request To Be Approved of "+new LoginUser().UserName+"</strong>";

                notification.Description = "<strong style='color:brown'>You Have Pending Leave Request To Be Approved of "+new LoginUser().UserName+"</strong>";
            }           

            notification.NotificationType = notificationtype;
            notification.TriggerNow = true;
            notification.GroupID = null;
            notification.Link = "/Admin/TakeLeave";
            notification.OrganisationID = Convert.ToInt16(organisationid);
            notification.TriggerDate = DateTime.Now;
            notification.ExpiryDate = DateTime.Now.AddDays(2);
            notification.IsInternal = true;
            notification.EmployeeID = employeeid;
            var notificationSave= takeLeaveRepo.AddUpdateNotificationTakeLeave(notification, employeeid);
            if (notificationtype == "5")
            {
                notificationRepo.PushNotificationToUser(notification);
            }
            return notificationSave;
          

        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, ClientTakeLeave search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var employeeList = takeLeaveRepo.GetAllTakeLeave(search);

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

    }
}