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
    public class HouseInfoController : Controller
    {

        private readonly IHouseInfoRepository houseInfoRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";

        public HouseInfoController(IHouseInfoRepository houseInfoRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.houseInfoRepo = houseInfoRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }

        // GET: Admin/HouseInfo
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
        public JsonResult saveHouseInfo(HouseInfo houseInfo)
        {
            try
            {
                if (houseInfo != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (houseInfo.ID > 0)
                        {
                            var savechange = houseInfoRepo.AddUpdateHouseInfo(houseInfo);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = houseInfoRepo.AddUpdateHouseInfo(houseInfo);
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

            var houses = houseInfoRepo.GetAllHouseInfo();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (houses != null)
            {
                return new JsonResult()
                {
                    Data = houses.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = houses,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
           



        }

        [HttpPost]
        public JsonResult editHouseInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSchoolType = houseInfoRepo.EditHouseInfo(id);
                    return new JsonResult()
                    {
                        Data = editSchoolType,
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
        public JsonResult deleteHouseInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = houseInfoRepo.DeleteHouseInfo(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.Deleteailure;
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