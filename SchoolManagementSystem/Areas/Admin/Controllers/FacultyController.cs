using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IFacultyRepository facultRepo;
        private string message="";
        // GET: Admin/Faculty
        public FacultyController(IMessageHandlerRepository messageHandlerRepo, IFacultyRepository facultRepo)
        {
            this.facultRepo = facultRepo;
            this.messageHandlerRepo = messageHandlerRepo;
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult LoadPartialView()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            //foreach(var claim in claims)
            //{
            //    identity.RemoveClaim(claim);

            //}         

            //string sHostName = Dns.GetHostName();
            //IPHostEntry ipE = Dns.GetHostByName(sHostName);
            //IPAddress[] IpA = ipE.AddressList;
            //for (int i = 0; i < IpA.Length; i++)
            //{
            //    Console.WriteLine("IP Address {0}: {1} ", i, IpA[i].ToString());
            //}
            return PartialView("View");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveFaculty(Facultys facult)
        {
            try
            {
                if (facult != null)
                {
                    facult.Faculty = facult.Faculty.ToUpper();
                    if (ModelState.IsValid)
                    {
                        if (facult.ID > 0)
                        {
                            var savechange = facultRepo.AddUpdateFaculty(facult);
                            message = (savechange == true) ? MassageDescription.UpdateSuccess : MassageDescription.UpdateFailure;
                            //update
                        }
                        else
                        {
                            var savechange = facultRepo.AddUpdateFaculty(facult);
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
        public JsonResult DeleteFaculty(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = facultRepo.Deleteaculty(id);
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
        public JsonResult EditFaculty(int id)
        {
            try
            {
                if (id != 0)
                {
                    var editFaculty = facultRepo.EditFaculty(id);
                    return new JsonResult()
                    {
                        Data = editFaculty,
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


                var allFaculty = facultRepo.GetAllFaculty();// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allFaculty != null)
                {
                    return new JsonResult()
                    {
                        Data = allFaculty.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allFaculty,
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