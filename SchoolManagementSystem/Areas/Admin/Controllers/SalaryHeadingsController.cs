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
    public class SalaryHeadingsController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly ISalaryHeadRepository salaryHeadRepo;
        private string message = "";
        // GET: Admin/SalaryHeadings

        public SalaryHeadingsController(IMessageHandlerRepository messageHandlerRepo, ISalaryHeadRepository salaryHeadRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.salaryHeadRepo = salaryHeadRepo;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPartialView()
        {
            return View("View");
        }


        [HttpPost]
        public JsonResult DeleteSalaryHeading(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = salaryHeadRepo.DeleteSalaryHeading(id);
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
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSalaryHeadings(SalaryHeading salHead)
        {
            try
            {
                if (salHead != null)
                {
                   
                    if (ModelState.IsValid)
                    {
                        if (salHead.ID > 0)
                        {
                            var savechange = salaryHeadRepo.AddUpdateSalaryHeading(salHead);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = salaryHeadRepo.AddUpdateSalaryHeading(salHead);
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

            var allSalaryHeadings = salaryHeadRepo.GetAllSalaryHeading();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allSalaryHeadings != null)
            {
                return new JsonResult()
                {
                    
                    Data = allSalaryHeadings.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allSalaryHeadings,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditSalaryHeadings(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSalaryHeadings = salaryHeadRepo.EditSalaryHeading(id);
                    return new JsonResult()
                    {
                        Data = editSalaryHeadings,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}