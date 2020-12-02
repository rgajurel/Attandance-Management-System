using DomainEntities;
using DomainEntities.Mobile;
using DomainInterface;
using SchoolManagementSystem.App_Start;
using SchoolManagementSystem.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SchoolManagementSystem.Controllers.API
{

    [MobileAuthorize]
    [RoutePrefix("api/mobile")]
    public class MobileController : ApiController
    {
        private readonly ITakeLeaveRepository takeLeaveRepo;
        private readonly IEmployerRepository employeeRepo;
        private readonly IMobileRepository mobileRepo;
        public readonly IOfficialLeaveRepository officialLeaveRepo;

        private readonly IUserRepository userRepo;
        private readonly IManualAttandanceRepository manualRepo;
        public readonly INotificationRepository notificationRepo;

        public MobileController(INotificationRepository notificationRepo,IEmployerRepository employeeRepo, IMobileRepository mobileRepo, IOfficialLeaveRepository officialLeaveRepo, ITakeLeaveRepository takeLeaveRepo, IUserRepository userRepo, IManualAttandanceRepository manualRepo)
        {
            this.takeLeaveRepo = takeLeaveRepo;
            this.userRepo = userRepo;
            this.manualRepo = manualRepo;
            this.employeeRepo = employeeRepo;
            this.notificationRepo = notificationRepo;
            this.mobileRepo = mobileRepo;
            this.officialLeaveRepo = officialLeaveRepo;
        }
       

        [HttpPost]
        [Route("takeleave")]
        public IHttpActionResult TakeLeave(ClientTakeLeave takeleave)
        {
            try
            {
                var data = "".ToService();
                DateTime datefrom;
                DateTime dateto;
                DateTime.TryParseExact(takeleave.StartDate, "MM/dd/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None, out datefrom);
                DateTime.TryParseExact(takeleave.EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateto);
                takeleave.DateFrom = datefrom;
                takeleave.DateTo = dateto;
                if (datefrom.Date == dateto.Date)
                {
                    takeleave.Days = "1";
                }
                takeleave.Days = ((dateto.Date - datefrom.Date).Days +1).ToString();

                if (datefrom.Date > dateto.Date)
                {
                    throw new ArgumentException("From Date Must be Less Than or Equal To To Date");
                }

                bool savechanges = false;
                var totalremainingdays = takeLeaveRepo.CalculateRemainingLeave(takeleave.ToService());
                if (string.IsNullOrEmpty(totalremainingdays) || totalremainingdays == "0")
                {
                    throw new ArgumentException("You Dont Have Leave To Apply For");
                }
                else
                {


                    savechanges = takeLeaveRepo.AddUpdateTakeLeave(takeleave.ToService());
                }

                if (savechanges)
                {
                    Notification notification = new Notification()
                    {
                        Title = "Leave Request From " + data.LoginInfo.UserName,
                        Description = "You Have Pending Leave Request To Be Approved of " + data.LoginInfo.UserName,
                        EmployeeID = takeleave.ApprovedBy
                    };
                    notificationRepo.PushNotificationToUser(notification);

                    return Ok(new { Message = "Leave Applied Successfully" });
                }
                else
                {
                    throw new ArgumentException("Some Error Occured");
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }


        }

        [HttpGet]
        [Route("pushnotification")]
        [AllowAnonymous]

        public IHttpActionResult PushNotification()
        {
            try
            {
                Notification notification = new Notification()
                {
                    Title = "<strong>Leave Request From ram<strong>",
                    Description = "You Have Pending Leave Request To Be Approved",
                    EmployeeID = "2063"
                };
                notificationRepo.PushNotificationToUser(notification);
                return Ok();


            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }


        }


        [HttpPost]
        [Route("travelrequest")]
        public IHttpActionResult TravelRequest(Attandance attandance)
        {
            try
            {
                var data = "".ToService();
                DateTime datefrom;
                DateTime dateto;
                DateTime.TryParseExact(attandance.StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datefrom);
                DateTime.TryParseExact(attandance.EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateto);
                attandance.DateFrom = datefrom;
                attandance.DateTo = dateto;
                attandance.EmployeeID = Convert.ToInt16(data.LoginInfo.EmployeeID);
                attandance.OrganisationID = Convert.ToInt16(data.LoginInfo.OrganisationID);
                attandance.AddedBy = data.LoginInfo.UserName;
                attandance.AttandanceType = "Kaaj";
               // attandance.EntryLocation = data.LocationDetails.Address;
                //attandance.ExitLocation = data.LocationDetails.Address;

                if (datefrom.Date == dateto.Date)
                {
                    attandance.Days = 1;
                }
                attandance.Days = ((dateto.Date - datefrom.Date).Days + 1);

                if (datefrom.Date > dateto.Date)
                {
                    throw new ArgumentException("From Date Must be Less Than or Equal To To Date");
                }

                bool savechanges = officialLeaveRepo.AddUpdateMobileTravellRequest(attandance);            
                          

                if (savechanges)
                {
                    Notification notification = new Notification()
                    {
                        Title = "Travel Request From " + data.LoginInfo.UserName,
                        Description = "You Have Pending Travel Request To Be Approved of " + data.LoginInfo.UserName,
                        EmployeeID = attandance.ApprovedBy
                    };
                    notificationRepo.PushNotificationToUser(notification);

                    return Ok(new { Message = "Travel Request Applied Successfully" });
                }
                else
                {
                    throw new ArgumentException("Some Error Occured");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }


        }

        [HttpGet]
        [Route("leavehistory")]
        public IHttpActionResult LeaveHistory()
        {
            try
            {
                var leavehistory = new List<LeaveHistory>();
               
                var getleaveHistory = takeLeaveRepo.LeaveHistoryList("".ToService());
                return Ok(getleaveHistory);



            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }


        }

        [HttpGet]
        [Route("leavehistorystatus")]
        public IHttpActionResult LeaveHistoryStatus()
        {
            try
            {          

                var getleaveHistory = takeLeaveRepo.LeaveHistoryStatus("".ToService());
                return Ok(getleaveHistory);

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }

        [HttpPost]
        [Route("getattandancestatus")]
        public IHttpActionResult GetAttandanceStatus(DeviceLogViewModel model)
        {
            try
            {               
                var getattandanceStatus = manualRepo.GetAttandanceStatus(model.ToService());               
                return Ok(getattandanceStatus);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }

        [HttpGet]
        [Route("getprofileinfo")]
        public IHttpActionResult UserProfileDetails()
        {
            try
            {
                var url = ConfigurationManager.AppSettings["Url"];
                var getProfileInfo = userRepo.GetUserProfileInfo("".ToService().LoginInfo.EmployeeID);
                if(!string.IsNullOrWhiteSpace(getProfileInfo.ImageUrl) || !string.IsNullOrEmpty(getProfileInfo.ImageUrl))
                {
                    getProfileInfo.ImageUrl = string.Concat(url, getProfileInfo.ImageUrl);
                }
                return Ok(getProfileInfo);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }


        [HttpGet]
        [Route("getallattandance")]
        public IHttpActionResult GetaAllAttandance()
        {
            try
            {
                var getallAttandnce = manualRepo.GetAllAttandanceMobile("".ToService());
                return Ok(getallAttandnce);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }


        [HttpGet]
        [Route("getalltravelrequest")]
        public IHttpActionResult GetAllTravelRequest()
        {
            try
            {
                var getallAttandnce = officialLeaveRepo.TravelRequestList("".ToService());
                return Ok(getallAttandnce);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }

        [HttpGet]
        [Route("upcomingevents")]
        public IHttpActionResult GetaAllUpComingEvents()
        {
            try
            {
                var getallAttandnce = mobileRepo.GetAllOrganisationEvents("".ToService());
                return Ok(getallAttandnce);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }

        [HttpGet]
        [Route("getallholidays")]
        public IHttpActionResult GetAllHolidayList()
        {
            try
            {
                var getallHolidaysList = mobileRepo.GetAllOrganisationHolidaysList("".ToService());
                return Ok(getallHolidaysList);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some Error Occured");
            }

        }

        [HttpPost]
        [Route("dodailyattandance")]
        public IHttpActionResult DoDailyAttandance()
        {
            try
            {
                var latlong = HttpContext.Current.Request.Headers["LatLong"];
                if(string.IsNullOrEmpty(latlong)|| string.IsNullOrWhiteSpace(latlong))
                {
                    throw new ArgumentException("Please Enable Location For Attandance");
                }
                var data = new DeviceLogViewModel().ToService();              
                var attandance = new Attandance();
                attandance.AddedBy = data.LoginInfo.FullName;
                attandance.UpdatedBy = data.LoginInfo.FullName;
                attandance.LeaveDaysID = 1;
                attandance.DateFrom = DateTime.Now;
                attandance.DateTo = DateTime.Now;
                attandance.OrganisationID = Convert.ToInt16(data.LoginInfo.OrganisationID);
                attandance.Year = Convert.ToInt16(data.LoginInfo.Year);
                attandance.Month = Convert.ToInt16(data.LoginInfo.Month);              
                attandance.AttandanceType = "ManualLoginMobile";
                  
                  var loginuser = employeeRepo.EditEmployee(Convert.ToInt16(data.LoginInfo.EmployeeID));
                    var setting = manualRepo.GetSettingsTime();
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
                        var attandanceforthatday = manualRepo.GetAttandanceOfDay(timeofday, Convert.ToInt16(data.LoginInfo.EmployeeID), offflag).Count();
                        if (attandanceforthatday > 0)
                        {
                            throw new ArgumentException("Success ! Attandance For That Day is Already Done");
                        }
                        else
                        {
                            var count = manualRepo.GetAttandanceOfDay(timeofday, Convert.ToInt16(data.LoginInfo.EmployeeID), onflag).Count();

                            if (count <= 0)
                            {
                                if (totalminutes <= Convert.ToInt16(setting.ValidTimeAfterEntry) && totalminutes >= -(Convert.ToInt16(setting.ValidTimeBeforeEntry)))
                                {
                                    attandance.IsDailyAttandance = true;
                                    attandance.EmployeeID = Convert.ToInt16(data.LoginInfo.EmployeeID);
                                    attandance.Status = "4";
                                    attandance.IsKaaj = false;
                                    attandance.Days = 1;
                                    attandance.IsManualAttandance = true;
                                    attandance.EntryTime = Convert.ToString(Nepalidatetime.TimeOfDay);
                                    attandance.ExitTime = Convert.ToString(ss);
                                    attandance.EntryLocation = data.LocationDetails.Address==null?"":data.LocationDetails.Address;

                                }
                                else
                                {
                                    attandance.IsDailyAttandance = true;
                                    attandance.EmployeeID = Convert.ToInt16(data.LoginInfo.EmployeeID);
                                    attandance.Status = "5";
                                    attandance.IsKaaj = false;
                                attandance.Days = 0;
                                attandance.IsManualAttandance = true;
                                    attandance.EntryTime = Convert.ToString(Nepalidatetime.TimeOfDay);
                                    attandance.ExitTime = Convert.ToString(ss);
                                attandance.EntryLocation = data.LocationDetails.Address == null ? "" : data.LocationDetails.Address;
                            }
                                
                                manualRepo.AddDailyAttandance(attandance);

                            }
                            else
                            {
                                TimeSpan time11 = loginuser.ExitTime.Subtract(loginuser.EntryTime);
                                double totalmins = time11.TotalMinutes;

                                TimeSpan tsp = sp.Subtract(loginuser.ExitTime);
                                double minu = tsp.TotalMinutes;
                                var emp1 = manualRepo.GetAttandanceOfDay(timeofday, Convert.ToInt16(data.LoginInfo.EmployeeID), onflag).FirstOrDefault();
                                if (emp1.Status == "5")
                                {
                                    attandance.IsDailyAttandance = false;
                                    attandance.ID = emp1.ID;
                                    attandance.ExitTime = Convert.ToString(sp);
                                    attandance.EntryTime = Convert.ToString(emp1.EntryTime);
                                    attandance.ExitLocation = data.LocationDetails.Address==null?"":data.LocationDetails.Address;
                                    manualRepo.UpdateDailyAttandance(attandance);
                                }
                                else
                                {

                                    emp1.ExitTime = Convert.ToString(sp);
                                    TimeSpan time112 = TimeSpan.Parse(emp1.ExitTime).Subtract(TimeSpan.Parse(emp1.EntryTime));
                                    double mins = time112.TotalMinutes;
                                    if (mins >= -(Convert.ToInt16(setting.ValidTimeBeforeEntry)) && mins <= Convert.ToInt16(setting.ValidTimeAfterEntry))
                                    {

                                       throw new ArgumentException("Clock In Already Done");

                                    }
                                    if (minu >= 0 && minu <= Convert.ToInt16(setting.ValidTimeAfterLeave))
                                    {
                                        attandance.IsDailyAttandance = false;
                                        attandance.ExitTime = Convert.ToString(sp);
                                        attandance.EntryTime = Convert.ToString(emp1.EntryTime);
                                        attandance.ID = emp1.ID;
                                        attandance.ExitLocation = data.LocationDetails.Address == null ? "" : data.LocationDetails.Address;
                                      manualRepo.UpdateDailyAttandance(attandance);
                                        
                                    }


                                }
                            }

                        }
                    }
                    else
                    {
                        throw new ArgumentException("Time Of Attandance Is Expired " + timeofday);
                        
                    }

                return Ok(manualRepo.GetAttandanceStatus(data));
            }
            catch (Exception ex)
            {
                throw ex;
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
            return new DateTime(NepaliYear, NepaliMonth, NepaliDay, timeofday.Hour, timeofday.Minute, timeofday.Second);

        }




    }
}
