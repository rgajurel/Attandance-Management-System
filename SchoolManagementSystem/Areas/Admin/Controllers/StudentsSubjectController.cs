using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class StudentsSubjectController : Controller
    {

        private readonly IDropDownRepository dropDownRepo;       
        private readonly IMessageHandlerRepository messageHandlerRepo;
        // GET: Admin/StudentsSubject

        public StudentsSubjectController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public ActionResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }
        public void LoadDropDown()
        {
            var classList = dropDownRepo.GetClasswDropDown();
            if (classList != null)
            {
                ViewBag.classList = new SelectList(classList, "ID", "Name");
            }
            else
            {
                ViewBag.classList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
        }
    }
}