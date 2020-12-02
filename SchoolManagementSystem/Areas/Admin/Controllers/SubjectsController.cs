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
    public class SubjectsController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly ISubjectsRepository subjectRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = "";
        // GET: Admin/Subjects

        public SubjectsController(ISubjectsRepository subjectRepo, IMessageHandlerRepository messageHandlerRepo, IDropDownRepository dropDownRepo)
        {
            this.subjectRepo = subjectRepo;
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSubjects(List<Subjects> SubjectList)
        {
            try
            {
                if (!SubjectList.Any(c => c.ClassID == 0))
                {
                    if (SubjectList.Count() == 1)
                    {
                        var savechange = subjectRepo.AddUpdateSubject(SubjectList.FirstOrDefault());
                         message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.UpdateFailure;
                    }
                    else
                    {
                        int subjectcount = subjectRepo.SubjectBatchUpload(SubjectList);
                        if (subjectcount > 0)
                        {
                            message = MassageDescription.SaveSuccess;
                        }
                        else
                        {
                            message = MassageDescription.SaveFailure;
                        }
                    }
                }
                else
                {
                    message = MassageDescription.SaveFailure;
                    return Json(messageHandlerRepo.GetMessage(message));
                }
              
                return Json(messageHandlerRepo.GetMessage(message));
            }
            catch(Exception)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));
            }


          
            //try
            //{
            //    if (SubjectList != null)
            //    {
            //        if (ModelState.IsValid)
            //        {
            //            foreach (var subject in SubjectList)
            //            {
            //                if (subject.ClassID==0)
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    if (subject.ID > 0)
            //                    {
            //                        var savechange = subjectRepo.AddUpdateSubject(subject);
            //                        message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
            //                        //update
            //                    }
            //                    else
            //                    {
            //                        var savechange = subjectRepo.AddUpdateSubject(subject);
            //                        message = (savechange == true) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;
            //                        //add
            //                    }
            //                }
            //            }

            //        }
            //        else
            //        {
            //            message = MassageDescription.ModelErrorOccured;
            //            // return Json(messageHandlerRepo.GetMessage(message));
            //            //model error occured
            //        }
            //    }
            //    else
            //    {
            //        message = MassageDescription.ExceptionOrNullError;
            //        //null error occured
            //    }
            //    return Json(messageHandlerRepo.GetMessage(message));
            //}
            //catch (Exception ex)
            //{
            //    message = MassageDescription.ExceptionOrNullError;
            //    return Json(messageHandlerRepo.GetMessage(message));

            //}


        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {

            var allSubjects = subjectRepo.GetAllSubjects();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);

            if (allSubjects != null)
            {
                return new JsonResult()
                {
                    Data = allSubjects.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };

            }
            else
            {
                return new JsonResult()
                {
                    Data = allSubjects,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }




        }
        public void LoadDropDown()
        {
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

        [HttpPost]
        public JsonResult EditSubject(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editSubjects = subjectRepo.EditSubjects(id);
                    return new JsonResult()
                    {
                        Data = editSubjects,
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

        public JsonResult DeleteSubjects(int  id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = subjectRepo.DeleteSubjects(id);
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
    }
     }