
using DomainEntities;
using DomainInterface;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class FeeCollectionController : Controller
    {
        private readonly IFeeCollectionRepository feeCollectionRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        private string message = "";
        public FeeCollectionController(IFeeCollectionRepository feeCollectionRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.feeCollectionRepo = feeCollectionRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.dropDownRepo = dropDownRepo;
        }
        // GET: Admin/FeeCollection
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
           
        }

        public JsonResult GetMonth(string studentId)
        {
            try
            {
                if (studentId != null)
                {
                    var months = feeCollectionRepo.GetAllMonthList(studentId);
                    return new JsonResult()
                    {
                        Data = months,
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



        public JsonResult GetAllStudents([DataSourceRequest] DataSourceRequest request, FeeCollection search)
        {
            search.offset = request.PageSize * (request.Page - 1);
            var studentsList = feeCollectionRepo.GetAllStudentsList(search);

            if (studentsList != null)
            {
                var result = new DataSourceResult()
                {
                    Data = studentsList,
                    Total = studentsList.Select(model => model.Total).FirstOrDefault()
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new DataSourceResult()
                {
                    Data = studentsList,
                    Total = 0
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }

        public JsonResult GetAllFeeList([DataSourceRequest] DataSourceRequest request, FeeCollection search)
        {

            var feeList = feeCollectionRepo.GetAllFeeList(search);

            try
            {
                if (feeList != null && feeList.Count() > 0)
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
            catch {
                return new JsonResult()
                {
                    Data = feeList,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }

        }



        public decimal GetPreviousDue(string StudentId,string SessionId, string FacultyId,  string ClassId, string Section)
        {
            
            decimal dueAmount = feeCollectionRepo.CalculatePreviousDue(StudentId, SessionId, FacultyId, ClassId, Section);

            return dueAmount;

        }


        [HttpPost]
        public JsonResult SaveFeeCollection(string data1, string stuId, string session, string faculty, string classs, string section, string previousDue, string totalDiscount,string totalFee, string grandTotal, string balance, string totalPaid)
        {
            try
            {
                List<FeeCollection> ListWithError = new List<FeeCollection>();

                var feeCollections = JsonConvert.DeserializeObject<List<FeeCollection>>(data1);

                if (feeCollections != null)
                {
                    foreach (var fee in feeCollections)
                    {
                        if (fee.GrandTotal < fee.TotalPaid)
                        {
                            ListWithError.Add(fee);
                        }
                    }
                    if (ListWithError.Count() > 0)
                    {
                        return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(true, MassageDescription.ExceptionOrNullError, ListWithError.OfType<dynamic>().ToList()), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        FeeCollection cf = new FeeCollection();
                        cf.StudentId = Convert.ToInt32(stuId);
                        cf.Session = session;
                        cf.FacultyID = faculty;
                        cf.ClassID = classs;
                        cf.Section = section;
                        cf.PreviousDue = Convert.ToDecimal(previousDue);
                        cf.TotalDiscount = Convert.ToDecimal(totalDiscount);
                        cf.GrandTotal = Convert.ToDecimal(grandTotal);
                        cf.Balance = Convert.ToDecimal(balance);
                        cf.TotalPaid = Convert.ToDecimal(totalPaid);
                        cf.TotalFee = Convert.ToDecimal(totalFee);
                        string saveChanges = feeCollectionRepo.AddFeeCollection(feeCollections.ToList(), stuId, session, faculty, classs, section, previousDue,totalDiscount,totalFee, grandTotal,balance,totalPaid);

                        message = (saveChanges != "Failure") ? saveChanges : MassageDescription.SaveFailure;

                    }

                }

                return Json(message);

            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
            }
        }


        //public ActionResult PrintBill(int id)
        //{
        //    try
        //    {
        //        List<FeeCollectionReport> li = new List<FeeCollectionReport>();
        //        li = feeCollectionRepo.FeeCollectionBill(id.ToString());
        //        ReportDocument rd = new ReportDocument();
        //        rd.Load(Path.Combine(Server.MapPath("~/Areas/Admin/Reports"), "rptPaymentBill.rpt"));
        //        rd.SetDataSource(li);
        //        rd.SetParameterValue("Logo", Server.MapPath("~" + li[0].SchoolImage.ToString()));
        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        str.Seek(0, SeekOrigin.Begin);
        //        return File(str, "application/pdf", "PaymentBill_" + id + ".pdf");

        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = ex.Message;
        //        return null;
        //    }
        //}


        //public ActionResult PrintDueBill(int id)
        //{
        //    try
        //    {
        //        List<FeeDueReport> li = new List<FeeDueReport>();
        //        li = feeCollectionRepo.FeeDueBill(id.ToString());
        //       // ReportDocument rd = new ReportDocument();
        //        rd.Load(Path.Combine(Server.MapPath("~/Areas/Admin/Reports"), "rptPaymentDueBill.rpt"));
        //        rd.SetDataSource(li);
        //        rd.SetParameterValue("Logo", Server.MapPath("~" + li[0].SchoolImage.ToString()));
        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        str.Seek(0, SeekOrigin.Begin);
        //        return File(str, "application/pdf", "DueBill_" + id + ".pdf");

        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = ex.Message;
        //        return null;
        //    }
        //}



        [HttpPost]
        public JsonResult SaveDueBill(string data1, string stuId, string session, string faculty, string classs, string section, string previousDue, string totalDiscount, string totalFee, string grandTotal, string balance)
        {
            try
            {
               

                var feeCollections = JsonConvert.DeserializeObject<List<FeeCollection>>(data1);

                if (feeCollections != null)
                {
                    
                        FeeCollection cf = new FeeCollection();
                        cf.StudentId = Convert.ToInt32(stuId);
                        cf.Session = session;
                        cf.FacultyID = faculty;
                        cf.ClassID = classs;
                        cf.Section = section;
                        cf.PreviousDue = Convert.ToDecimal(previousDue);
                        cf.TotalDiscount = Convert.ToDecimal(totalDiscount);
                        cf.GrandTotal = Convert.ToDecimal(grandTotal);
                        cf.Balance = Convert.ToDecimal(balance);
                        cf.TotalFee = Convert.ToDecimal(totalFee);
                        string saveChanges = feeCollectionRepo.AddDueBill(feeCollections.ToList(), stuId, session, faculty, classs, section, previousDue, totalDiscount, totalFee, grandTotal);

                        message = (saveChanges != "Failure") ? saveChanges : MassageDescription.SaveFailure;


                }

                return Json(message);

            }
            catch (Exception ex)
            {
                return Json(messageHandlerRepo.GetErrorMessageWithListAlongWithErrorList(false, MassageDescription.ExceptionOrNullError, null), JsonRequestBehavior.AllowGet);
            }
        }
    }
}