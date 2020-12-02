using DomainEntities;
using DomainInterface;
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
    public class QuizReportController : Controller
    {
        // GET: Admin/QuizReport
        string settingDate;
        //[AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            //GetAllQuizStatus();
            //GetAllQuizCategory();
            GetDateFormat();
            GetItemPerPage();
            GetUserGroup();
         //   GetGridPageSize();
            return View();
        }
        private readonly IQuizReport _QuizReportRepo;
       // private readonly ISettingsRepository _SettingRepo;
        private readonly IQuizRepository _QuizRepo;
       // private readonly IStatusRepository _statusRepo;
       // private readonly IUserGroup _userGroupRepo;
        private readonly IDropDownRepository _dropDownRepo;
        private readonly IQuizUserReportRepository _QuizUserReportRepo;
        public QuizReportController(IQuizUserReportRepository QuizUserReportRepo, IQuizReport QuizReportRepo, IQuizRepository QuizRepo,  IDropDownRepository dropDownRepo)
        {
            this._QuizReportRepo = QuizReportRepo;
            //this._SettingRepo = SettingRepo;
            this._QuizRepo = QuizRepo;
            //this._statusRepo = statusRepo;
            //this._userGroupRepo = userGroupRepo;
            this._dropDownRepo = dropDownRepo;
            this._QuizUserReportRepo = QuizUserReportRepo;
        }

        public JsonResult GetAllQuiz([DataSourceRequest] DataSourceRequest request, string objInfo1)
        {
            string message = String.Empty;
            try
            {
                QuizReportSearch objInfo = JsonConvert.DeserializeObject<QuizReportSearch>(objInfo1);
                objInfo.PageIndex = request.Page;
                objInfo.PageSize = request.PageSize;
                IEnumerable<QuizReport> ListObj = _QuizReportRepo.GetAllQuizListing(objInfo);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetQuizByID(int QuizID)
        {
            if (ModelState.IsValid)
            {
                QuizEntity objInfo = _QuizReportRepo.GetQuizByID(QuizID);
                GetDateFormat();
                objInfo.DateFormat = settingDate;
                return Json(new { StatusCodeDescription.success, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });
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
            //settingDate = _SettingRepo.GetSettingByIDandGroup("1023", generalSettingGroup);
            //if (String.IsNullOrEmpty(settingDate))
            //{
            //    settingDate = "MM-dd-yyyy";
            //}
            ViewBag.settingDateFormat = "{0:" + "MM-dd-yyyy" + "}";
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
        private void GetUserGroup()
        {
            ViewBag.userGroupList = new SelectList(_dropDownRepo.GetUserGroup(), "ID", "GroupName");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetUserQuizAnswerByUserID(int QuizUserID)
        {
            if (ModelState.IsValid)
            {
                QuizQuestionUserReport objInfo = _QuizUserReportRepo.UserQuizAnswerByUserID(QuizUserID);
                return Json(new { StatusCodeDescription.success, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });
            }

        }
    }
}