using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class ParentsController : Controller
    {
        private readonly IParentsChildRepository parentsChildRepo;

        public ParentsController(IParentsChildRepository parentsChildRepo)
        {
            this.parentsChildRepo = parentsChildRepo;

        }
        // GET: Client/Parents
        public ActionResult Parents()
        {
            try
            {
                if (Session["parentEmail"].ToString() == "")
                {
                    return RedirectToAction("Index", "Home", new { area = "Client" });
                }
                else
                {
                    ViewBag.StudentsList = parentsChildRepo.GetAllStudents(Session["parentEmail"].ToString());
                    Session["test"] = parentsChildRepo.GetAllStudents(Session["parentEmail"].ToString());
                    return View(ViewBag.StudentsList);
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
            
        }
        public ActionResult Logout()
        {
            Session.Remove("parentEmail");
            return RedirectToAction("Index", "Home", new { area = "Client" });
        }
    }
}