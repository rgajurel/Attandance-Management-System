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
    public class SalaryHeadingSettingsController : Controller
    {
        private readonly ISalaryHeadSettingsRepository salaryHeadSetRepo;
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private string message = String.Empty;
        // GET: Admin/SalaryHeadingSettings

        public SalaryHeadingSettingsController(ISalaryHeadSettingsRepository salaryHeadSetRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.salaryHeadSetRepo = salaryHeadSetRepo;
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
            return View("View");
        }

        public void LoadDropDown()
        {
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

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, SalaryHeadingSettings search)
        {
            try
            {
                var allSalarySetHeadings = salaryHeadSetRepo.GetAllSalaryHeadingSettings(search);// iDepartRepo.GetAllDepartment().OrderByDescending(model => model.DepartmentID);
                if (allSalarySetHeadings != null)
                {
                    return new JsonResult()
                    {
                        Data = allSalarySetHeadings.ToDataSourceResult(request),
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
                else
                {
                    return new JsonResult()
                    {
                        Data = allSalarySetHeadings,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    };
                }
            }
            catch (Exception)
            {
                return null;
            }



        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveSalaryHeadingSettings(string data)
        {
            try
            {               
                var results = JsonConvert.DeserializeObject<List<SalaryHeadingSettings>>(data);
                //  if (results.Any(model => String.IsNullOrEmpty(model.HeadName)))
                // {
                // results.Remove(results.Any(model => String.IsNullOrEmpty(model.HeadName)));
                //  }
                results.RemoveAll(model => String.IsNullOrEmpty(model.HeadName));
                //else
                //{
                    var datarequiredfordelete = results.FirstOrDefault();
                    salaryHeadSetRepo.DeleteData(datarequiredfordelete);
                    int markscount = salaryHeadSetRepo.SalaryHeadingsSettingsBatchUpload(results);
                    if (markscount > 0)
                    {
                        message = (markscount > 0) ? MassageDescription.SaveSuccess : MassageDescription.SaveFailure;                      
                    }
                    else
                    {
                        message = MassageDescription.ExceptionOrNullError;                       
                    }

               // }

                return Json(messageHandlerRepo.GetMessage(message));

            }
            catch (Exception)
            {
                return Json(messageHandlerRepo.GetMessage(MassageDescription.ExceptionOrNullError));
            }
            
        }

    }
}