using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
   
    public class EmployerController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo; 
        private readonly IEmployerRepository employerRepo;     
        private string message = "";
        public EmployerController(IDropDownRepository dropDownRepo, IEmployerRepository employerRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.employerRepo = employerRepo;
        }

        // GET: Admin/Employer
      // [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            //ViewBag.error = "Employer";
            LoadDropDown();
            return View();
        }

      //  [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult LoadPartialView()
        {

            LoadDropDown();
            return PartialView("View");
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, EmployeeSearch search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var employeeList =employerRepo.GetAllEmployee(search);

            if (employeeList != null)
            {
                var result = new DataSourceResult()
                {
                    Data = employeeList,
                    Total = employeeList.Select(model => model.Total).FirstOrDefault()
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var result = new DataSourceResult()
                {
                    Data = employeeList,
                    Total = 0
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }







        }
             
        public JsonResult CheckExistingUserID(string UserID,int ID)
      {
            if (ID == 0)
            {
                var userdata = employerRepo.GetEmployeeByUserID(UserID);
                if (userdata != null)
                {
                    return Json(!userdata.UserID.Equals(Convert.ToInt16(UserID)), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var userdata = employerRepo.GetEmployeeByUserID(UserID);
                if (userdata != null)
                {
                    if (userdata.ID == ID)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            }
            
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveEmployer(Employee employee)
        {
            ModelState["NepaliJoioningDate"].Errors.Clear();
            ModelState["NepaliDateOfBirth"].Errors.Clear();
            try
            {                
                if (employee != null)
                {
                    if (ModelState.IsValid)
                    {
                       if (employee.ID > 0)
                        {
                            employee.Image = UploadImageUpdate(employee.imageFile);
                            var savechange = employerRepo.AddUpdateEmployee(employee);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            employee.Image = UploadImageSave(employee.imageFile);
                            var savechange = employerRepo.AddUpdateEmployee(employee);
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

        private string UploadImageSave(HttpPostedFileBase image)
        {
            var SchoolPicImage = "";

            if (image != null)
            {
                string schoolPicExtension = Path.GetExtension(image.FileName);
                //var ImageName = Guid.NewGuid();
                image.SaveAs(HttpContext.Server.MapPath("~/Content/Images/Teachers/" + image.FileName));

                SchoolPicImage = "/Content/Images/Teachers/" + image.FileName;
            }
            else
            {
                SchoolPicImage = DefaultImages.studentImage;
            }

            return SchoolPicImage;
        }

        private string UploadImageUpdate(HttpPostedFileBase image)
        {
            var SchoolPicImage = String.Empty;

            if (image != null)
            {
                string schoolPicExtension = Path.GetExtension(image.FileName);
                //var ImageName = Guid.NewGuid();
                image.SaveAs(HttpContext.Server.MapPath("~/Content/Images/Teachers/" + image.FileName));
                SchoolPicImage = "/Content/Images/Teachers/" + image.FileName;
            }


            return SchoolPicImage;
        }

        [HttpPost]
        public JsonResult GetDepartmentBasedOnOrganisation(string ID)
        {
            try
            {
                if (ID != null)
                {
                    var facultys = employerRepo.GetDepartmentBasedOnOrganisation(ID);
                    return new JsonResult()
                    {
                        Data = facultys,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
                }


            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }
       [HttpPost]
        public JsonResult GetDesignationBasedOnOrganisation(string ID)
        {
            try
            {
                if (ID != null)
                {
                    var facultys = employerRepo.GetDesignationBasedOnOrganisation(ID);
                    return new JsonResult()
                    {
                        Data = facultys,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
                }


            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        } 
        public void LoadDropDown()
        {
           
            var allOrganisation = dropDownRepo.GetAllOrganisation();
            if (allOrganisation != null)
            {
                ViewBag.allOrganisation = new SelectList(allOrganisation, "ID", "Name");
            }
            else
            {
                ViewBag.allOrganisation = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var jobTypeList = dropDownRepo.GetJobTypeDropDown();
            if (jobTypeList != null)
            {
                ViewBag.jobType = new SelectList(jobTypeList, "ID", "Name");
            }
            else
            {
                ViewBag.jobType = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

        }

        [HttpPost]
        public JsonResult EditEmployer(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editEmployer = employerRepo.EditEmployee(id);
                    return new JsonResult()
                    {
                        Data = editEmployer,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
                }


            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
               // return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
        }

        [HttpPost]
        public JsonResult DetailsEmployer(int id)
        {
            try
            {
                if (id != 0)
                {
                    var detailsStudents = employerRepo.DetailsEmployer(id);
                    return new JsonResult()
                    {
                        Data = detailsStudents,
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
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetUniqueDeivceUserID(string UserID)
        {
            try
            {
                if (UserID != null)
                {
                    var uniqueregistration = employerRepo.GetUniqueDeivceID();
                    return new JsonResult()
                    {
                        Data = uniqueregistration,
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

    }
}