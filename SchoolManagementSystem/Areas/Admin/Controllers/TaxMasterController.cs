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
    public class TaxMasterController : Controller
    {
        private readonly ITaxMasterRepository taxMasterRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = String.Empty;
        // GET: Admin/TaxMaster
        public TaxMasterController(ITaxMasterRepository taxMasterRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.taxMasterRepo = taxMasterRepo;
            this.messageHandlerRepo = messageHandlerRepo;

        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult LoadPartialView()
        {
            return PartialView("View");
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allTaxMaster = taxMasterRepo.GetAllTaxMaster();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allTaxMaster != null)
            {
                return new JsonResult()
                {
                    Data = allTaxMaster.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allTaxMaster,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveTaxMaster(TaxMaster taxMaster)
        {
            try
            {
                if (taxMaster != null)
                {                   
                    if (ModelState.IsValid)
                    {
                        if (taxMaster.ID > 0)
                        {
                            var savechange = taxMasterRepo.AddUpdateTextMaster(taxMaster);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = taxMasterRepo.AddUpdateTextMaster(taxMaster);
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

        [HttpPost]
        public JsonResult EditTaxMaster(int id)
        {
            try
            {
                if (id != 0)
                {
                    var taxMaster = taxMasterRepo.EditTaxMaster(id);
                    return new JsonResult()
                    {
                        Data = taxMaster,
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
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteTaxMaster(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = taxMasterRepo.DeleteTaxMaster(id);
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