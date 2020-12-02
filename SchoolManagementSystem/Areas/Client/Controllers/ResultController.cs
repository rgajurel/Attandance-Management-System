using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class ResultController : Controller
    {
        private readonly IClientResultRepository clientResult;
        private readonly IMarksSheetPrintRepository marksSheetPrintRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        public ResultController(IMessageHandlerRepository messageHandlerRepo,IClientResultRepository clientResult, IMarksSheetPrintRepository marksSheetPrintRepo)
        {
            this.clientResult = clientResult;
            this.messageHandlerRepo = messageHandlerRepo;
            this.marksSheetPrintRepo = marksSheetPrintRepo;
        }

        [HttpPost]
        public ActionResult Index(string a, string b)
        {
            var termsList = clientResult.getPublishedTerms(a, b);
            if (termsList != null)
            {
                ViewBag.termsList = new SelectList(termsList, "ID", "Name");
            }
            else
            {
                ViewBag.termsList = new SelectList("ID", "Name");
            }
            return PartialView("Index");
        }

        [HttpPost]
        public ActionResult ViewResult(string p, string q, string r, string s, string t, string u)
        {
            try
            {
                MarkSheetPrint markshettprint = new MarkSheetPrint();
                markshettprint = marksSheetPrintRepo.GetStudentInfoForClient(t,p,q,s,u);
               

                var allmarkSheet = marksSheetPrintRepo.GetAllMarkSheets(markshettprint);
                
                if (allmarkSheet != null)
                {
                    return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, allmarkSheet.OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                    
                }
                else
                {
                    return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, "Error !!!. Please Contact School/College.", null), JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
                
            }
        }


        [HttpPost]
        public ActionResult getResultType(string p, string q, string r, string s, string t, string u)
        {
            try
            {
                MarkSheetPrint markshettprint = new MarkSheetPrint();
                markshettprint = marksSheetPrintRepo.GetStudentInfoForClient(t, p, q, s, u);
                var resulttype = markshettprint.ResultType;

                if (markshettprint != null)
                {
                    return Content(resulttype.ToString());

                }
                else
                {
                    return Content("1");
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

    }
}