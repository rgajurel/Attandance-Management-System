using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class TravellingAllowanceController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly ITravellingAllowanceRepository travelAllowanceRepo;
        private string message = "";
        // GET: Admin/TravellingAllowance

        public TravellingAllowanceController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, ITravellingAllowanceRepository travelAllowanceRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.travelAllowanceRepo = travelAllowanceRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public void LoadDropDown()
        {
            var allActiveSession = dropDownRepo.GetActiveSessionDropDown();
            if (allActiveSession != null)
            {
                ViewBag.sessionList = new SelectList(allActiveSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionListAll = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionListAll = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allMonth = dropDownRepo.GetMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveTravellingAllownace(TravellingAllowance allowance)
        {
            try
            {
                if (allowance != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (allowance.ID > 0)
                        {

                            var savechange = travelAllowanceRepo.AddUpdateTravellingAllowance(allowance);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {

                            var savechange = travelAllowanceRepo.AddUpdateTravellingAllowance(allowance);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;

                        //model error occured
                    }
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                    //null error occured
                }
                return Json(messageHandlerRepo.GetMessage(message));
            }
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }
    }
}