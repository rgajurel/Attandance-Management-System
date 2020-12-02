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
    public class StudentsCategoryController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IStudentsCategoryRepository studentsCategoryRepo;
        private string message = "";

        public StudentsCategoryController(IMessageHandlerRepository messageHandlerRepo, IStudentsCategoryRepository studentsCategoryRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.studentsCategoryRepo = studentsCategoryRepo;
        }
        // GET: Admin/StudentsCategory
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
        public JsonResult SaveStudentCategory(StudentsCategorys category)
        {
            try
            {
                if (category != null)
                {
                    category.StudentsCategory = category.StudentsCategory.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (category.ID > 0)
                        {
                            var savechange = studentsCategoryRepo.AddUpdateStudentsCategory(category);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = studentsCategoryRepo.AddUpdateStudentsCategory(category);
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
            catch (Exception )
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }


        }

        [HttpPost]
        public JsonResult DeleteStudentCategory(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = studentsCategoryRepo.DeleteStudentsCategory(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
                }
                else
                {
                    message = MassageDescription.ExceptionOrNullError;
                }
                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }
        }

        [HttpPost]
        public JsonResult EditStudentCategory(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editStudentsCategory = studentsCategoryRepo.EditStudentsCategory(id);
                    return new JsonResult()
                    {
                        Data = editStudentsCategory,
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


        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allStudentsCategory = studentsCategoryRepo.GetAllStudentsCategory();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allStudentsCategory != null)
            {
                return new JsonResult()
                {
                    Data = allStudentsCategory.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = allStudentsCategory,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }
    }
}