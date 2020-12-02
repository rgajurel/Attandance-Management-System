using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class PersonalFeeController : Controller
    {
        private readonly IPersonalFeeRepository personalFeeRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private string message = "";
        public PersonalFeeController(IPersonalFeeRepository personalFeeRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.personalFeeRepo = personalFeeRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
        }
        // GET: Admin/CommonFeeDiscount
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
        public JsonResult GetClassBasedOnFaculty(string faculty)
        {
            try
            {
                if (faculty != null)
                {
                    var Classes = personalFeeRepo.GetClassBasedOnFaculty(faculty);
                    return new JsonResult()
                    {
                        Data = Classes,
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
        public JsonResult GetSectionBaseOnClass(string ID, string faculty)
        {
            try
            {
                if (ID != null && faculty != null)
                {
                    var sections = personalFeeRepo.GetSectionBasedOnClass(ID, faculty);
                    return new JsonResult()
                    {
                        Data = sections,
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



            var sessionList = dropDownRepo.GetActiveSessionDropDown();
            if (sessionList != null)
            {
                ViewBag.sessionList = new SelectList(sessionList, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
            var facultyList = dropDownRepo.GetFacultyDropDown();
            if (facultyList != null)
            {
                ViewBag.facultyList = new SelectList(facultyList, "ID", "Name");
            }
            else
            {
                ViewBag.facultyList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

           
            var typeList = dropDownRepo.GetPersonnelTypeDropDown();
            if (typeList != null)
            {
                ViewBag.typeList = new SelectList(typeList, "ID", "Name");
            }
            else
            {
                ViewBag.typeList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
            var monthList = dropDownRepo.GetMonthDropDown();
            if (monthList != null)
            {
                ViewBag.month = new SelectList(monthList, "ID", "Name");
            }
            else
            {
                ViewBag.month = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, PersonalFee search)
        {

            var feeList = personalFeeRepo.GetAllPersonalFee(search);

            if (feeList != null || feeList.Count() > 0)
            {
                return new JsonResult()
                {
                    Data = feeList.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = feeList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }

        }

        [HttpPost]
        public JsonResult savePersonalFee(string data1, string session, string classs, string faculty, string section, string type, string month)
        {
            try
            {
                List<PersonalFee> ListWithError = new List<PersonalFee>();
                var fees = JsonConvert.DeserializeObject<List<PersonalFee>>(data1);


                if (fees != null)
                {
                    foreach (var singleFee in fees)
                    {
                        if (singleFee.Fee < singleFee.Discount)
                        {
                            singleFee.Discount = 0;
                            ListWithError.Add(singleFee);
                        }
                    }
                    if (ListWithError.Count() > 0)
                    {
                        return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.ExceptionOrNullError, ListWithError.OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        PersonalFee pf = new PersonalFee();
                        pf.Session = session;
                        pf.Class = classs;
                        pf.Faculty = faculty;
                        pf.Section = section;
                        pf.Type = type;
                        pf.Month = month;
                        string saveChanges = personalFeeRepo.AddUpdatePersonalFee(fees.ToList(), faculty, session, classs, section, type, month);
                        message = (saveChanges == "Success") ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;

                    }

                }
                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeletePersonalFee(int id)
        {
            try
            {
                if (id != 0)
                {
                    var savechanges = personalFeeRepo.DeletePersonalFee(id);
                    message = (savechanges == true) ? MassageDescription.DeleteSuccess : MassageDescription.Deleteailure;
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