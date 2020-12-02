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
    public class FeeTypeController : Controller
    {
        private readonly IFeeTypeRepository feeTypeRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/FeeType
        public FeeTypeController(IFeeTypeRepository feeTypeRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.feeTypeRepo = feeTypeRepo;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveFeeType(FeeType feeType)
        {
            try
            {
                if (feeType != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (feeType.ID > 0)
                        {
                            var savechange = feeTypeRepo.AddUpdateFeeType(feeType);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = feeTypeRepo.AddUpdateFeeType(feeType);
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
            try
            {


                var allFeeType = feeTypeRepo.GetAllFeeType();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allFeeType != null)
                {
                    return new JsonResult()
                    {
                        Data = allFeeType.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allFeeType,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
            }
            catch(Exception ex)
            {
                return null;
            }



        }

        [HttpPost]
        public JsonResult EditFeeType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editFeeType = feeTypeRepo.EditFeeType(id);
                    return new JsonResult()
                    {
                        Data = editFeeType,
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
        public JsonResult DeleteFeeType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = feeTypeRepo.DeleteFeeType(id);
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
    }
}