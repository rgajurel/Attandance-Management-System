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
    public class TakeAdvanceController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly ITakeAdvanceRepository takeAdvanceRepository;
        private string message = "";
        // GET: Admin/TakeAdvance
        public TakeAdvanceController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, ITakeAdvanceRepository takeAdvanceRepository)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.takeAdvanceRepository = takeAdvanceRepository;

        }
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
        public JsonResult SaveTakeAdvance(TakeAdvance takeAdvance)
        {
            try
            {
                if (takeAdvance != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (takeAdvance.ID > 0)
                        {
                            var savechange = takeAdvanceRepository.AddUpdateTakeAdvance(takeAdvance);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = takeAdvanceRepository.AddUpdateTakeAdvance(takeAdvance);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;
                        // return Json(messageHandlerRepo.GetMessage(message));
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
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allTakeAdvance = takeAdvanceRepository.GetAllTakeAdvance();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allTakeAdvance != null)
            {
                return new JsonResult()
                {
                    Data = allTakeAdvance.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allTakeAdvance,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditTakeAdvance(int id)
        {
            try
            {
                if (id != 0)
                {
                    var edittakeAdvance = takeAdvanceRepository.EditTakeAdvance(id);
                    return new JsonResult()
                    {
                        Data = edittakeAdvance,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
                }


            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
                // return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }

        public JsonResult DeleteTakeAdvance(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = takeAdvanceRepository.DeleteTakeadvance(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                }
                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}