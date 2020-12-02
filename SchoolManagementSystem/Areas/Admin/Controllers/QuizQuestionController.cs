using DomainEntities;
using DomainInterface;
using Infrastructure;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using TechtonneMS.Helper;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class QuizQuestionController : Controller
    {
        // GET: Admin/QuizQuestion
        private readonly IQuizQuestionRepository _QuizRepo;
        private readonly IMessageHandlerRepository _MessageRepo;
        //private readonly ISettingsRepository _SettingRepo;
        //private readonly IStatusRepository _StatusRepo;
        public QuizQuestionController(IQuizQuestionRepository QuizRepo, IMessageHandlerRepository MessageRepo)
        {
            this._QuizRepo = QuizRepo;
            this._MessageRepo = MessageRepo;
            //this._SettingRepo = SettingRepo;
            //this._StatusRepo = StatusRepo;
        }
        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            //IEnumerable<DomainEntities.Menu> allowedMenus = MenuRepository.GetMenuAccessBasedOnRole(new LoginUser().UserName);

            //ViewBag.editAccess = AuthorizeUser.AuthorizeControlForButton("Edit", allowedMenus);
            //ViewBag.createAccess = AuthorizeUser.AuthorizeControlForButton("Add", allowedMenus);
            //ViewBag.deleteAccess = AuthorizeUser.AuthorizeControlForButton("Delete", allowedMenus);
            GetItemPerPage();
            //GetAllQuizStatus();
            GetDateFormat();
            return View();
        }
        [ValidateAntiForgeryToken]
        public JsonResult AddUpdateQuizQuestion(QuizQuestionEntity objInfo)
        {
            string message = String.Empty;
            int ReturnCode;
            try
            {
                objInfo.AddedBy = new User().UserName;
                objInfo.AddedBy= new User().UserName;
                bool status = _QuizRepo.AddUpdateQuizQuestion(objInfo);

                if (objInfo.QuestionID<1 && status==true)
                {
                    message = StatusCodeDescription.QuizQuestionAddSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else if(objInfo.QuestionID > 0 && status == true)
                {
                    message = StatusCodeDescription.QuizQuestionUpdateSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else
                {
                    message = StatusCodeDescription.QuizQuestionErrorMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                return Json(new { Message= message , ReturnCode =ReturnCode});

            }
            catch (Exception ex)
            {
                return Json(new { Message = StatusCodeDescription.FailureMessage, ReturnCode = StatusCodeDescription.failure });
            }
        }

        public JsonResult GetAllQuizQuestion([DataSourceRequest] DataSourceRequest request, QuizSearchQuestionEntity objInfo)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
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
                IEnumerable<QuizQuestionEntity> ListObj = _QuizRepo.GetAllQuizQuestion(objInfo);
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
        public JsonResult DeleteQuizQuestion(QuizQuestionEntity objInfo)
        {
            string message = String.Empty;
            int ReturnCode;
            try
            {
                int i = _QuizRepo.DeleteQuizQuestion(objInfo);
                if (i==1)
                {
                    message = StatusCodeDescription.QuizQuestionDeleteSuccess;
                    ReturnCode = StatusCodeDescription.success;
                }
                else if(i==0)
                {
                    message = StatusCodeDescription.QuizQuestionDependencyMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                else
                {
                    message = StatusCodeDescription.QuizQuestionDeleteMessage;
                    ReturnCode = StatusCodeDescription.failure;
                }
                //return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.DeleteQuizQuestion(objInfo)));
                return Json(new { ReturnCode = ReturnCode, Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { ReturnCode = StatusCodeDescription.FailureMessage, Message = StatusCodeDescription.failure });
                //return Json (_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
       // [AuthorizeUser(Controls = "Edit")]
        public JsonResult GetQuizQuestionByID(int QuestionID)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.GetQuizQuestionByID(QuestionID)));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetAllQuizQuestionType()
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {

                //  return _MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.GetAllQuizQuestionType());
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.GetAllQuizQuestionType()));
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetAllQuizQuestionDifficultyLevel()
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.GetAllQuizQuestionDifficulty()));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult GetAllQuizQuestionWeightageLevel()
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.GetAllQuizQuestionWeight()));
            }
            catch (Exception ex)
            {
                return Json(_MessageRepo.GetErrorMessageWithData(true, ex.Message.ToString(), null));
            }
        }

        [ValidateAntiForgeryToken]
        public JsonResult BatchUpdateQuizQuestionStatus(string JsonObject)
        {
            string message = String.Empty;
            MessageHolder basicData = new MessageHolder();
            try
            {
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.BatchUpdateQuizQuestionStatus(JsonObject)));
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
                return Json(_MessageRepo.GetSuccessMessageWithData(true, StatusCodeDescription.successMessage, _QuizRepo.GetStatusForBatchUpdateQuestionUpdate(JsonObject)));
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
        private void GetDateFormat()
        {
            //string generalSettingGroup = SettingsGroupName.GeneralGroup;
            //string settingDate = _SettingRepo.GetSettingByIDandGroup("1023", generalSettingGroup);
            //if (String.IsNullOrEmpty(""))
            //{
            //    settingDate = "MM-dd-yyyy";
            //}
            ViewBag.settingDateFormat = "{0:" + "MM-dd-yyyy" + "}";
        }
        //private void GetAllQuizStatus()
        //{
        //    ViewBag.QuizQuestionStatus = new SelectList(_StatusRepo.GetStatusBasedOnIdentifier(StatusIdentifier.identifierQuizQuestion), "StatusValue", "StatusName");
        //}
    }
}