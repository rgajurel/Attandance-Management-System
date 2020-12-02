using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class MarksEntryCaseController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMarksEntryRepository marksRepo;
        private string message = "";
        // GET: Admin/MarksEntryCase

        public MarksEntryCaseController(IMessageHandlerRepository messageHandlerRepo, IMarksEntryRepository marksRepo, IDropDownRepository dropDownRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
            this.marksRepo = marksRepo;
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
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