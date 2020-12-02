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
    public class NotificationController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly INotificationRepository notificationRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        public string message = "";
           
       
        // GET: Admin/Notification

        public NotificationController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, INotificationRepository notificationRepo)
        {
            this.notificationRepo = notificationRepo;
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

        [HttpPost]
        public JsonResult GetGroupBasedOnOrganisation(string ID)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    var userGroup = notificationRepo.GetUserGroupBasedOnOrganisation(Convert.ToString(ID));
                    return new JsonResult()
                    {
                        Data = userGroup,
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
            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
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
        public JsonResult SaveNotification(Notification notification)
        {
            try
            {
                ModelState["GroupID"].Errors.Clear();
                notification.IsInternal = false;
                notification.GroupID = string.Join(",", notification.GroupArray.ToArray());
                if (notification.TriggerNow == true)
                {
                    notification.ExpiryDate = DateTime.Now.AddDays(2);
                }
                if (notification != null)
                {
                   
                    if (ModelState.IsValid)
                    {
                        if (notification.ID > 0)
                        {                           
                                var savechange = notificationRepo.AddUpdateNotification(notification);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;                         
                           
                            //update
                        }
                        else
                        {
                            var savechange = notificationRepo.AddUpdateNotification(notification);
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request,Notification notificationSearch)
        {
            try
            {
                notificationSearch.offset = request.PageSize * (request.Page - 1);
                var allNotification = notificationRepo.GetAllNotification(notificationSearch);// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allNotification != null)
                {
                    var result = new DataSourceResult()
                    {
                        Data = allNotification,
                        Total = allNotification.Select(model => model.Total).FirstOrDefault()
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new DataSourceResult()
                    {
                        Data = allNotification,
                        Total = 0
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }



        }

        [HttpPost]
        public JsonResult DeleteNotification(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = notificationRepo.DeleteNotification(id);
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
        public JsonResult EditNotification(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editNotification = notificationRepo.EditNotification(id);
                    return new JsonResult()
                    {
                        Data = editNotification,
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
        public JsonResult DisableNotification(string userNotificationID)
        {
            notificationRepo.DisableNotification(userNotificationID);

            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}