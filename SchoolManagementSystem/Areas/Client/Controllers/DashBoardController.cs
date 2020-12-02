using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: Client/DashBoard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadPartialView()
        {            
            return PartialView("View");
        }
    }
}