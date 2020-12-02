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
    public class TermMasterDynamicController : Controller
    {
        private readonly ITermMasterRepository termMasterRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/TermMasterDynamic
        public TermMasterDynamicController(ITermMasterRepository termMasterRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.termMasterRepo = termMasterRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadPartialView()
        {
            return PartialView("View");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(TermMaster termmaster, string data )
        {
            try
            {
                if (termmaster != null)
                {
                    if (ModelState.IsValid)
                    {
                       
                           
                                if (termmaster.ID > 0)
                                {
                                    var savechange = termMasterRepo.AddUpdateTermMaster(termmaster);
                                    message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                                    //update
                                }
                                else
                                {
                                    var savechange = termMasterRepo.AddUpdateTermMaster(termmaster);
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

            var allTermMasteInfo = termMasterRepo.GetAllTermMaster();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allTermMasteInfo != null)
            {
                return new JsonResult()
                {
                    Data = allTermMasteInfo.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allTermMasteInfo,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditTermMaster(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editTermMaster = termMasterRepo.EditTermMaster(id);
                    return new JsonResult()
                    {
                        Data = editTermMaster,
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

        public JsonResult DeleteTermMasterDynamic(int  id)
        {
            try
            {
                if (id!=0)
                {
                    var savechanges = termMasterRepo.DeleteTermMaster(id);
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