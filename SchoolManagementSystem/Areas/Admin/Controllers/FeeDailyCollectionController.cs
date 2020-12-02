using DomainInterface;
using System;
using System.Web.Mvc;
using System.Data;
using System.IO;

using DomainEntities;
using ClosedXML.Excel;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class FeeDailyCollectionController : Controller
    {

        private readonly IFeeDailyCollectionRepository feeDailyCollectionRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IDropDownRepository dropDownRepo;
        public FeeDailyCollectionController(IFeeDailyCollectionRepository feeDailyCollectionRepo, IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo)
        {
            this.feeDailyCollectionRepo = feeDailyCollectionRepo;
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
            var sessionList = dropDownRepo.GetSessionDropDown();
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


        public ActionResult ExportData(FeeDailyCollection fee)
        {
            MemoryStream stream = new MemoryStream();
            XLWorkbook wb = new XLWorkbook();
            DataSet ds = new DataSet();
            ds = feeDailyCollectionRepo.getAllData(fee);
            if (ds == null)
            {
                TempData["success"]= "Warning!!! Data not found.";
                return RedirectToAction("Index");
            }
            try
            {
                
                ds.Tables[0].Rows.Add();
                int rowsCount = ds.Tables[0].Rows.Count;
                int colCount = ds.Tables[0].Columns.Count;
                
                try
                {
                    for (int i = 5; i < colCount; i++)
                    {
                        if (ds.Tables[0].Columns[i].ColumnName != "->Total Remaining Due" && ds.Tables[0].Columns[i].ColumnName != "->Total Previous Due")
                        {
                            decimal total = 0;
                            for (int j = 0; j <= rowsCount; j++)
                            {
                                try
                                {
                                    total += Convert.ToDecimal(ds.Tables[0].Rows[j][i].ToString());
                                }
                                catch
                                {
                                    total += 0;
                                }
                            }
                            ds.Tables[0].Rows[rowsCount - 1][i] = total;
                        }
                    }
                    
                }

                catch (Exception ex)
                {

                }
            }
            catch
            {

            }
            string fileName = "DailyCollection" + DateTime.Now.ToShortDateString() + ".xlsx";
            wb.Worksheets.Add(ds.Tables[0]);
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}