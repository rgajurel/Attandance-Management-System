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
    public class DepartmentController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDepartmentRepository departmentRepo;
        private string message = "";
        // GET: Admin/Department
        public DepartmentController(IMessageHandlerRepository messageHandlerRepo, IDropDownRepository dropDownRepo, IDepartmentRepository departmentRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.departmentRepo = departmentRepo;
            this.dropDownRepo=dropDownRepo;
        }

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
        public JsonResult SaveDepartment(Department department)
        {
            try
            {
                if (department != null)
                {
                    department.DepartmentName = department.DepartmentName.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (department.ID > 0)
                        {
                            var savechange = departmentRepo.AddUpdateDepartment(department);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = departmentRepo.AddUpdateDepartment(department);
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
        public JsonResult DeleteDepartment(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = departmentRepo.DeleteDepartment(id);
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

        [HttpPost]
        public JsonResult EditDepartment(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editDesignation = departmentRepo.EditDepartment(id);
                    return new JsonResult()
                    {
                        Data = editDesignation,
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
            try
            {

                var allDepartment = departmentRepo.GetAllDepartment();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allDepartment != null)
                {
                    return new JsonResult()
                    {
                        Data = allDepartment.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allDepartment,
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
    }
}