using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class MarksEntryEditController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
       
        private readonly IEditMarksEntryRepository marksRepoEdit;

        // GET: Admin/MarksEntryEdit
        public MarksEntryEditController(IDropDownRepository dropDownRepo, IEditMarksEntryRepository marksRepoEdit)
        {
            this.dropDownRepo = dropDownRepo;
            this.marksRepoEdit = marksRepoEdit;

        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }
        public PartialViewResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, MarksEntry search)
        {


            var studentsList = marksRepoEdit.GetAllMarksEntryEdit(search);

            if (studentsList != null || studentsList.Count() >0)
            {
                return new JsonResult()
                {
                    Data = studentsList.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = studentsList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }


        }

        public void LoadDropDown()
        {
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

    }
}