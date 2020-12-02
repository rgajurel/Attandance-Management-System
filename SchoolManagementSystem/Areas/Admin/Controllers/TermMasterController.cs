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
    public class TermMasterController : Controller
    {

        private readonly ITermMasterRepository termMasterRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        public TermMasterController(ITermMasterRepository termMasterRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.termMasterRepo = termMasterRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        // GET: Admin/TermMaster
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
        public JsonResult SaveTermMaster(List<TermMaster> termMaster)
        {
            try
            {
                if (termMaster != null)
                {
                    if (ModelState.IsValid)
                    {
                        foreach (var termmaster in termMaster)
                        {
                            if (string.IsNullOrEmpty(termmaster.TermName))
                            {
                                continue;
                            }
                            else
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
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allSectionInfo = termMasterRepo.GetAllTermMaster();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allSectionInfo != null)
            {
                return new JsonResult()
                {
                    Data = allSectionInfo.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allSectionInfo,
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
            catch (Exception)
            {
                return Json(new { success = false, responseText = MassageDescription.ExceptionOrNullError }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}