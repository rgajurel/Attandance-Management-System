using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class FeeDetailsController : Controller
    {
        private readonly IFeeDetailsForClientRepository FeeDetailsRepo;

        public FeeDetailsController(IFeeDetailsForClientRepository FeeDetailsRepo)
        {
            this.FeeDetailsRepo = FeeDetailsRepo;
        }
        // GET: Client/FeeDetails
        [HttpPost]
        public ActionResult Index(string a, string b)
        {
            var studentsFee = FeeDetailsRepo.GetFeeDetails(a, b);
            var collectionDetails = FeeDetailsRepo.getCollectionDetails(a, b);
            ViewBag.StudentsFee= studentsFee;
            ViewBag.StudentsCollection = collectionDetails;
            return PartialView("Index");
        }

        
    }
}