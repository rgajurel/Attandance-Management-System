using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class AccumulativeLeaveController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IAccumulativeLeaveRepository accumulativeLeaveRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/AccumulativeLeave

        public AccumulativeLeaveController(IMessageHandlerRepository messageHandlerRepo, IAccumulativeLeaveRepository accumulativeLeaveRepo, IDropDownRepository dropDownRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.accumulativeLeaveRepo = accumulativeLeaveRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        public PartialViewResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }


        public void LoadDropDown()
        {
            var serializer = new JavaScriptSerializer();
            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionListAll = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionListAll = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

        }
        public JsonResult AutoComplete(string text,int organisation)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    text = null;
                }

                var employee = (accumulativeLeaveRepo.GetAllEmployee(text,organisation));
                if (employee!=null)
                {
                    return Json(employee, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(new { success = false, responseText = "No Records Found" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetLeaveTypeBasedOnOrganisation(string ID)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    var leaveType = accumulativeLeaveRepo.GetLeaveTypeBasedOnOrganisation(Convert.ToString(ID));
                    return new JsonResult()
                    {
                        Data = leaveType,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveAccumulativeLeave(AccumulativeLeave accumulativeLeave)
        {
            try
            {
                ModelState.Remove("EmployeeID");
                if (accumulativeLeave != null)
                {                   
                    if (ModelState.IsValid)
                    {
                        if (accumulativeLeave.ID > 0)
                        {
                            var savechange = accumulativeLeaveRepo.AddUpdateAccumulativeLeave(accumulativeLeave);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = accumulativeLeaveRepo.AddUpdateAccumulativeLeave(accumulativeLeave);
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
                throw ex;

            }


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try
            {

                var allAccumulativeLeave = accumulativeLeaveRepo.GetAllAccumulativeLeave();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allAccumulativeLeave != null)
                {
                    return new JsonResult()
                    {
                        Data = allAccumulativeLeave.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allAccumulativeLeave,
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

        [HttpPost]
        public JsonResult EditAccumulativeLeave(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editEmployer = accumulativeLeaveRepo.EditAccumulativeLeave(id);
                    return new JsonResult()
                    {
                        Data = editEmployer,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {
                throw ex;
                //throw ex;
                // return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }

    }
}