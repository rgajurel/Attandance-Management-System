using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class StudentsAssignmentsController : Controller
    {
        private readonly IStudentAssignmentsRepository studentsassignmentsRepository;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly ISubjectsRepository subjectRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly INotificationRepository notificationRepository;
        private string message;
        // GET: Admin/StudentsAssignments

        public StudentsAssignmentsController(IMessageHandlerRepository messageHandlerRepo, ISubjectsRepository subjectRepo, INotificationRepository notificationRepository, IDropDownRepository dropDownRepo, IStudentAssignmentsRepository studentsassignmentsRepository)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.studentsassignmentsRepository = studentsassignmentsRepository;
            this.notificationRepository = notificationRepository;
            this.subjectRepo = subjectRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public void LoadDropDown()
        {
            var allUserGroup = dropDownRepo.GetUserGroup();
            if (allUserGroup != null)
            {
                ViewBag.allUserGroup = new SelectList(allUserGroup, "ID", "GroupName");
            }
            else
            {
                ViewBag.allUserGroup = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
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

            var sessionList = dropDownRepo.GetActiveSessionDropDown();
            if (sessionList != null)
            {
                ViewBag.sessionList = new SelectList(sessionList, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var classList = dropDownRepo.GetClasswDropDown();
            if (classList != null)
            {
                ViewBag.classList = new SelectList(classList, "ID", "Name");
            }
            else
            {
                ViewBag.classList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


            var termList = dropDownRepo.GetTermDropDown();
            if (termList != null)
            {
                ViewBag.termList = new SelectList(termList, "ID", "Name");
            }
            else
            {
                ViewBag.termList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveStudentsAssignments(StudentsAssignments studentAssignments)
        {
            try
            {
                if (studentAssignments != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (studentAssignments.imageFile.ContentLength != 0)
                        {
                            if (studentAssignments.ID > 0)
                            {
                                studentAssignments.Image = UploadImageSave(studentAssignments.imageFile);
                                var savechange = studentsassignmentsRepository.AddUpdateStudentsAssignments(studentAssignments);
                                message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                                //update
                            }
                            else
                            {                                
                                studentAssignments.Image = UploadImageSave(studentAssignments.imageFile);
                                var addnotification = AddNotification(studentAssignments);
                                if (addnotification)
                                {
                                    var savechange = studentsassignmentsRepository.AddUpdateStudentsAssignments(studentAssignments);
                                    message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                                }
                                else
                                {
                                    message = MassageDescription.SaveFailure;
                                }

                               
                                
                            }
                        }
                        else
                        {
                            message = MassageDescription.SelectFile;
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
        public JsonResult EditStudentsAssignments(int id)
        {
            try
            {
                if (id != 0)
                {
                    var studentAssignments = studentsassignmentsRepository.EditStudentAssignments(id);
                    var tt = studentAssignments.Image.Split('/')[3];
                    var file=HttpContext.Server.MapPath("~/Content/StudentsAssignments/" + studentAssignments.Image.Split('/')[3]);
                    if ((System.IO.File.Exists(file)))
                    {
                        System.IO.File.Delete(file);
                    }
                    return new JsonResult()
                    {
                        Data = studentAssignments,
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
                throw ex;
                //throw ex;
                // return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }


        [HttpPost]
        public JsonResult DeleteStudentsAssignments(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = studentsassignmentsRepository.DeleteStudentsAssignments(id);
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, StudentAssignmentsDetails search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var assignmentsList = studentsassignmentsRepository.GetAllStudentsAssignments(search);

            if (assignmentsList != null)
            {
                var result = new DataSourceResult()
                {
                    Data = assignmentsList,
                    Total = assignmentsList.Select(model => model.Total).FirstOrDefault()
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var result = new DataSourceResult()
                {
                    Data = assignmentsList,
                    Total = 0
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }







        }
        public bool AddNotification(StudentsAssignments studentAssignments)
        {
            Notification notification = new Notification();
            var subject=subjectRepo.GetAllSubjects().Where(model => model.ID == studentAssignments.SubjectID).Select(model => model.SubjectName).ToList();
            notification.Title = "New Assignments of<strong style='color:brown'> "+subject[0]+" </strong> has been Assigned to you";
            notification.Link = "";
            notification.Description = "";
            notification.GroupID = Convert.ToString(studentAssignments.GroupID);
            notification.NotificationType = studentAssignments.NotificationType;
            notification.TriggerNow = true;
            notification.ExpiryDate = DateTime.Now.AddDays(2);
            notification.TriggerDate = DateTime.Now;
            notification.IsInternal = true;
           var savechanges= notificationRepository.AddUpdateNotification(notification);
            return savechanges;

        }
        private string UploadImageSave(HttpPostedFileBase file)
        {
            var SchoolPicImage = "";

            if (file != null)
            {
                string schoolPicExtension = Path.GetExtension(file.FileName);
                //var ImageName = Guid.NewGuid();
                file.SaveAs(HttpContext.Server.MapPath("~/Content/StudentsAssignments/" + file.FileName));
                SchoolPicImage = "/Content/StudentsAssignments/" + file.FileName;
            }
            return SchoolPicImage;
           
        }
    }
}