using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
   
    public class OrganisationInformationController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly ISchoolInformationRepository schoolInformationRepo;
        private string message = "";


        public OrganisationInformationController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, ISchoolInformationRepository schoolInformationRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.schoolInformationRepo = schoolInformationRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        // GET: Admin/SchoolInformation
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }
       // [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public PartialViewResult LoadPartialView()
        {
            LoadDropDown();
            return PartialView("View");
        }

       

        public void LoadDropDown()
        {
            var schoolTypeList = dropDownRepo.GetSchoolTypeDropDown();
            if (schoolTypeList != null)
            {
                ViewBag.schoolTypeList = new SelectList(schoolTypeList, "ID", "Name");
            }
            else
            {
                ViewBag.schoolTypeList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }
        private string UploadImageSave(HttpPostedFileBase image)
        {
            var SchoolPicImage = "";

            if (image != null)
            {      
                string schoolPicExtension = Path.GetExtension(image.FileName);
                //var ImageName = Guid.NewGuid();
                image.SaveAs(HttpContext.Server.MapPath("~/Content/Images/School/" +image.FileName));

               SchoolPicImage = "/Content/Images/School/" +image.FileName;
            }
            else
            {
                SchoolPicImage = DefaultImages.schoolImage;
            }

            return SchoolPicImage;
        }

        private string UploadImageUpdate(HttpPostedFileBase image)
        {
            var SchoolPicImage =String.Empty;

            if (image != null)
            {
                string schoolPicExtension = Path.GetExtension(image.FileName);
               
                image.SaveAs(HttpContext.Server.MapPath("~/Content/Images/School/" + image.FileName));

                SchoolPicImage = "/Content/Images/School/" + image.FileName;
            }
           

            return SchoolPicImage;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSchoolInformation(SchoolInformation schoolInformation)
        {            
            try
            {
                if (schoolInformation != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (schoolInformation.ID > 0)
                        {
                            schoolInformation.Image = UploadImageUpdate(schoolInformation.imageFile);
                            var savechange = schoolInformationRepo.AddUpdateSchoolInformation(schoolInformation);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            schoolInformation.Image = UploadImageSave(schoolInformation.imageFile);
                            var savechange = schoolInformationRepo.AddUpdateSchoolInformation(schoolInformation);
                            message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
                            //add
                        }

                    }
                    else
                    {
                        message = MassageDescription.ModelErrorOccured;
                        
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
        public JsonResult CheckIfMainBranchExist(string IsMainBranch)
        {
            try
            {
                if (!String.IsNullOrEmpty(IsMainBranch) && IsMainBranch == "1")
                {

                    var mainbranchcount = schoolInformationRepo.GetAllSchoolInformation().Where(model => model.IsMainBranch == Convert.ToInt16(IsMainBranch)).Count();
                    if (mainbranchcount != 0)
                    {
                        return Json(messageHandlerRepo.GetMessage("Organisation Cannot contain Multiple Main Branch"));
                    }
                    else
                    {
                        return Json(null);
                    }
                }
                else
                {
                    return Json(null);
                }
                             


            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allSchoolInformation = schoolInformationRepo.GetAllSchoolInformation();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allSchoolInformation != null)
            {

                return new JsonResult()
                {
                    Data = allSchoolInformation.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allSchoolInformation,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
           



        }

        [HttpPost]
        public JsonResult DeleteSchoolInformation(int id)
        {
            
            try
            {
                if (id != 0)
                {
                    var savechanges = schoolInformationRepo.DeleteSchoolInformation(id);
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
        public JsonResult EditSchoolInformation(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSchooInformation = schoolInformationRepo.EditSchoolInformation(id);
                    return new JsonResult()
                    {
                        Data = editSchooInformation,
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