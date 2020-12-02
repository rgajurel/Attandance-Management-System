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
    public class EntranceUserReportController : Controller
    {
        private readonly IEntranceUserReportRepository _EntranceUserReportRepo;
        private readonly IEntranceRepository _EntranceRepo;
        private readonly IDropDownRepository _dropDownRepo;

        public EntranceUserReportController(IEntranceUserReportRepository EntranceUserReportRepo, IEntranceRepository EntranceRepo, IDropDownRepository dropDownRepo)
        {
            this._EntranceUserReportRepo = EntranceUserReportRepo;
            this._EntranceRepo = EntranceRepo;
            //this._SettingRepo = SettingRepo;
            this._dropDownRepo = dropDownRepo;
        }
        // GET: Admin/EntranceUserReport
        public ActionResult Index()
        {
            GetItemPerPage();
            GetDateFormat();
            GetUserGroup();
            GetAllUserList();
            GetEntranceStatus();
            return View();
        }
        private void GetItemPerPage()
        {

            ViewBag.ItemPerPage = 10;
        }
        private void GetDateFormat()
        {
            ViewBag.settingDateFormat = "{0:" + "MM-dd-yyyy" + "}";
        }
        private void GetUserGroup()
        {
            ViewBag.userGroupList = new SelectList(_dropDownRepo.GetUserGroup(), "ID", "GroupName");
        }
        private void GetAllUserList()
        {
            ViewBag.UserList = new SelectList(_EntranceUserReportRepo.GetAllUserForEntrance(), "UserID", "UserName");
        }
        private void GetEntranceStatus()
        {
            var list = new List<SelectListItem>{
                new SelectListItem{ Text="Select Entrance Status", Value ="" },
                 new SelectListItem{ Text="Completed", Value ="Completed" },
                 new SelectListItem{ Text="Running", Value ="Running" },
                    };
            ViewBag.QuizStatus = new SelectList(list, "Value", "Text");
        }
        public JsonResult GetAllEntranceUserListing([DataSourceRequest] DataSourceRequest request, string ObjInfo)
        {

            SearchParamEntranceUserReport obj = JsonConvert.DeserializeObject<SearchParamEntranceUserReport>(ObjInfo);
            obj.PageIndex = request.Page;
            obj.PageSize = request.PageSize;
            IEnumerable<EntranceUserReport> QuizLst = _EntranceUserReportRepo.GetAllEntranceUserListing(obj);
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
        public ActionResult GetUserEntranceAnswerByUserID(int EntranceUserID)
        {
            if (ModelState.IsValid)
            {
                EntranceQuestionUserReport objInfo = _EntranceUserReportRepo.UserEntranceAnswerByUserID(EntranceUserID);
                return Json(new { StatusCodeDescription.success, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });
            }

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AssignMarksToUser(int UserScore, int ID)
        {
            if (ModelState.IsValid)
            {
                bool objInfo = _EntranceUserReportRepo.AssignMarktoUser(UserScore, ID);
                return Json(new { StatusCodeDescription.success, data = objInfo });
            }
            else
            {
                return Json(new { StatusCodeDescription.failure, StatusCodeDescription.QuizErrorMessage });
            }

        }
    }
}