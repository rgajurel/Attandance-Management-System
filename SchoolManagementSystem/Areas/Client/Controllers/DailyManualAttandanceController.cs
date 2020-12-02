using DomainEntities;
using DomainInterface;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class DailyManualAttandanceController : Controller
    {
        private readonly IDropDownRepository dropDownRepo;
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IEmployerRepository employeeRepo;
        private readonly IManualAttandanceRepository manualAttandanceRepo;
        private string message = "";
        // GET: Client/DailyManualAttandance
        public DailyManualAttandanceController(IDropDownRepository dropDownRepo, IManualAttandanceRepository manualAttandanceRepo, IMessageHandlerRepository messageHandlerRepo, IEmployerRepository employeeRepo)
        {
            this.dropDownRepo = dropDownRepo;
            this.messageHandlerRepo = messageHandlerRepo;
            this.employeeRepo = employeeRepo;
            this.manualAttandanceRepo = manualAttandanceRepo;
        }
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
            var allActiveSession = dropDownRepo.GetActiveSessionDropDown();
            if (allActiveSession != null)
            {
                ViewBag.sessionList = new SelectList(allActiveSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }

            var allSession = dropDownRepo.GetSessionDropDown();
            if (allSession != null)
            {
                ViewBag.sessionListAll = new SelectList(allSession, "ID", "Name");
            }
            else
            {
                ViewBag.sessionListAll = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }
            var allMonth = dropDownRepo.GetAllMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }


        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request, Attandance search)
        {

            search.offset = request.PageSize * (request.Page - 1);
            var employeeList = manualAttandanceRepo.GetAllDailyManualAttandance(search);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveDailyManualAttandance(Attandance attandance)
        {
            try
            {
                if (attandance != null)
                {
                    attandance.AddedBy = new LoginUser().UserName;
                    attandance.UpdatedBy = new LoginUser().UserName;
                    attandance.AttandanceType = "ManualLoginWeb";
                    attandance.EntryLocation = "LoginWeb";
                    var loginuser = employeeRepo.EditEmployee(new LoginUser().LoggedInEmployeeID);
                    var setting = manualAttandanceRepo.GetSettingsTime();
                    bool offflag = false;
                    bool onflag = true;
                    var serverTime = DateTime.Now;
                    DateTime timeofday = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Nepal Standard Time");

                    var minute = (timeofday.TimeOfDay.Subtract(loginuser.EntryTime)).TotalMinutes;
                    var minut = (timeofday.TimeOfDay.Subtract(loginuser.ExitTime)).TotalMinutes;
                   if (minute <= Convert.ToInt16(setting.ValidTimeAfterEntry) && minute >= -(Convert.ToInt16(setting.ValidTimeBeforeEntry)) || (minut >= -Convert.ToInt16(setting.ValidTimeBeforeLeave) && minut <= Convert.ToInt16(setting.ValidTimeAfterLeave)))
                   {
                        var Nepalidatetime = GetNepaliDateTime();
                        TimeSpan sp = Nepalidatetime.TimeOfDay;
                        TimeSpan ss = new TimeSpan(00, 00, 00);
                        TimeSpan time1 = loginuser.EntryTime;
                        TimeSpan time2 = (sp.Subtract(time1));
                        double totalminutes = time2.TotalMinutes;
                        var attandanceforthatday = manualAttandanceRepo.GetAttandanceOfDay(timeofday, new LoginUser().LoggedInEmployeeID, offflag).Count();
                        if (attandanceforthatday > 0)
                        {
                            message = "Success ! Attandance For That Day is Already Done";
                        }
                        else
                        {
                            var count = manualAttandanceRepo.GetAttandanceOfDay(timeofday, new LoginUser().LoggedInEmployeeID, onflag).Count();

                            if (count <= 0)
                            {
                                if (totalminutes <= Convert.ToInt16(setting.ValidTimeAfterEntry) && totalminutes >= -(Convert.ToInt16(setting.ValidTimeBeforeEntry)))
                                {
                                    attandance.IsDailyAttandance = true;
                                    attandance.EmployeeID = new LoginUser().LoggedInEmployeeID;
                                    attandance.Status = "4";
                                    attandance.IsKaaj = false;
                                    attandance.IsManualAttandance = true;
                                    attandance.EntryTime = Convert.ToString(Nepalidatetime.TimeOfDay);
                                    attandance.ExitTime = Convert.ToString(ss);
                                }
                                else
                                {
                                    attandance.IsDailyAttandance = true;
                                    attandance.EmployeeID = new LoginUser().LoggedInEmployeeID;
                                    attandance.Status = "5";
                                    attandance.IsKaaj = false;
                                    attandance.IsManualAttandance = true;
                                    attandance.EntryTime = Convert.ToString(Nepalidatetime.TimeOfDay);
                                    attandance.ExitTime = Convert.ToString(ss);
                                }
                                message = "Attandance is Done";
                                manualAttandanceRepo.AddDailyAttandance(attandance);

                            }
                            else
                            {
                                TimeSpan time11 = loginuser.ExitTime.Subtract(loginuser.EntryTime);
                                double totalmins = time11.TotalMinutes;

                                TimeSpan tsp = sp.Subtract(loginuser.ExitTime);
                                double minu = tsp.TotalMinutes;
                                var emp1 = manualAttandanceRepo.GetAttandanceOfDay(timeofday, new LoginUser().LoggedInEmployeeID, onflag).FirstOrDefault();
                                if (emp1.Status == "5")
                                {
                                    attandance.IsDailyAttandance = false;
                                    attandance.ID = emp1.ID;
                                    attandance.ExitTime = Convert.ToString(sp);
                                    attandance.EntryTime = Convert.ToString(emp1.EntryTime);
                                    manualAttandanceRepo.UpdateDailyAttandance(attandance);
                                }
                                else
                                {

                                    emp1.ExitTime = Convert.ToString(sp);
                                    TimeSpan time112 = TimeSpan.Parse(emp1.ExitTime).Subtract(TimeSpan.Parse(emp1.EntryTime));
                                    double mins = time112.TotalMinutes;
                                    if (mins >= -(Convert.ToInt16(setting.ValidTimeBeforeEntry)) && mins <= Convert.ToInt16(setting.ValidTimeAfterEntry))
                                    {

                                        message = " Success ! Attandance Already Done";

                                    }
                                    if (minu >= 0 && minu <= Convert.ToInt16(setting.ValidTimeAfterLeave))
                                    {
                                        attandance.IsDailyAttandance = false;
                                        attandance.ExitTime = Convert.ToString(sp);
                                        attandance.EntryTime = Convert.ToString(emp1.EntryTime);
                                        attandance.ID = emp1.ID;
                                        manualAttandanceRepo.UpdateDailyAttandance(attandance);
                                        message = "Success ! Attandance Done Successfully";

                                    }


                                }
                            }

                        }
                    }
                    else
                    {
                        message = "Time Of Attandance Is Expired " + timeofday;
                        //TempData["timeofday"] = timeofday;
                        //ModelState.AddModelError("attandaceexpired", "Time For Attendace is Expired");
                        //return View("Index");
                    }

                }
              
                return Json(messageHandlerRepo.GetMessage(message));
            }
            catch (Exception ex)
            {
                message = MassageDescription.ExceptionOrNullError;
                return Json(messageHandlerRepo.GetMessage(message));

            }

        }

        public DateTime GetNepaliDateTime()
        {
            Dictionary<int, int[]> NepaliMap = new Dictionary<int, int[]>();
            NepaliMap.Add(2000, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2001, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2002, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2003, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2004, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2005, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2006, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2007, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2008, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31 });
            NepaliMap.Add(2009, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2010, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2011, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2012, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 });
            NepaliMap.Add(2013, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2014, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2015, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2016, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 });
            NepaliMap.Add(2017, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2018, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2019, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2020, new int[] { 0, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2021, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2022, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 });
            NepaliMap.Add(2023, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2024, new int[] { 0, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2025, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2026, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2027, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2028, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2029, new int[] { 0, 31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2030, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2031, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2032, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2033, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2034, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2035, new int[] { 0, 30, 32, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31 });
            NepaliMap.Add(2036, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2037, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2038, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2039, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 });
            NepaliMap.Add(2040, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2041, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2042, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2043, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 });
            NepaliMap.Add(2044, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2045, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2046, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2047, new int[] { 0, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2048, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2049, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 });
            NepaliMap.Add(2050, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2051, new int[] { 0, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2052, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2053, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 });
            NepaliMap.Add(2054, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2055, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2056, new int[] { 0, 31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2057, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2058, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2059, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2060, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2061, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2062, new int[] { 0, 30, 32, 31, 32, 31, 31, 29, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2063, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2064, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2065, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2066, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31 });
            NepaliMap.Add(2067, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2068, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2069, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2070, new int[] { 0, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30 });
            NepaliMap.Add(2071, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2072, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2073, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 });
            NepaliMap.Add(2074, new int[] { 0, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2075, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2076, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 });
            NepaliMap.Add(2077, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 });
            NepaliMap.Add(2078, new int[] { 0, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2079, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 });
            NepaliMap.Add(2080, new int[] { 0, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30 });
            NepaliMap.Add(2081, new int[] { 0, 31, 31, 32, 32, 31, 30, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2082, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2083, new int[] { 0, 31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2084, new int[] { 0, 31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2085, new int[] { 0, 31, 32, 31, 32, 30, 31, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2086, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2087, new int[] { 0, 31, 31, 32, 31, 31, 31, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2088, new int[] { 0, 30, 31, 32, 32, 30, 31, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2089, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 });
            NepaliMap.Add(2090, new int[] { 0, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30 });

            //As we have dictionary of date ranging from 2000 to 2090 the equivalent english date is as follows

            int StartingNepaliYear = 2000;

            int StartingNepaliMonth = 9;

            int StartingNepaliDay = 17;


            DateTime newdate = new DateTime(1944, 1, 1);
            DateTime actualnepalidate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Nepal Standard Time");
            DateTime olddate = new DateTime(actualnepalidate.Year, actualnepalidate.Month, actualnepalidate.Day);
            TimeSpan ts = olddate - newdate;
            int DifferenceInDays = ts.Days;


            int NepaliYear = StartingNepaliYear;
            int NepaliMonth = StartingNepaliMonth;
            int NepaliDay = StartingNepaliDay;
            int DayOfWeek = 7;

            while (DifferenceInDays != 0)
            {

                int[] days = NepaliMap[NepaliYear];
                int p = days[NepaliMonth];

                NepaliDay++; // incrementing nepali day

                if (NepaliDay > p)
                {
                    NepaliMonth++;
                    NepaliDay = 1;
                }
                if (NepaliMonth > 12)
                {
                    NepaliYear++;
                    NepaliMonth = 1;
                }
                DayOfWeek++;
                if (DayOfWeek > 7)
                {
                    DayOfWeek = 1;
                }

                DifferenceInDays--;
            }

            int months = NepaliMonth;
            int day = DayOfWeek;

            var serverTime = DateTime.Now;
            DateTime timeofday = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Nepal Standard Time");
            return  new DateTime(NepaliYear, NepaliMonth, NepaliDay, timeofday.Hour, timeofday.Minute, timeofday.Second);
          
        }

    

    }
}