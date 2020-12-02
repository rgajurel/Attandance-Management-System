using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class MarksSheetLedgerController : Controller
    {

        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMarkSheetLedgerRepository marksLedgerRepo;
        private readonly IGradeMasterRepository gradeMasterRepo;
        // GET: Admin/MarksSheetLedger

        public MarksSheetLedgerController(IGradeMasterRepository gradeMasterRepo, IMessageHandlerRepository messageHandlerRepo, IMarkSheetLedgerRepository marksLedgerRepo ,IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.marksLedgerRepo = marksLedgerRepo;
            this.gradeMasterRepo = gradeMasterRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        [HttpPost]
        public JsonResult GetFullMarksSheetLedger(MarksEntry marksEntry)
        {
            try
            {
                if (marksEntry != null)
                {
                    var allGrade = gradeMasterRepo.GetAllGradeMaster().OrderByDescending(model => model.ID).ToList();
                    var marksSheetLedgerList = marksLedgerRepo.GetAllMarksSheetLedger(marksEntry).OrderBy(model=>model.RollNo);
              if (allGrade.Count()>=0 && marksSheetLedgerList.Count()>0)
               { 
                    foreach (var ledger in marksSheetLedgerList)
                    {
                        foreach (var grade in allGrade)
                        {
                            if ((Convert.ToDecimal(ledger.TotalObtained) >= grade.MarksFrom) && (Convert.ToDecimal(ledger.TotalObtained) <= grade.MarksTo))
                            {
                                ledger.TotalObtained = ledger.Total + "(" + grade.GradePoint + ")" + "(" + grade.Grade + ")";
                                break;
                            }

                        }

                    }
                        return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.ExceptionOrNullError, marksSheetLedgerList.OrderBy(model=>model.RollNo).OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.NoData, null), JsonRequestBehavior.AllowGet);
                    }

                }
               
                else
                {

                    return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.NoData, null), JsonRequestBehavior.AllowGet);
                }



            }


            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
            }
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
    }
}