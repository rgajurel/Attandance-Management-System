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
    public class OfficialLeaveController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IOfficialLeaveRepository officialLeaveRepo;
        private readonly ITakeLeaveRepository takeLeaveRepo;
        private readonly INotificationRepository notificaitionRepo;
        private string message = "";
        public OfficialLeaveController(INotificationRepository notificaitionRepo, IMessageHandlerRepository messageHandlerRepo,ITakeLeaveRepository takeLeaveRepo, IOfficialLeaveRepository officialLeaveRepo  , IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.officialLeaveRepo = officialLeaveRepo;
            this.takeLeaveRepo = takeLeaveRepo;
            this.notificaitionRepo = notificaitionRepo;
        }
        // GET: Admin/OfficialLeave
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
        [ValidateAntiForgeryToken]
        public JsonResult SaveOfficialLeave(Attandance attandance)
        {
            try
            {
                if (attandance != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (attandance.ID > 0)
                        {

                            var savechange = officialLeaveRepo.AddUpdateOfficialLeave(attandance);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            if (attandance.DateFrom.Date.Month != attandance.DateTo.Date.Month)
                            {
                                message = "Cannot Select Date of Two different Months";
                            }
                            else
                            {
                                var savechange = officialLeaveRepo.AddUpdateOfficialLeave(attandance);
                                message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
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

        public void LoadDropDown()
        {
           
            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionListAll = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionListAll = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
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

        [HttpPost]
        public JsonResult GetLeaveTypeBasedOnOrganisation(string ID)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    var leaveType = officialLeaveRepo.GetLeaveTypeBasedOnOrganisation(Convert.ToString(ID));
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

        [HttpPost]
        public JsonResult GetEmployeeBaesdOrganisation(string OrganisationID)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(OrganisationID))
                {
                    var employeeList = officialLeaveRepo.GetEmployeeBasedOnOrganisation(OrganisationID);
                    return new JsonResult()
                    {
                        Data = employeeList,
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, Attandance search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var employeeList = officialLeaveRepo.GetAllOfficialLeave(search);

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
        public JsonResult EditOfficialLeave(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editOfficialLeave = officialLeaveRepo.EditOfficialLeave(id);
                    return new JsonResult()
                    {
                        Data = editOfficialLeave,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
                }


            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
                // return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }

        public JsonResult DeleteOfficialLeave(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = officialLeaveRepo.DeleteOfficialLeave
                        (id);
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

        [HttpPost]

        public JsonResult ApproveLeave(string status, string notificationtype, string id, string employeeid, string organisationid)
        {
            try
            {
                if (!String.IsNullOrEmpty(status) && !String.IsNullOrEmpty(notificationtype) && !String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(employeeid) && !String.IsNullOrEmpty(organisationid))
                {

                    var savechange = officialLeaveRepo.ApproveLeave(status, id);
                    if (savechange)
                    {
                        var savenotification = SendNotification(notificationtype, employeeid, organisationid, status);
                        message = (savechange == true) ? MassageDescription.ApproveSuccess : MassageDescription.ApproveFailure;
                    }
                    else
                    {
                        message = MassageDescription.SaveFailure;
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

        private bool SendNotification(string notificationtype, string employeeid, string organisationid, string status)
        {
            Notification notification = new Notification();

            if (status == "0" && notificationtype!="5")
            {
                notification.Title = "<strong style='color:brown'>Your Travel ReQuest Has Been Approved</strong>";

                notification.Description = "<strong style='color:brown'>Your Travel Request Has Been Approved by" + new LoginUser().UserName + "</strong>";
            }
            if (status == "0" && notificationtype == "5")
            {
                notification.Title = "Your Travel ReQuest Has Been Approved";

                notification.Description = "Your Travel Request Has Been Approved by" + new LoginUser().UserName;
            }
            if (status == "1" && notificationtype == "5")
            {
                notification.Title = "Your Leave Request Has Been Rejected";
                notification.Description = "Your Leave Request Has Been Rejectedby" + new LoginUser().UserName;
            }

            if (status == "1" && notificationtype != "5")
            {
                notification.Title = "<strong style='color:red'>Your Leave Request Has Been Rejected</strong>";
                notification.Description = "<strong style='color:red'>Your Leave Request Has Been Rejectedby" + new LoginUser().UserName;
            }

            notification.NotificationType = notificationtype;
            notification.TriggerNow = true;
            notification.GroupID = null;
            notification.Link = "/Client/TakeLeave";
            notification.OrganisationID = Convert.ToInt16(organisationid);
            notification.TriggerDate = DateTime.Now;
            notification.ExpiryDate = DateTime.Now.AddDays(2);
            notification.IsInternal = true;
            notification.EmployeeID = employeeid;
          var saveChanges=  takeLeaveRepo.AddUpdateNotificationTakeLeave(notification, employeeid);

            if (notificationtype == "5")
            {
                notificaitionRepo.PushNotificationToUser(notification);
            }
            return saveChanges;
            

        }
    }
}