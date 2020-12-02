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
    public class QuizUserReportController : Controller
    {
        // GET: Admin/QuizUserReport
        private readonly IQuizUserReportRepository _QuizUserReportRepo;
        private readonly IQuizRepository _QuizRepo;
        //private readonly ISettingsRepository _SettingRepo;
        private readonly IDropDownRepository _dropDownRepo;
        //string settingDate;
        public QuizUserReportController(IQuizUserReportRepository QuizUserReportRepo, IQuizRepository QuizRepo, IDropDownRepository dropDownRepo)
        {
            this._QuizUserReportRepo = QuizUserReportRepo;
            this._QuizRepo = QuizRepo;
            //this._SettingRepo = SettingRepo;
            this._dropDownRepo = dropDownRepo;
        }
       // [AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            GetItemPerPage();
            GetDateFormat();
            GetUserGroup();
            GetAllUserList();
            GetQuizStatus();
            return View();
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
        private void GetUserGroup()
        {
            ViewBag.userGroupList = new SelectList(_dropDownRepo.GetUserGroup(), "ID", "GroupName");
        }
        private void GetAllUserList()
        {
            ViewBag.UserList = new SelectList(_QuizUserReportRepo.GetAllUserForQuiz(), "UserID", "UserName");
        }
        private void GetQuizStatus()
        {
            var list = new List<SelectListItem>{
                new SelectListItem{ Text="Select Quiz Status", Value ="" },
                 new SelectListItem{ Text="Completed", Value ="Completed" },
                 new SelectListItem{ Text="Running", Value ="Running" },
                    };
            ViewBag.QuizStatus = new SelectList(list,"Value","Text");
        }
        public JsonResult GetAllQuizUserListing([DataSourceRequest] DataSourceRequest request, string ObjInfo)
        {

            SearchParamQuizUserReport obj = JsonConvert.DeserializeObject<SearchParamQuizUserReport>(ObjInfo);
            obj.PageIndex = request.Page;
            obj.PageSize = request.PageSize;
            IEnumerable<QuizUserReport> QuizLst = _QuizUserReportRepo.GetAllQuizUserListing(obj);
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AssignMarksToUser(int UserScore,int ID)
        {
            if (ModelState.IsValid)
            {
                bool objInfo = _QuizUserReportRepo.AssignMarktoUser(UserScore, ID);
                return Json(new { StatusCodeDescription.success, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });
            }

        }
    }
}