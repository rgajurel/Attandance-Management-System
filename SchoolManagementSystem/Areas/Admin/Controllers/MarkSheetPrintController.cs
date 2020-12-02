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
    public class MarkSheetPrintController : Controller
    {
        private readonly IMarksSheetPrintRepository marksSheetPrintRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        // GET: Admin/MarkSheetPrint
        public MarkSheetPrintController(IMarksSheetPrintRepository marksSheetPrintRepo, IMessageHandlerRepository messageHandlerRepo, IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.marksSheetPrintRepo = marksSheetPrintRepo;
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

        public ActionResult GetAllMarkSheet(MarkSheetPrint markshettprint)
        {
            try
            {
                var allmarkSheet = marksSheetPrintRepo.GetAllMarkSheets(markshettprint);

                if (allmarkSheet != null)
                {
                    return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, allmarkSheet.OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                                           
                }
                else
                {
                    return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.NoData, null), JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
               // return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.NoData, null), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, MarkSheetPrint marksheetprint)
        {
            try
            {
                var marksSheetList = marksSheetPrintRepo.GetAllStudents(marksheetprint);

                if (marksSheetList != null || marksSheetList.Count() > 0)
                {
                    return new JsonResult()
                    {
                        Data = marksSheetList.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = marksSheetList,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}