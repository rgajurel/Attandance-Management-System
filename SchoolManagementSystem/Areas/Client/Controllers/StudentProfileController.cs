using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class StudentProfileController : Controller
    {

        private readonly IStudentsProfileRepository stuProfile;

        public StudentProfileController(IStudentsProfileRepository stuProfile)
        {
            this.stuProfile = stuProfile;

        }
        // GET: Client/StudentProfile
        [HttpPost]
        public ActionResult Index(string a)
        {
            ViewBag.StudentsProfile = stuProfile.getStudentsInfo(a);

            return View("Index");
        }
        
    }
}