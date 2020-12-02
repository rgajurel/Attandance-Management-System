using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechtonneMS;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class OrganisationTypeController : Controller
    {
        private readonly IShoolTypeRepository schoolTypeRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
       private bool success = false;


        public OrganisationTypeController(IShoolTypeRepository schoolTypeRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.schoolTypeRepo = schoolTypeRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        // GET: Admin/SchoolType
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
        public JsonResult SaveSchoolType(SchoolType schoolType)
        {
            
            try
            {
                if (schoolType != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (schoolType.ID > 0)
                        {
                            var savechange = schoolTypeRepo.AddUpdateSchoolType(schoolType);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            success = true;
                                                 
                            //update
                        }
                        else
                        {
                            var savechange = schoolTypeRepo.AddUpdateSchoolType(schoolType);
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
            catch(Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }
          

        }
        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            
          var allSchoolType = schoolTypeRepo.GetAllSchoolType();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
            if (allSchoolType != null)
            {
                return new JsonResult()
                {
                    Data = allSchoolType.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else 
            {
                return new JsonResult()
                {
                    Data = allSchoolType,  
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }    
               
           
            
              
        }

        [HttpPost]
        public JsonResult EditSchoolType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSchoolType = schoolTypeRepo.EditSchoolType(id);
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
        public JsonResult DeleteSchoolType(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = schoolTypeRepo.DeleteSchoolType(id);
                    message= (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.CannotDeleteDependency;
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