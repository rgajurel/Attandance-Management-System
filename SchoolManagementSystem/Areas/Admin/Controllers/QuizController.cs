using DomainEntities;
using DomainInterface;
using Infrastructure;
using Kendo.Mvc.UI;
//using Microsoft.Security.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http.ModelBinding;
using System.Web.Mvc;
//using TechtonneMS.Helper;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class QuizController : Controller
    {
       // private readonly IStatusRepository _statusRepo;
        private readonly IQuizRepository _QuizRepo;
        private readonly IQuizQuestionRepository _QuizQuestionRepo;
        private readonly IMessageHandlerRepository _messageRepo;
        private readonly IUserGroupRepository _UserGroupRepo;
        // private readonly ISettingsRepository _SettingRepo;
        private readonly IDropDownRepository _dropDownRepo;
       // private readonly IArticle _ArticleRepo;
      //  string notificationIcon = NotificationIcon.quizIcon;
       // private readonly INotificationRepository _NotificationRepo;

        //Notification notification = new Notification();


        public QuizController( IQuizRepository QuizRepo, IQuizQuestionRepository QuizQuestionRepo, IMessageHandlerRepository messageRepo, IDropDownRepository dropDownRepo, IUserGroupRepository UserGroupRepo)
        {
            //this._statusRepo = statusRepo;
            this._QuizRepo = QuizRepo;
            this._QuizQuestionRepo = QuizQuestionRepo;
            this._messageRepo = messageRepo;
            //this._NotificationRepo = NotificationRepo;
           // this._SettingRepo = SettingRepo;
            this._dropDownRepo = dropDownRepo;
            this._UserGroupRepo = UserGroupRepo;
            //this._ArticleRepo = ArticleRepo;
        }
        // GET: Admin/Quiz
       // [AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            //IEnumerable<DomainEntities.Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(new LoginUser().UserName);

            //ViewBag.editAccess = AuthorizeUser.AuthorizeControlForButton("Edit", allowedMenus);
            //ViewBag.createAccess = AuthorizeUser.AuthorizeControlForButton("Add", allowedMenus);
            //ViewBag.deleteAccess = AuthorizeUser.AuthorizeControlForButton("Delete", allowedMenus);
            GetDateFormat();
            GetAllUserGroup();
            //GetAllQuizCategoryWithCount();
            //GetAllQuizStatus();
            //GetAllQuizCategory();
            //GetAllNotification();
            GetAllQuizDifficulty();
            //GetAllQuizCourse();
            //GetAllQuestionCategory();
            GetItemPerPage();
            //GetTags();
            //GetProrityDropDown();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]

        public ActionResult InsertUpdateQuiz(QuizEntity objInfo)
        {
            //NotificationHelper notiHelper = new NotificationHelper();

            if (ModelState.IsValid)
            {
                string MessageStatus;
                int operationStatus;
                objInfo.AddedBy = new LoginUser().UserName;
                QuizEntity quizInfo = _QuizRepo.AddUpdateQuiz(objInfo);
                //int status = 0;
                if (quizInfo.QuizID > -1)
                {
                    //if (objInfo.StatusValue == 1)
                    //{
                    //    notification.NotificationDescription = objInfo.QuizDescription;
                    //    objInfo.QuizSlug = quizInfo.QuizSlug;
                    //    if (objInfo.QuizID > 0)
                    //    {
                    //        //description = "Course Added<a href='/Client/Course/Detail?Slug=" + course.Slug + "'>" + course.CourseName + "</a>";

                    //        title = "Quiz Updated (" + objInfo.QuizTitle + ")";
                    //        description = "Quiz Updated <br/><a href='/Client/QuizDetail?QuizSlug=" + quizInfo.QuizSlug + "' onclick='DisablePopupNotificationAndRedirect(event);'>" + objInfo.QuizTitle + "</a>";
                    //    }
                    //    else
                    //    {
                    //        title = "New Quiz Added (" + objInfo.QuizTitle + ")";
                    //        description = notification.NotificationDescription = "Quiz <br/><a href='/Client/QuizDetail?QuizSlug=" + objInfo.QuizSlug + "' onclick='DisablePopupNotificationAndRedirect(event);'>" + objInfo.QuizTitle + "</a>";
                    //    }

                    //    var type = objInfo.NotificationID.Split(',').ToList();
                    //    objInfo.QuizSlug = quizInfo.QuizSlug;
                    //    if (type.Contains(Convert.ToString(1)))
                    //    {
                    //        // notification.NotificationDescription = description;
                    //        notification.NotificationType = "1";
                    //        CallNotification(title, description, objInfo, notification.NotificationType, Convert.ToString(objInfo.CategoryID));
                    //    }
                    //    if (type.Contains(Convert.ToString(2)))
                    //    {
                    //        notification.PopUpDescription = description;
                    //        notification.NotificationType = "2";
                    //        CallNotification(title, description, objInfo, notification.NotificationType, Convert.ToString(objInfo.CategoryID));

                    //    }

                    //    if (type.Contains(Convert.ToString(3))) //sms notification
                    //    {
                    //        string templateEvent = EmailAndSMSEventIDNotification.OnQuizModifiedSMSNotification;
                    //        if (objInfo.QuizID <= 0)
                    //        {
                    //            templateEvent = EmailAndSMSEventIDNotification.OnQuizCreateSMSNotification;
                    //        }
                    //        var smstemplate = _NotificationRepo.GetEventTemplate(templateEvent);
                    //        var smseventvariable = smstemplate.EventVariable.Split(',').ToList();
                    //        var body = smstemplate.Body;
                    //        var smsmessage = smseventvariable[1];
                    //        var finalsmsdescription = body.Replace(smsmessage, objInfo.QuizTitle);
                    //        notification.SmsDescription = finalsmsdescription;

                    //        int insertedNotificationID = CallNotification(title, notification.SmsDescription, objInfo, "3", Convert.ToString(objInfo.CategoryID));

                    //        if (objInfo.NotifyNow)
                    //        {
                    //            List<UserNotification> usersNotification = _NotificationRepo.GetUserNotificationByNotificationID(insertedNotificationID);

                    //            SMSSenderReceiverData smsSenderData = new SMSSenderReceiverData();
                    //            string smsSettingGeneralGroup = SettingsGroupName.SMSGroup;
                    //            try
                    //            {
                    //                smsSenderData.SMSHost = _SettingRepo.GetSettingByIDandGroup("1006", smsSettingGeneralGroup);
                    //            }
                    //            catch (Exception)
                    //            {
                    //                smsSenderData.SMSHost = "http://localhost";
                    //            }
                    //            try
                    //            {
                    //                smsSenderData.SMSPort = _SettingRepo.GetSettingByIDandGroup("1042", smsSettingGeneralGroup);
                    //            }
                    //            catch (Exception)
                    //            {
                    //                smsSenderData.SMSPort = "8093";
                    //            }
                    //            smsSenderData.BodyMessage = notification.SMSBody;
                    //            smsSenderData.NotificationID = insertedNotificationID;

                    //            await SMSHelper.SendBulkSMSToUser(smsSenderData, usersNotification, _NotificationRepo);
                    //        }

                    //    }
                    //    if (type.Contains(Convert.ToString(4))) //email notification
                    //    {

                    //        string templateEvent = EmailAndSMSEventIDNotification.OnQuizModifiedEmailNotification;
                    //        if (objInfo.QuizID <= 0)
                    //        {
                    //            templateEvent = EmailAndSMSEventIDNotification.OnQuizCreateEmailNotification;
                    //        }
                    //        var emailtemplate = _NotificationRepo.GetEventTemplate(templateEvent);
                    //        var emaileventvariable = emailtemplate.EventVariable.Split(',').ToList();
                    //        var body = emailtemplate.Body;
                    //        var emailMessage = emaileventvariable[1];
                    //        var finalemaildescription = body.Replace(emailMessage, objInfo.QuizTitle);
                    //        notification.EmailDescription = finalemaildescription;
                    //        int insertedNotificationID = CallNotification(title, notification.EmailDescription, objInfo, "4", Convert.ToString(objInfo.CategoryID));
                    //        if (objInfo.NotifyNow)
                    //        {
                    //            List<UserNotification> usersNotification = _NotificationRepo.GetUserNotificationByNotificationID(insertedNotificationID);

                    //            EmailSenderReceiverData emailSenderData = new EmailSenderReceiverData();
                    //            string emailSettingEmailGroup = SettingsGroupName.EmailGroup;
                    //            emailSenderData.NotificationID = insertedNotificationID;
                    //            emailSenderData.SMTPHost = _SettingRepo.GetSettingByIDandGroup("1003", emailSettingEmailGroup);
                    //            emailSenderData.SMTPUserName = _SettingRepo.GetSettingByIDandGroup("1004", emailSettingEmailGroup);
                    //            emailSenderData.SMTPPassword = _SettingRepo.GetSettingByIDandGroup("1005", emailSettingEmailGroup);
                    //            try
                    //            {
                    //                emailSenderData.SMTPPort = Convert.ToInt32(_SettingRepo.GetSettingByIDandGroup("1051", emailSettingEmailGroup));
                    //            }
                    //            catch (Exception)
                    //            {
                    //                emailSenderData.SMTPPort = 587;
                    //            }
                    //            await EmailHelper.SendBulkEmail(emailSenderData, usersNotification, _NotificationRepo);
                    //        }
                    //    }
                    //}
                    if (objInfo.QuizID > 0)
                    {

                        // notiHelper.AddedNotification(objInfo.QuizTitle, "New Quiz ("+objInfo.QuizTitle+") has been added for you.", objInfo.StartDate, objInfo.EndDate, objInfo.NotificationID.ToString(), objInfo.StatusValue, Convert.ToString(objInfo.CategoryID), notificationIcon);

                        MessageStatus = StatusCodeDescription.QuizUpdateSuccess;
                    }
                    else
                    {
                        //  notiHelper.AddedNotification(objInfo.QuizTitle, "New Quiz (" + objInfo.QuizTitle + ") has been added for you.", objInfo.StartDate, objInfo.EndDate, objInfo.NotificationID.ToString(), objInfo.StatusValue, Convert.ToString(objInfo.CategoryID), notificationIcon);
                        MessageStatus = StatusCodeDescription.QuizAddSuccess;
                    }
                    operationStatus = StatusCodeDescription.success;
                }
                else
                {
                    MessageStatus = StatusCodeDescription.QuizErrorMessage;
                    operationStatus = StatusCodeDescription.failure;
                }

                return Json(new { operationStatus, MessageStatus });

            }
            return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });

        }
        //public int CallNotification(string title, string description, QuizEntity quiz, string notificationtype, string categoryID)
        //{
        //    notification.NotificationTitle = title;
        //    notification.NotificationDescription = description;
        //    notification.NotificationType = notificationtype;
        //    notification.GroupID = null;
        //    notification.NotificationTriggredDate = quiz.StartDate;
        //    notification.NotificationExpiryDate = quiz.EndDate;
        //    notification.NotificationStatusID = quiz.StatusValue;
        //    notification.CategoryID = categoryID;
        //    notification.Image = null;
        //    notification.Video = null;
        //    notification.IsShown = false;
        //    notification.TriggerNow = false;
        //    notification.NotificationExternalLink = "/Client/QuizDetail?QuizSlug=" + quiz.QuizSlug;
        //    notification.NotificationIcon = NotificationIcon.newCourse;
        //    int insertedNotifcaitionID = _NotificationRepo.AddNotification(notification);
        //  //  int insertedNotifcaitionID = inotiRepo.AddNotification(notification);
        //    return insertedNotifcaitionID;

        //}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetQuizByID(int QuizID)
        {
            if (ModelState.IsValid)
            {
                QuizEntity objInfo = _QuizRepo.GetQuizByID(QuizID);
                return Json(new { StatusCodeDescription.failure, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });
            }

        }
        [ValidateAntiForgeryToken]
        //[AuthorizeUser(Controls = "Delete")]
        [HttpPost]
        public ActionResult DeleteQuizByID(int QuizID)
        {
            if (ModelState.IsValid)
            {
                int status = _QuizRepo.DeleteQuizByID(QuizID);
                int ReturnCode;
                string Message;
                if (status == 1)
                {
                    Message = StatusCodeDescription.QuizDeleteSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else if (status == 0)
                {
                    Message = StatusCodeDescription.QuizDependencyDeleteMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                else
                {
                    Message = StatusCodeDescription.QuizDeleteFailure;
                    ReturnCode = StatusCodeDescription.failure;
                }
                return Json(new { ReturnCode, Message });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizDeleteFailure });
            }

        }
        //private void GetAllQuizStatus()
        //{
        //    ViewBag.QuizStatus = new SelectList(_statusRepo.GetStatusBasedOnIdentifier(StatusIdentifier.identifierQuiz), "StatusValue", "StatusName");
        //}
        //private void GetAllQuizCategory()
        //{
        //    IEnumerable<CategoryTree> LstCategory = _QuizRepo.GetAllQuizCategory(CategoryType.CategoryQuiz, StatusIdentifier.identifierQuiz);
        //    ViewBag.QuizCategory = new SelectList(LstCategory, "CategoryID", "CategoryName");

        //}
        //private void GetAllQuizCategoryWithCount()
        //{
        //    IEnumerable<CategoryTree> LstCategory = _QuizRepo.GetAllQuizCategoryWithCount(CategoryType.CategoryQuizQuestion, StatusIdentifier.identifierQuizQuestion);
        //    ViewBag.QuizQuestionCategoryWithCount = new SelectList(LstCategory, "CategoryID", "CategoryName");

        //}
        //private void GetAllNotification()
        //{
        //    //var LstNotification = _NotificationRepo.GetAllNotificationType().Select(q => new { NotificationID = q.NotificationTypeID, NotificationTitle=q.NotificationTypeName }).ToList();
        //    ViewBag.Notification = new SelectList(_NotificationRepo.GetAllNotificationType().Select(q => new { NotificationID = q.NotificationTypeID, NotificationTitle = q.NotificationTypeName }), "NotificationID", "NotificationTitle");
        //}
        private void GetAllQuizDifficulty()
        {
            IEnumerable<QuizQuestionDifficultyEntity> LstQuizDifficulty = _QuizQuestionRepo.GetAllQuizQuestionDifficulty();
            ViewBag.QuizQestionDifficulty = new SelectList(LstQuizDifficulty, "DifficultyLevelID", "DifficultyLevel");

        }
        //private void GetAllQuestionCategory()
        //{
        //    IEnumerable<CategoryTree> LstCategory = _QuizRepo.GetAllQuizCategory(CategoryType.CategoryQuizQuestion, StatusIdentifier.identifierQuizQuestion);
        //    ViewBag.QuizQuestionCategory = new SelectList(LstCategory, "CategoryID", "CategoryName");

        //}
        private void GetDateFormat()
        {
            //string generalSettingGroup = SettingsGroupName.GeneralGroup;
            //settingDate = _SettingRepo.GetSettingByIDandGroup("1023", generalSettingGroup);
            //if (String.IsNullOrEmpty(settingDate))
            //{
            //    settingDate = "MM-dd-yyyy";
            //}
            ViewBag.settingDateFormat = "{0:" + "MM-dd-yyyy" + "}";
        }
        //private void GetAllQuizCourse()
        //{
        //    ViewBag.QuizCourse = new SelectList(_QuizRepo.GetAllCourseForQuiz(), "CourseID", "FullCourseName");
        //}
        private void GetItemPerPage()
        {
            //string generalSettingGroup = SettingsGroupName.GeneralGroup;
            //string itemPerPageSettingValue = _SettingRepo.GetSettingByIDandGroup("1001", generalSettingGroup);
            //int itemPerPage;
            //try
            //{
            //    itemPerPage = Convert.ToInt16(itemPerPageSettingValue);
            //}
            //catch
            //{
            //    itemPerPage = 10;
            //}

            ViewBag.ItemPerPage = 10;
        }
        private void GetAllUserGroup()
        {
            ViewBag.GetAllUserGroup = new SelectList(_dropDownRepo.GetUserGroup(), "ID", "GroupName"); 
        }
        //private void GetTags()
        //{
        //    ViewData["TagList"] = _dropDownRepo.GetAllTag(TagIdentifiler.Quiz).Select(d => new SelectListItem
        //    {
        //        Text = d.Name,
        //        Value = d.ID.ToString()
        //    });
        //}
        //private void GetProrityDropDown()
        //{
        //    string generalSettingGroup = SettingsGroupName.GeneralGroup;
        //    int settingSearchPriority = 5;
        //    try
        //    {
        //        settingSearchPriority = Convert.ToInt16(_SettingRepo.GetSettingByIDandGroup("1050", generalSettingGroup));
        //    }
        //    catch (Exception)
        //    {
        //        settingSearchPriority = 5;
        //    }

        //    List<SelectListItem> priorityItems = new List<SelectListItem>();
        //    for (int i = 0; i <= settingSearchPriority; i++)
        //    {
        //        priorityItems.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
        //    }
        //    ViewBag.priorityList = new SelectList(priorityItems, "Value", "Text");
        //}
        public JsonResult GetAllQuizQuestion([DataSourceRequest] DataSourceRequest request, QuizSearchQuestionEntity objInfo)
        {

            //QuizSearchQuestionEntity objInfo = JsonConvert.DeserializeObject<QuizSearchQuestionEntity>(objInfo1);
            objInfo.Page = request.Page;
            objInfo.PageSize = request.PageSize;
            if (objInfo.SearchQuizQuestion == null)
            {
                objInfo.SearchQuizQuestion = "";
            }
            if (objInfo.SearchCategoryID == 0)
            {
                objInfo.SearchCategoryID = -1;
            }
            if (objInfo.SearchDifficultyLevelID == 0)
            {
                objInfo.SearchDifficultyLevelID = -1;
            }
            if (objInfo.SearchWeightageID == 0)
            {
                objInfo.SearchWeightageID = -1;
            }
            if (objInfo.SearchQuestionTypeID == 0)
            {
                objInfo.SearchQuestionTypeID = -1;
            }
            IEnumerable<QuizQuestionEntity> ListObj = _QuizQuestionRepo.GetAllQuizQuestionForQuiz(objInfo);
            int Total;
            try
            {
                Total = ListObj.FirstOrDefault().RowTotal;
            }
            catch (Exception)
            {
                Total = 0;
            }
            //  return _MessageRepo.GetSuccessMessageWithList(true, StatusCodeDescription.successMessage, ListObj, Total);
            var result = new DataSourceResult()
            {
                Data = ListObj,
                Total = Total
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllQuiz([DataSourceRequest] DataSourceRequest request, string objInfo)
        {

            SearchQuizParam obj = JsonConvert.DeserializeObject<SearchQuizParam>(objInfo);

            obj.PageIndex = request.Page;
            obj.PageSize = request.PageSize;
            IEnumerable<QuizEntity> QuizLst = _QuizRepo.GetAllQuizListing(obj);
            int total;
            try
            {
                total = QuizLst.FirstOrDefault().RowTotal;
            }
            catch (Exception)
            {
                total = 0;
            }
            var result = new DataSourceResult()
            {
                Data = QuizLst,
                Total = total
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        //[HttpGet]
        //public JsonResult LoadTag(int Identifier)
        //{
        //    var list = _dropDownRepo.GetAllTag(TagIdentifiler.Quiz);
        //    return Json(list, JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public JsonResult SaveTag(string[] TagName)
        //{
        //    var username = new LoginUser().UserName;
        //    string message;
        //    var tags = new List<Tag>();
        //    for (int i = 0; i < TagName.Length; i++)
        //    {
        //        var tag = _ArticleRepo.SaveDeleteTag(TagIdentifiler.Quiz, TagName[i], username, true);
        //        if (tag != null)
        //        {
        //            tags.Add(tag);
        //        }
        //    }

        //    //if (tags.Count() > 0)
        //    //{
        //        message = StatusCodeDescription.QuizTagSaveMessage;
        //        return Json(_messageRepo.GetSuccessMessageWithData(true, message, tags));

        //    //}
        //    //else
        //    //{
        //    //    message = StatusCodeDescription.QuizTagSaveFaliureMessage;
        //    //    return Json(_messageRepo.GetErrorMessageWithData(true, message, null));

        //    //}

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BatchUpdateStatusForQuiz(string JsonObject)
        {
            return Json(_QuizRepo.BatchUpdateQuiz(JsonObject));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStatusForBatchQuizUpdate(string JsonObject)
        {
            return Json(_QuizRepo.GetBatchUploadStatus(JsonObject));
        }
    }
}