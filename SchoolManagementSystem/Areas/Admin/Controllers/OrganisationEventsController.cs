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
    public class OrganisationEventsController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;       
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IOrganisationEventsRepository organisationEventsRepo;
        private readonly INotificationRepository notificationRepo;
        public string message = "";
        // GET: Admin/OrganisationEvents

       public OrganisationEventsController(IDropDownRepository dropDownRepo, INotificationRepository notificationRepo, IOrganisationEventsRepository organisationEventsRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.organisationEventsRepo = organisationEventsRepo;
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
        public JsonResult SaveOrganisationEvents(OrganisationEvents organisationEvents)
        {
            try
            {

                ModelState["GroupID"].Errors.Clear();
                organisationEvents.GroupID = string.Join(",", organisationEvents.GroupArray.ToArray());
                
                if (organisationEvents != null)
                {

                    if (ModelState.IsValid)
                    {
                        if (organisationEvents.ID > 0)
                        {
                            var savenotification = SendNotification(organisationEvents);
                            if (savenotification)
                            {
                                var savechange = organisationEventsRepo.AddUpdateOrganisationevents(organisationEvents);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            }
                            else
                            {
                                message = MassageDescription.SaveFailure;

                            }

                            //update
                        }
                        else
                        {
                            var savenotification=SendNotification(organisationEvents);
                            if (savenotification)
                            {
                                var savechange = organisationEventsRepo.AddUpdateOrganisationevents(organisationEvents);
                                message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            }
                            else
                            {
                                message =MassageDescription.SaveFailure;

                            }
                           
                           
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
        public JsonResult EditOrganisationEvents(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editOrganisationEvents = organisationEventsRepo.EditOrganisationEvents(id);
                    return new JsonResult()
                    {
                        Data = editOrganisationEvents,
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
        public JsonResult DeleteOrganisationEvents(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges =organisationEventsRepo.DeleteOrganisationEvents(id);
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
        private bool SendNotification(OrganisationEvents organisationEvents )
        {
            Notification notification = new Notification();
            if (organisationEvents.ID >=0)
            {
                notification.Title = "New Event<strong style='color:brown'> " + organisationEvents.EventName +"</strong> Information Is Updated";
            }
            else
            {
                notification.Title = "New <strong style='color:brown'>" + organisationEvents.EventName+ "</strong> Event is Going To Happen";
            }
          
            notification.Description = organisationEvents.EventDescription;
            notification.NotificationType = organisationEvents.NotificationType;
            notification.TriggerNow = true;
            notification.GroupID = organisationEvents.GroupID;
            notification.Link = "";
            notification.OrganisationID = organisationEvents.OrganisationID;
            notification.TriggerDate = DateTime.Now;
            notification.ExpiryDate = DateTime.Now.AddDays(2);
            notification.IsInternal = true;
            return notificationRepo.AddUpdateNotification(notification);           

        }
        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, OrganisationEvents organisationEventsSearch)
        {
            try
            {
                organisationEventsSearch.offset = request.PageSize * (request.Page - 1);
                var allOrganisationEvents = organisationEventsRepo.GetAllOrganisationEvents(organisationEventsSearch);// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allOrganisationEvents != null)
                {
                    var result = new DataSourceResult()
                    {
                        Data = allOrganisationEvents,
                        Total = allOrganisationEvents.Select(model => model.Total).FirstOrDefault()
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new DataSourceResult()
                    {
                        Data = allOrganisationEvents,
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
    }
}