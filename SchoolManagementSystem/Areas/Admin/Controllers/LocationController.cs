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
    public class LocationController : Controller
    {
        public readonly ILocationInfoRepository locationRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/Location
        public LocationController(ILocationInfoRepository locationRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.locationRepo = locationRepo;
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

            var allLocation = locationRepo.GetAllLocationInfo();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allLocation != null)
            {
                return new JsonResult()
                {
                    Data = allLocation.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allLocation,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditLocation(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editLocation = locationRepo.EditLocationInfo(id);
                    return new JsonResult()
                    {
                        Data = editLocation,
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
        public JsonResult DeleteLocation(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = locationRepo.DeleteLocationInfo(id);
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult SaveLocation(Location location)
        //{
        //    try
        //    {
        //        if (location != null)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                if (location.ID > 0)
        //                {
        //                    var savechange = locationRepo.AddUpdateLocationInfo(location);
        //                    message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
        //                    //update
        //                }
        //                else
        //                {
        //                    var savechange = locationRepo.AddUpdateLocationInfo(location);
        //                    message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
        //                    //add
        //                }

        //            }
        //            else
        //            {
        //                message = MassageDescription.ModelErrorOccured;
        //                // return Json(messageHandlerRepo.GetMessage(message));
        //                //model error occured
        //            }
        //        }
        //        else
        //        {
        //            message = MassageDescription.ExceptionOrNullError;
        //            //null error occured
        //        }
        //        return Json(messageHandlerRepo.GetMessage(message));
        //    }
        //    catch (Exception ex)
        //    {
        //        message = MassageDescription.ExceptionOrNullError;
        //        return Json(messageHandlerRepo.GetMessage(message));

        //    }


        //}

    }
}