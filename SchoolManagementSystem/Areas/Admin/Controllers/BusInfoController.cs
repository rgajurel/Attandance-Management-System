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
    public class BusInfoController : Controller
    {
        private readonly IBusInfoRepository busInfoRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";


        public BusInfoController(IBusInfoRepository busInfoRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.busInfoRepo = busInfoRepo;
            this.messageHandlerRepo = messageHandlerRepo;

        }
        // GET: Admin/BusInfo

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public  ActionResult Index()
        {
            ViewBag.error = "Hello";
            return View();
        }
        public PartialViewResult LoadPartialView()
        {
            return PartialView("View");
        }


        [HttpPost]
        public JsonResult EditBusInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editBusInfo = busInfoRepo.EditBusInfo(id);
                    return new JsonResult()
                    {
                        Data = editBusInfo,
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveBusInfo(BusInfo busInfo)
        {
            try
            {
                if (busInfo != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (busInfo.ID > 0)
                        {
                            var savechange = busInfoRepo.AddUpdateBusInfo(busInfo);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = busInfoRepo.AddUpdateBusInfo(busInfo);
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
            try {
                var allBusInfo = busInfoRepo.GetAllBusInfo();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allBusInfo != null)
                {
                    return new JsonResult()
                    {
                        Data = allBusInfo.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allBusInfo,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }

            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public JsonResult DeleteBusInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = busInfoRepo.DeleteBusInfo(id);
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