using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Admin.Controllers
{
    public class DeviceAttandanceController : Controller
    {
        
        private readonly IMessageHandlerRepository messageHandlerRepo;
        private readonly IEmployerRepository employeeRepo;
        private readonly IManualAttandanceRepository manualAttandanceRepo;
        private readonly IDropDownRepository dropDownRepo;
        string message = "";
        int operationStatus;
        
        string days;
        // GET: Admin/DeviceAttandance

        public DeviceAttandanceController(IDropDownRepository dropDownRepo, IMessageHandlerRepository messageHandlerRepo, IEmployerRepository employeeRepo, IManualAttandanceRepository manualAttandanceRepo)
        {
            this.messageHandlerRepo = messageHandlerRepo;
            this.employeeRepo = employeeRepo;
            this.manualAttandanceRepo = manualAttandanceRepo;
            this.dropDownRepo = dropDownRepo;
        }
        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }
        public PartialViewResult LoadPartialView()
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

            var allMonth = dropDownRepo.GetMonthDropDown();
            if (allMonth != null)
            {
                ViewBag.monthList = new SelectList(allMonth, "ID", "Name");
            }
            else
            {
                ViewBag.monthList = new SelectList(dropDownRepo.GetErrorList(), "ID", "Name");
            }


        }

        public JsonResult PullData(BiometricDevice biometric)
        {
            try
            {


                var axCZKEM1 = new zkemkeeper.CZKEMClass();
                bool bIsConnected = false;//the boolean value identifies whether the device is connected
                int iMachineNumber = 1;//the serial number of the device.After connecting the device ,this value will be changed.
                string sdwEnrollNumber = "";
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;
                try
                {
                    bIsConnected = axCZKEM1.Connect_Net(biometric.IpAddress, biometric.Port);
                    if (bIsConnected == false)
                    {
                        message = MassageDescription.ConnectionFailure;
                        return Json(new { operationStatus = StatusCodeDescription.failure, message });
                    }
                    if (bIsConnected == true)
                    {
                        int iValue = 0;
                        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
                        axCZKEM1.GetDeviceStatus(iMachineNumber, 6, ref iValue); //Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
                        int counts = iValue;
                        if (counts == 0)
                        {
                            message = MassageDescription.NoRecordsInDevice;
                            return Json(new { operationStatus = StatusCodeDescription.failure, message });

                        }
                        else
                        {
                            var attandance = new Attandance();
                            var attandanceHistory = new AttandanceHistory();
                            attandance.LeaveDaysID = 1;
                            attandance.DateFrom = DateTime.Now;
                            attandance.DateTo = DateTime.Now;
                            attandance.AttandanceType = "D";
                            if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
                            {
                                while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                                        out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                                {
                                    var date = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                                    attandanceHistory.UserID = sdwEnrollNumber;
                                    attandanceHistory.DateTime = date;
                                    manualAttandanceRepo.AddAttandanceHistory(attandanceHistory);
                                    var loginuser = employeeRepo.EditEmployeeDeviceUserID(Convert.ToInt16(sdwEnrollNumber));
                                    if (loginuser == null)
                                    {
                                        continue;
                                    }
                                    var setting = manualAttandanceRepo.GetSettingsTime();
                                    var Devicetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, idwHour, idwMinute, idwSecond);
                                    bool offflag = false;
                                    bool onflag = true;
                                    TimeSpan sp = Devicetime.TimeOfDay;
                                    TimeSpan ss = new TimeSpan(00, 00, 00);
                                    TimeSpan time1 = loginuser.EntryTime;
                                    TimeSpan time2 = (sp.Subtract(time1));
                                    double totalminutes = time2.TotalMinutes;
                                    DateTime timeofday = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Nepal Standard Time");


                                    var minute = (timeofday.TimeOfDay.Subtract(loginuser.EntryTime)).TotalMinutes;
                                    var minut = (timeofday.TimeOfDay.Subtract(loginuser.ExitTime)).TotalMinutes;


                                    var attandanceforthatday = manualAttandanceRepo.GetAttandanceOfDay(timeofday, new LoginUser().LoggedInEmployeeID, offflag).Count();
                                    if (attandanceforthatday > 0)
                                    {
                                        continue;
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
                                                attandance.Days = 1;
                                                attandance.IsKaaj = false;
                                                attandance.IsManualAttandance = true;
                                                attandance.EntryTime = Convert.ToString(Devicetime.TimeOfDay);
                                                attandance.ExitTime = Convert.ToString(ss);
                                            }
                                            else
                                            {
                                                attandance.IsDailyAttandance = true;
                                                attandance.EmployeeID = new LoginUser().LoggedInEmployeeID;
                                                attandance.Status = "5";
                                                attandance.IsKaaj = false;
                                                attandance.IsManualAttandance = true;
                                                attandance.Days = 0;
                                                attandance.EntryTime = Convert.ToString(Devicetime.TimeOfDay);
                                                attandance.ExitTime = Convert.ToString(ss);
                                            }

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

                                                    continue;

                                                }
                                                if (minu >= 0 && minu <= Convert.ToInt16(setting.ValidTimeAfterLeave))
                                                {
                                                    attandance.IsDailyAttandance = false;
                                                    attandance.ExitTime = Convert.ToString(sp);
                                                    attandance.EntryTime = Convert.ToString(emp1.EntryTime);
                                                    attandance.ID = emp1.ID;
                                                    manualAttandanceRepo.UpdateDailyAttandance(attandance);


                                                }


                                            }
                                        }

                                    }


                                }
                            }

                            else
                            {
                                message = MassageDescription.ConnectDeviceFirst;
                                return Json(new { operationStatus = StatusCodeDescription.failure, message });
                            }


                        }

                    }
                    if (axCZKEM1.ClearGLog(iMachineNumber))
                    {
                        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                        axCZKEM1.Disconnect();

                    }
                    message = MassageDescription.DataPullSuccess;
                    return Json(new { operationStatus = StatusCodeDescription.success, message });
                }
                catch (Exception ex)
                {
                    axCZKEM1.Disconnect();
                    message = MassageDescription.ErrorOccured;
                    return Json(new { operationStatus = 200, message });
                }
            }
            catch(Exception ex)
            {
                message = MassageDescription.ErrorOccured;
                return Json(new { operationStatus = 200, message });
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult ConnectDevice(BiometricDevice biometric)
        //{
        //    try
        //    {

        //        if (biometric != null)
        //        {

        //            if (ModelState.IsValid)
        //            {
        //                if (biometric.connectDevice == "Connect")
        //                {
        //                    bIsConnected = axCZKEM1.Connect_Net(biometric.IpAddress, biometric.Port);
        //                    if (bIsConnected)
        //                    {
        //                        iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
        //                        axCZKEM1.RegEvent(iMachineNumber, 65535);//Here you can register the realtime events that you want to be triggeuered(the parameters 65535 means registering all)
        //                        message = MassageDescription.ConnectionSuccess;
        //                        operationStatus = StatusCodeDescription.success;
        //                    }
        //                    else
        //                    {
        //                        message = MassageDescription.ConnectionFailure;
        //                        operationStatus = StatusCodeDescription.failure;
        //                    }
        //                }
        //                else
        //                {
        //                    axCZKEM1.Disconnect();
        //                    message = MassageDescription.DisconnectDevice;
        //                    operationStatus = 300;
        //                }

        //            }
        //            else
        //            {
        //                message = MassageDescription.ModelErrorOccured;
        //                operationStatus = StatusCodeDescription.failure;

        //            }
        //        }
        //        else
        //        {
        //            message = MassageDescription.ExceptionOrNullError;
        //            operationStatus = StatusCodeDescription.failure;

        //        }
        //        return Json(new { operationStatus, message });
        //    }
        //    catch (Exception ex)
        //    {
        //        message = MassageDescription.ExceptionOrNullError;
        //        return Json(new { operationStatus, message });

        //    }


        //}
    }
}