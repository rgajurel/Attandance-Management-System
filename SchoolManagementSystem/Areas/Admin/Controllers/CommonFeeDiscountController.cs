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
    public class CommonFeeDiscountController : Controller
    {
        private readonly ICommonFeeDiscountRepository commonFeeDiscountRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private string message = "";
        public CommonFeeDiscountController(ICommonFeeDiscountRepository commonFeeDiscountRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.commonFeeDiscountRepo = commonFeeDiscountRepo;
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
            
        }

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, CommonFeeDiscount search)
        {

            var discountList = commonFeeDiscountRepo.GetAllCommonFeeDiscount(search);

            if (discountList != null || discountList.Count() > 0)
            {
                return new JsonResult()
                {
                    Data = discountList.ToDataSourceResult(request),
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = discountList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                };
            }

        }

        [HttpPost]
        public JsonResult GetClassBasedOnFaculty(string faculty)
        {
            try
            {
                if (faculty != null)
                {
                    var Classes = commonFeeDiscountRepo.GetClassBasedOnFaculty(faculty);
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
        public JsonResult GetSectionBaseOnClass(string Id, string Faculty)
        {
            try
            {
                if (Id != null && Faculty != null)
                {
                    var sections = commonFeeDiscountRepo.GetSectionBasedOnClass(Id, Faculty);
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


        [HttpPost]
        public JsonResult GetFeeType(string Faculty, string session, string Class, string Section)
        {
            try
            {
                if (Faculty != null && Class != null && Section != null)
                {
                    var type = commonFeeDiscountRepo.GetFeeTypeBasedOnSection(Faculty, session, Class, Section);
                    return new JsonResult()
                    {
                        Data = type,
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
        public JsonResult GetMonthBasedOnFeeType(string Faculty, string session, string Class, string Section, string Type)
        {
            try
            {
                if (Faculty != null && Class != null && Section != null && Type != null)
                {
                    var type = commonFeeDiscountRepo.GetMonthBasedOnFeeType(Faculty, session, Class, Section, Type);
                    return new JsonResult()
                    {
                        Data = type,
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
        public JsonResult SaveCommonFeeDiscount(string data1, string session, string classs, string faculty, string section, string type, string month)
        {
            try
            {
                List<CommonFeeDiscount> ListWithError = new List<CommonFeeDiscount>();
                var discounts = JsonConvert.DeserializeObject<List<CommonFeeDiscount>>(data1);

                if (discounts != null)
                {
                    foreach (var discount in discounts)
                    {
                        if (discount.Fee < discount.Discount)
                        {
                            discount.Discount = 0;
                            ListWithError.Add(discount);
                        }
                    }
                    if (ListWithError.Count() > 0)
                    {
                        return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.ExceptionOrNullError, ListWithError.OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        CommonFeeDiscount cf = new CommonFeeDiscount();
                        cf.Session = session;
                        cf.Class = classs;
                        cf.Faculty = faculty;
                        cf.Section = section;
                        cf.Type = type;
                        cf.Month = month;
                        string saveChanges = commonFeeDiscountRepo.AddUpdateCommonFeeDiscount(discounts.ToList(), faculty, session, classs, section, type, month);

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

    }
}