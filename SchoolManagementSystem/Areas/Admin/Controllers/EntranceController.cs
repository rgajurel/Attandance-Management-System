using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class EntranceController : Controller
    {
        private readonly IEntranceRepository _EntranceRepo;
        private readonly IEntranceQuestionRepository _EntranceQuestionRepo;
        private readonly IMessageHandlerRepository _messageRepo;
        private readonly IUserGroupRepository _UserGroupRepo;
        // private readonly ISettingsRepository _SettingRepo;
        private readonly IDropDownRepository _dropDownRepo;
        // GET: Admin/Entrance


        public EntranceController(IEntranceRepository EntranceRepo, IEntranceQuestionRepository EntranceQuestionRepo, IMessageHandlerRepository messageRepo, IDropDownRepository dropDownRepo, IUserGroupRepository UserGroupRepo)
        {
            //this._statusRepo = statusRepo;
            this._EntranceRepo = EntranceRepo;
            this._EntranceQuestionRepo = EntranceQuestionRepo;
            this._messageRepo = messageRepo;
            //this._NotificationRepo = NotificationRepo;
            // this._SettingRepo = SettingRepo;
            this._dropDownRepo = dropDownRepo;
            this._UserGroupRepo = UserGroupRepo;
            //this._ArticleRepo = ArticleRepo;
        }
        public ActionResult Index()
        {
            GetDateFormat();
            GetAllUserGroup();
            GetAllQuestionCategory();
            GetAllEntranceDifficulty();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public ActionResult InsertUpdateEntrance(EntranceEntity objInfo)
        {
            //NotificationHelper notiHelper = new NotificationHelper();

            if (ModelState.IsValid)
            {
                string MessageStatus;
                int operationStatus;
                objInfo.AddedBy = new LoginUser().UserName;
                EntranceEntity entranceInfo = _EntranceRepo.AddUpdateEntrance(objInfo);
                //int status = 0;
                if (entranceInfo.EntranceID > -1)
                {
                    if (entranceInfo.EntranceID > 0)
                    {

                        // notiHelper.AddedNotification(objInfo.QuizTitle, "New Quiz ("+objInfo.QuizTitle+") has been added for you.", objInfo.StartDate, objInfo.EndDate, objInfo.NotificationID.ToString(), objInfo.StatusValue, Convert.ToString(objInfo.CategoryID), notificationIcon);

                        MessageStatus = StatusCodeDescription.EntranceUpdateSuccess;
                    }
                    else
                    {
                        //  notiHelper.AddedNotification(objInfo.QuizTitle, "New Quiz (" + objInfo.QuizTitle + ") has been added for you.", objInfo.StartDate, objInfo.EndDate, objInfo.NotificationID.ToString(), objInfo.StatusValue, Convert.ToString(objInfo.CategoryID), notificationIcon);
                        MessageStatus = StatusCodeDescription.EntranceUpdateSuccess;
                    }
                    operationStatus = StatusCodeDescription.success;
                }
                else
                {
                    MessageStatus = StatusCodeDescription.EntranceErrorMessage;
                    operationStatus = StatusCodeDescription.failure;
                }

                return Json(new { operationStatus, MessageStatus });

            }
            return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetEntranceByID(int EntranceID)
        {
            if (ModelState.IsValid)
            {
                EntranceEntity objInfo = _EntranceRepo.GetEntranceByID(EntranceID);
                return Json(new { StatusCodeDescription.failure, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.EntranceErrorMessage });
            }

        }

        [ValidateAntiForgeryToken]
        //[AuthorizeUser(Controls = "Delete")]
        [HttpPost]
        public ActionResult DeleteEntranceByID(int EntranceID)
        {
            if (ModelState.IsValid)
            {
                int status = _EntranceRepo.DeleteEntranceByID(EntranceID);
                int ReturnCode;
                string Message;
                if (status == 1)
                {
                    Message = StatusCodeDescription.EntranceDeleteSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else if (status == 0)
                {
                    Message = StatusCodeDescription.EntranceDependencyDeleteMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                else
                {
                    Message = StatusCodeDescription.EntranceDeleteFailure;
                    ReturnCode = StatusCodeDescription.failure;
                }
                return Json(new { ReturnCode, Message });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.EntranceDeleteFailure });
            }

        }
        private void GetAllEntranceDifficulty()
        {
            IEnumerable<EntranceQuestionDifficultyEntity> LstEntranceDifficulty = _EntranceQuestionRepo.GetAllEntranceQuestionDifficulty();
            ViewBag.EntranceQestionDifficulty = new SelectList(LstEntranceDifficulty, "DifficultyLevelID", "DifficultyLevel");

        }
        private void GetDateFormat()
        {
            ViewBag.settingDateFormat = "{0:" + "MM-dd-yyyy" + "}";
            ViewBag.ItemPerPage = 10;
        }
        private void GetAllUserGroup()
        {
            ViewBag.GetAllUserGroup = new SelectList(_dropDownRepo.GetUserGroup(), "ID", "GroupName");
        }

        public JsonResult GetAllEntranceQuestion([DataSourceRequest] DataSourceRequest request, EntranceSearchQuestionEntity objInfo)
        {
            objInfo.Page = request.Page;
            objInfo.PageSize = request.PageSize;
            if (objInfo.SearchEntranceQuestion == null)
            {
                objInfo.SearchEntranceQuestion = "";
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
            IEnumerable<EntranceQuestionEntity> ListObj = _EntranceQuestionRepo.GetAllEntranceQuestionForEntrance(objInfo);
            int Total;
            try
            {
                Total = ListObj.FirstOrDefault().RowTotal;
            }
            catch (Exception)
            {
                Total = 0;
            }
            var result = new DataSourceResult()
            {
                Data = ListObj,
                Total = Total
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllEntrance([DataSourceRequest] DataSourceRequest request, string objInfo)
        {

            SearchEntranceParam obj = JsonConvert.DeserializeObject<SearchEntranceParam>(objInfo);

            obj.PageIndex = request.Page;
            obj.PageSize = request.PageSize;
            IEnumerable<EntranceEntity> EntranceLst = _EntranceRepo.GetAllEntranceListing(obj);
            int total;
            try
            {
                total = EntranceLst.FirstOrDefault().RowTotal;
            }
            catch (Exception)
            {
                total = 0;
            }
            var result = new DataSourceResult()
            {
                Data = EntranceLst,
                Total = total
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BatchUpdateStatusForEntrance(string JsonObject)
        {
            return Json(_EntranceRepo.BatchUpdateEntrance(JsonObject));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStatusForBatchEntranceUpdate(string JsonObject)
        {
            return Json(_EntranceRepo.GetBatchUploadStatus(JsonObject));
        }
        private void GetAllQuestionCategory()
        {
            IEnumerable<CategoryTree> LstCategory = _EntranceQuestionRepo.GetAllEntraceQuestionCategory(CategoryType.CategoryEntranceQuestion);
            ViewBag.EntraceQuestionCategory = new SelectList(LstCategory, "CategoryID", "CategoryName");
        }
    }
}