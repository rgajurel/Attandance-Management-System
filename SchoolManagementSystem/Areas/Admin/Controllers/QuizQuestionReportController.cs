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
    public class QuizQuestionReportController : Controller
    {
        // GET: Admin/QuizQuestionReport
        private readonly IQuizQuestionReportRepository _QuizQuestionReportRepo;
        private readonly IQuizRepository _QuizRepo;
       // private readonly ISettingsRepository _SettingRepo;

        public QuizQuestionReportController(IQuizQuestionReportRepository QuizQuestionReportRepo, IQuizRepository QuizRepo)
        {
            this._QuizQuestionReportRepo = QuizQuestionReportRepo;
            this._QuizRepo = QuizRepo;
            //this._SettingRepo = SettingRepo;
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
      //  [AuthorizeUser(Controls = "View")]
        public ActionResult Index()
        {
            GetItemPerPage();
            return View();
        }
        public JsonResult GetAllQuestion([DataSourceRequest] DataSourceRequest request, SearchParamQuizQuestionreport obj)
        {

            //SearchParamQuizQuestionreport obj = JsonConvert.DeserializeObject<SearchParamQuizQuestionreport>(ObjInfo);
            obj.PageIndex = request.Page;
            obj.PageSize = request.PageSize;
            IEnumerable<QuizQuestionReport> QuizLst = _QuizQuestionReportRepo.GetAllQuestionListing(obj);
            //int total;
            //try
            //{
            //    total = QuizLst.FirstOrDefault().RowTotal;
            //}
            //catch (Exception)
            //{
            //    total = 0;
            //}
            //var result = new DataSourceResult()
            //{
            //    Data = QuizLst,
            //    Total = total
            //};
            return Json(QuizLst, JsonRequestBehavior.AllowGet);

        }
    }
}