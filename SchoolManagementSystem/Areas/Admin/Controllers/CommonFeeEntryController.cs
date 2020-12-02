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
    public class CommonFeeEntryController : Controller
    {

        private readonly ICommonFeeRepository commonFeeRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private string message = "";
        public CommonFeeEntryController(ICommonFeeRepository commonFeeRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.commonFeeRepo = commonFeeRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
        }


        // GET: Admin/CommonFeeEntry

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
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

            var classList = dropDownRepo.GetClasswDropDown();
            if (classList != null)
            {
                ViewBag.classList1 = new SelectList(classList, "ID", "Name");
            }
            else
            {
                ViewBag.classList1 = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


            var sessionList = dropDownRepo.GetActiveSessionDropDown();
            if (sessionList != null)
            {
                ViewBag.sessionList = new SelectList(sessionList, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var typeList = dropDownRepo.GetTypeDropDown();
            if (typeList != null)
            {
                ViewBag.typeList = new SelectList(typeList, "ID", "Name");
            }
            else
            {
                ViewBag.typeList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var monthList = dropDownRepo.GetMonthDropDown();
            if (monthList != null)
            {
                ViewBag.month = new SelectList(monthList, "ID", "Name");
            }
            else
            {
                ViewBag.month = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
        }

        [HttpPost]
        public JsonResult GetClassBasedOnFaculty(string faculty)
        {
            try
            {
                if (faculty != null)
                {
                    var Classes = commonFeeRepo.GetClassBasedOnFaculty(faculty);
                    return new JsonResult()
                    {
                        Data = Classes,
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
        public JsonResult GetSectionBaseOnClass(string ID,string faculty)
        {
            try
            {
                if (ID != null && faculty !=null)
                {
                    var sections = commonFeeRepo.GetSectionBasedOnClass(ID,faculty);
                    return new JsonResult()
                    {
                        Data = sections,
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var allCommonFee = commonFeeRepo.GetAllCommonFee().OrderByDescending(model=>model.ID);
            if (allCommonFee != null)
            {
                return new JsonResult()
                {
                    Data = allCommonFee.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allCommonFee,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveCommonFee(CommonFee Fees)
        {

            try
            {
                if (Fees != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (Fees.ID > 0)
                        {
                            var savechange = commonFeeRepo.AddUpdateCommonFee(Fees);

                            if (savechange == "true")
                            {
                                message = MassageDescription.UpdateSuccess;
                            }
                            else if (savechange == "already Inserted")
                            {
                                message = MassageDescription.AlreadyExist;
                            }
                            else
                            {
                                message = MassageDescription.UpdateFailure;
                            }
                            //update
                        }
                        else
                        {
                            var savechange = commonFeeRepo.AddUpdateCommonFee(Fees);
                            if (savechange == "true")
                            {
                                message = MassageDescription.SaveSuccess;
                            }
                            else if (savechange == "Already Inserted") {
                                message = MassageDescription.AlreadyExist;
                            }
                            else
                            {
                                message = MassageDescription.SaveFailure;
                            }
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;

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
        public JsonResult EditCommonFee(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editFees = commonFeeRepo.EditCommonFee(id);
                    return new JsonResult()
                    {
                        Data = editFees,
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
        public JsonResult DeleteCommonFee(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = commonFeeRepo.DeleteCommonFee(id);
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