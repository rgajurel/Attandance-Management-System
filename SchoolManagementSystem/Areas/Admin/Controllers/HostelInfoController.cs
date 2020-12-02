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
    public class HostelInfoController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IHostelInfoRepository hostelInfoRepository;
        private string message = "";

        // GET: Admin/HostelInfo
        public HostelInfoController(IMessageHandlerRepository messageHandlerRepo, IHostelInfoRepository hostelInfoRepository)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.hostelInfoRepository = hostelInfoRepository;
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
        public JsonResult SaveHostelInfo(HostelInfo hostelInfo)
        {
            try
            {
                if (hostelInfo != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (hostelInfo.ID > 0)
                        {
                            var savechange = hostelInfoRepository.AddUpdateHostelInfo(hostelInfo);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = hostelInfoRepository.AddUpdateHostelInfo(hostelInfo);
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

        public JsonResult DeleteHostelInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = hostelInfoRepository.DeleteHostelInfo(id);
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allHostelInfo = hostelInfoRepository.GetAllHostelInfo();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allHostelInfo != null)
            {
                return new JsonResult()
                {
                    Data = allHostelInfo.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allHostelInfo,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }

        [HttpPost]
        public JsonResult EditHostelInfo(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editBusInfo = hostelInfoRepository.EditHostelInfo(id);
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
    }
}