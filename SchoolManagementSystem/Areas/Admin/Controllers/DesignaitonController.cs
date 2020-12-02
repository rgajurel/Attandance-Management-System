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
    public class DesignaitonController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDesignationRepository designationRepo;
        private readonly IDropDownRepository dropDownRepo;
        private string message = "";
        public DesignaitonController(IMessageHandlerRepository messageHandlerRepo, IDropDownRepository dropDownRepo, IDesignationRepository designationRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.designationRepo = designationRepo;
        }
        // GET: Admin/Designaiton

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult LoadPartialView()
        {
           
            return PartialView("View");
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

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveDesignation(Designations desig)
        {
            try
            {
                if (desig != null)
                {
                    desig.Designation = desig.Designation.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (desig.ID > 0)
                        {
                            var savechange = designationRepo.AddUpdateDesignation(desig);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = designationRepo.AddUpdateDesignation(desig);
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
        public JsonResult DeleteDesignation(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = designationRepo.DeleteDesignaiton(id);
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
        public JsonResult EditDesignation(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editDesignation = designationRepo.EditDesignation(id);
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


                var allDesignation = designationRepo.GetAllDesignation();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allDesignation != null)
                {
                    return new JsonResult()
                    {
                        Data = allDesignation.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allDesignation,
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