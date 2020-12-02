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
  // [RequireHttps]
     public class ClassController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IClassRepository classRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = String.Empty;
        // GET: Admin/Class
        public ClassController(IDropDownRepository dropDownRepo, IClassRepository classRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.classRepo = classRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
        }
        // [ApplicationAuthorizeAttribute]

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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            try {
                var allClassInfo = classRepo.GetAllClass();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

                if (allClassInfo != null)
                {
                    return new JsonResult()
                    {
                        Data = allClassInfo.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };

                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allClassInfo,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveClass(Class classs)
        {
           
            try
            {
                
                classs.Sections = string.Join(",", classs.SectionArray.ToArray());
                if (classs != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (classs.ID > 0)
                        {
                            var savechange = classRepo.AddUpdateClass(classs);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = classRepo.AddUpdateClass(classs);
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
        //[ApplicationAuthorizeAttribute]
        //[ApplicationAuthorizeAttribute]
        public JsonResult EditClass(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editClass = classRepo.EditClass(id);
                    return new JsonResult()
                    {
                        Data = editClass,
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
        public JsonResult DeleteClass(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = classRepo.DeleteClass(id);
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

        public void LoadDropDown()
        {
            var facultyList = dropDownRepo.GetFacultyDropDown();
            if (facultyList != null)
            {
                ViewBag.facultyList = new SelectList(facultyList, "ID", "Name");
            }
            else
            {
                ViewBag.facultyList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var classTypeList = dropDownRepo.GetClassTypeDropDown();
            if (classTypeList != null)
            {
                ViewBag.classTypeList = new SelectList(classTypeList, "ID", "Name");
            }
            else
            {
                ViewBag.classTypeList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
           

            var sectionList = dropDownRepo.GetSectionDropDown();
            if (sectionList != null)
            {
                ViewBag.sectionList = new SelectList(sectionList, "Name", "Name");
            }
            else
            {
                ViewBag.sectionList = new SelectList(dropDownRepo.GetErrorList(), "Name", "Name");
            }

            var classList = dropDownRepo.GetClasswDropDown();
            if (classList != null)
            {
                ViewBag.classList = new SelectList(classList, "ID", "Name");
            }
            else
            {
                ViewBag.classList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }
    }
}