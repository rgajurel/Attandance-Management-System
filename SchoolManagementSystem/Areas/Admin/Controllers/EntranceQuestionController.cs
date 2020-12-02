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
    public class EntranceQuestionController : Controller
    {
        // GET: Admin/QuizQuestion
        private readonly IEntranceQuestionRepository _EntranceQuestionRepo;
        private readonly IMessageHandlerRepository _MessageRepo;
        //private readonly ISettingsRepository _SettingRepo;
        //private readonly IStatusRepository _StatusRepo;
        public EntranceQuestionController(IEntranceQuestionRepository EntranceQuestionRepo, IMessageHandlerRepository MessageRepo)
        {
            this._EntranceQuestionRepo = EntranceQuestionRepo;
            this._MessageRepo = MessageRepo;
        }
        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            //IEnumerable<DomainEntities.Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(new LoginUser().UserName);

            //ViewBag.editAccess = AuthorizeUser.AuthorizeControlForButton("Edit", allowedMenus);
            //ViewBag.createAccess = AuthorizeUser.AuthorizeControlForButton("Add", allowedMenus);
            //ViewBag.deleteAccess = AuthorizeUser.AuthorizeControlForButton("Delete", allowedMenus);
            GetItemPerPage();
            GetAllQuestionCategory();
            GetDateFormat();
            return View();
        }
        [ValidateAntiForgeryToken]
        public JsonResult AddUpdateEntranceQuestion(EntranceQuestionEntity objInfo)
        {
            string message = String.Empty;
            int ReturnCode;
            try
            {
                objInfo.AddedBy = new User().UserName;
                objInfo.AddedBy = new User().UserName;
                bool status = _EntranceQuestionRepo.AddUpdateEntranceQuestion(objInfo);

                if (objInfo.QuestionID < 1 && status == true)
                {
                    message = StatusCodeDescription.EntranceQuestionAddSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else if (objInfo.QuestionID > 0 && status == true)
                {
                    message = StatusCodeDescription.EntranceQuestionUpdateSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else
                {
                    message = StatusCodeDescription.EntranceQuestionErrorMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                return Json(new { Message = message, ReturnCode = ReturnCode });

            }
            catch (Exception ex)
            {
                return Json(new { Message = StatusCodeDescription.FailureMessage, ReturnCode = StatusCodeDescription.failure });
            }
        }

        public JsonResult GetAllEntranceQuestion([DataSourceRequest] DataSourceRequest request, EntranceSearchQuestionEntity objInfo)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                //QuizSearchQuestionEntity objInfo = JsonConvert.DeserializeObject<QuizSearchQuestionEntity>(objInfo1);
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
                IEnumerable<EntranceQuestionEntity> ListObj = _EntranceQuestionRepo.GetAllEntranceQuestion(objInfo);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [ValidateAntiForgeryToken]
        //[AuthorizeUser(Controls = "Delete")]
        public JsonResult DeleteQuizQuestion(EntranceQuestionEntity objInfo)
        {
            string message = String.Empty;
            int ReturnCode;
            try
            {
                int i = _EntranceQuestionRepo.DeleteEntranceQuestion(objInfo);
                if (i == 1)
                {
                    message = StatusCodeDescription.EntranceQuestionDeleteSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else if (i == 0)
                {
                    message = StatusCodeDescription.EntranceQuestionDependencyMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                else
                {
                    message = StatusCodeDescription.EntranceQuestionDeleteMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                //return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.DeleteQuizQuestion(objInfo)));
                return Json(new { ReturnCode = ReturnCode, Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { ReturnCode = StatusCodeDescription.FailureMessage, Message = StatusCodeDescription.failure });
                //return Json (_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
        //[AuthorizeUser(Controls = "Edit")]
        public JsonResult GetEntranceQuestionByID(int QuestionID)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.GetEntranceQuestionByID(QuestionID)));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetAllEntranceQuestionType()
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {

                //  return _MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.GetAllQuizQuestionType());
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.GetAllEntranceQuestionType()));
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetAllEntranceQuestionDifficultyLevel()
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.GetAllEntranceQuestionDifficulty()));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetAllEntranceQuestionWeightageLevel()
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.GetAllEntranceQuestionWeight()));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }

        [ValidateAntiForgeryToken]
        public JsonResult BatchUpdateEntranceQuestionStatus(string JsonObject)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.BatchUpdateEntranceQuestionStatus(JsonObject)));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetStatusForBatchUpdateQuestionUpdate(string JsonObject)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _EntranceQuestionRepo.GetStatusForBatchUpdateQuestionUpdate(JsonObject)));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        private void GetItemPerPage()
        {
            //string generalSettingGroup = SettingsGroupName.GeneralGroup;
            //string itemPerPageSettingValue = _SettingRepo.GetSettingByIDandGroup("1001", generalSettingGroup);
            int itemPerPage;
            //try
            //{
            //    itemPerPage = Convert.ToInt16(itemPerPageSettingValue);
            //}
            //catch
            //{
            itemPerPage = 10;
            //}

            ViewBag.ItemPerPage = itemPerPage;
        }
        private void GetDateFormat()
        {
            //string generalSettingGroup = SettingsGroupName.GeneralGroup;
            //string settingDate = _SettingRepo.GetSettingByIDandGroup("1023", generalSettingGroup);
            //if (String.IsNullOrEmpty(settingDate))
            //{
                string settingDate = "MM-dd-yyyy";
            //}
            ViewBag.settingDateFormat = "{0:" + settingDate + "}";
        }
        private void GetAllQuestionCategory()
        {
            IEnumerable<CategoryTree> LstCategory = _EntranceQuestionRepo.GetAllEntraceQuestionCategory(CategoryType.CategoryEntranceQuestion);
            ViewBag.EntraceQuestionCategory = new SelectList(LstCategory, "CategoryID", "CategoryName");
            // ViewBag.QuizQuestionStatus = new SelectList(_StatusRepo.GetStatusBasedOnIdentifier(StatusIdentifier.identifierQuizQuestion), "StatusValue", "StatusName");
        }
    }
}