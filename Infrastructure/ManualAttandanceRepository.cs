using Dapper;
using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.Mobile;

namespace Infrastructure
{
   public class ManualAttandanceRepository:IManualAttandanceRepository
    {

        public SettingsTime GetSettingsTime()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();          
                                       

                    var settings = SqlMapper.Query<SettingsTime>(connection, "[dbo].[GetAllSettingsValue]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return settings;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public bool AddUpdateManualAttandance(Attandance leave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", leave.ID);
                    parameters.Add("@OrganisationID", leave.OrganisationID);
                    parameters.Add("@LeaveTypeID", leave.LeaveTypeID);
                    parameters.Add("@EmployeeID", leave.EmployeeID);
                    parameters.Add("@LeaveDaysID", leave.LeaveDaysID);
                    parameters.Add("@DateFrom", leave.DateFrom);
                    parameters.Add("@DateTo", leave.DateTo);
                    parameters.Add("@NepaliDateFrom", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateFrom));
                    parameters.Add("@NepaliDateTo", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateTo));
                    parameters.Add("@Days", leave.Days);
                    parameters.Add("@Year", leave.Year);
                    parameters.Add("@Description", leave.Description);
                    parameters.Add("@EntryTime", leave.EntryTime);
                    parameters.Add("@ExitTime", leave.ExitTime);
                    parameters.Add("@Status", leave.Status);
                    parameters.Add("@Month", leave.Month);
                    parameters.Add("@IsManualAttandance", true);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    parameters.Add("@EntryLocation", "LoginAdmin");
                    parameters.Add("@ExitLocation", "LoginAdmin");
                    parameters.Add("@AttandanceType","ManualLoginAdmin");
                    var savechanges = connection.Execute("[dbo].[AddUpdateManualAttandance]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ChekAttandanceAlreadydone(Attandance leave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                   
                    parameters.Add("@OrganisationID", leave.OrganisationID);                    
                    parameters.Add("@EmployeeID", leave.EmployeeID);                    
                    parameters.Add("@DateFrom", leave.DateFrom);
                    parameters.Add("@DateTo", leave.DateTo);
                    var count = SqlMapper.Query<int>(connection, "[dbo].[CheckManualAttandance]", parameters, commandType: CommandType.StoredProcedure).ToList();
                    
                    if (count[0] > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Attandance> GetAllManualAttandance(Attandance search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    param.Add("@PageSize", search.pageSize);
                    param.Add("@OrganisationIDSearch", search.OrganisationIDSearch);
                    param.Add("@EmployerIDSearch", search.EmployerIDSearch);
                    param.Add("@YearSearch", search.YearSearch);
                    param.Add("@MonthSearch", search.MonthSearch);
                    param.Add("@DateSearch", search.DateSearch);

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<Attandance> employeeList = SqlMapper.Query<Attandance>(connection, "[dbo].[GetAllManualAttandance]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Attandance> GetAttandanceOfDay(DateTime datetime, int employeeid, bool isdailyattandance)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();                   
                   
                    param.Add("@Date", datetime);
                    param.Add("@EmployeeId", employeeid);
                    param.Add("@IsDailyAttandance", isdailyattandance);

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<Attandance> employeeList = SqlMapper.Query<Attandance>(connection, "[dbo].[GetDailyManualAttandance]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddDailyAttandance(Attandance leave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                  
                    parameters.Add("@OrganisationID", leave.OrganisationID);
                    parameters.Add("@LeaveTypeID", leave.LeaveTypeID);
                    parameters.Add("@EmployeeID", leave.EmployeeID);
                    parameters.Add("@LeaveDaysID", leave.LeaveDaysID);
                    parameters.Add("@DateFrom", leave.DateFrom);
                    parameters.Add("@DateTo", leave.DateTo);
                    parameters.Add("@NepaliDateFrom", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateFrom));
                    parameters.Add("@NepaliDateTo", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateTo));
                    parameters.Add("@Days", leave.Days);
                    parameters.Add("@Year", leave.Year);
                    parameters.Add("@Description", leave.Description="Daily Attandance");
                    parameters.Add("@EntryTime", leave.EntryTime);
                    parameters.Add("@ExitTime", leave.ExitTime);
                    parameters.Add("@Status", leave.Status);
                    parameters.Add("@Month", leave.Month);
                    parameters.Add("@IsManualAttandance",leave.IsManualAttandance);
                    parameters.Add("@IsDailyAttandance", leave.IsDailyAttandance);
                    parameters.Add("@IsKaajAttandance", leave.IsKaaj);
                    parameters.Add("@AddedBy",leave.AddedBy);
                    parameters.Add("@UpdatedBy",leave.UpdatedBy);
                    parameters.Add("@AttandanceType", leave.AttandanceType);
                    parameters.Add("@EntryLocation", leave.EntryLocation);
                    var savechanges = connection.Execute("[dbo].[AddEmployeeDailyAttandance]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

      public bool AddAttandanceHistory(AttandanceHistory history)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@UserID", history.UserID);
                    parameters.Add("@Date", history.DateTime);                    
               
                    var savechanges = connection.Execute("[dbo].[AddEmployeeAttandanceHistory]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool UpdateDailyAttandance(Attandance attend)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@ID", attend.ID);
                    parameters.Add("@IsDailyAttandance", attend.IsDailyAttandance);
                    parameters.Add("@EntryTime", attend.EntryTime);
                    parameters.Add("@ExitTime", attend.ExitTime);
                    parameters.Add("@ExitLocation", attend.ExitLocation);


                    var savechanges = connection.Execute("[dbo].[UpdateEmployeeDailyAttandance]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Attandance> GetAllDailyManualAttandance(Attandance search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);
                    param.Add("@EmployeeId",new LoginUser().LoggedInEmployeeID);
                    param.Add("@PageSize", search.pageSize);
                    param.Add("@MonthSearch", search.MonthSearch);
                    param.Add("@YearSearch", search.YearSearch);
                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<Attandance> employeeList = SqlMapper.Query<Attandance>(connection, "[dbo].[GetAllDailyManualAttandance]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region Mobile

        public MobileAttandanceList GetAllAttandanceMobile(GeneralViewModel<string> model)
        {
            try
            {
                var mobileAttandance = new MobileAttandanceList();
                var attandance = new List<MobileAttandance>();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    
                    param.Add("@EmployeeId", model.LoginInfo.EmployeeID);            
                    
                    param.Add("@Year", model.LoginInfo.Year);
                    

                    List<MobileAttandance> attandanceList = SqlMapper.Query<MobileAttandance>(connection, "[dbo].[GetAllDailyAttandanceMobile]", param, commandType: CommandType.StoredProcedure).ToList();
                    attandanceList.ForEach(x =>
                    {
                        if(x.EntryTime== "12:00AM")
                        {
                            x.EntryTime = "00.00";
                        }
                        if (x.ExitTime == "12:00AM")
                        {
                            x.ExitTime = "00.00";
                        }

                    });

                    if (attandanceList.Count() > 0)
                    {
                        mobileAttandance.Attandance = attandanceList;
                        mobileAttandance.IsAttandanceAvailiable = true;
                        mobileAttandance.AttandanceMessage = "";

                    }
                    else
                    {
                        mobileAttandance.Attandance = null;
                        mobileAttandance.IsAttandanceAvailiable = false;
                        mobileAttandance.AttandanceMessage = "No Attandance Record Availiable";
                    }
                    return mobileAttandance;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AttandanceStatus GetAttandanceStatus(GeneralViewModel<DeviceLogViewModel> model)
        {
            var attandanceStatus = new AttandanceStatus();
            try
            {
               
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    var date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Nepal Standard Time");
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EmployeeID", model.LoginInfo.EmployeeID);
                    param.Add("@Date",date.Date );
                    if (model.Model != null)
                    {
                        DynamicParameters parameter = new DynamicParameters();
                        parameter.Add("@EmployeeID", model.LoginInfo.EmployeeID);
                        parameter.Add("@DeviceKey", model.Model.DeviceToken);
                        parameter.Add("@CreatedDate", DateTime.Now);
                        parameter.Add("@ModifiedDate", DateTime.Now);
                        var savechanges = connection.Execute("[dbo].[AddUpdateDeviceLog]", parameter, commandType: CommandType.StoredProcedure);
                    }



                    AttandanceStatus attandanceStat = SqlMapper.Query<AttandanceStatus>(connection, "[dbo].[GetAllDailyAttandanceStatus]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (attandanceStat == null)
                    {
                        attandanceStatus.IsCheckedIn = false;
                        attandanceStatus.IsCheckedOut = false;
                        attandanceStatus.CheckedInTime = "";
                        attandanceStatus.CheckedOutTime = "";
                    
                        attandanceStatus.AttandanceStatusMessage = "You haven't clock in yet";

                    }
                    else
                    {
                        if(!string.IsNullOrEmpty(attandanceStat.CheckedInTime) && attandanceStat.CheckedOutTime== "12:00AM")
                        {
                            attandanceStatus.IsCheckedIn = true;
                            attandanceStatus.IsCheckedOut = false;
                            attandanceStatus.CheckedInTime = attandanceStat.CheckedInTime;
                            attandanceStatus.CheckedOutTime = "";
                        
                            attandanceStatus.AttandanceStatusMessage = "Dont forget to Check Out";
                        }

                       

                       else if (attandanceStat.CheckedInTime== "12:00AM" && !string.IsNullOrEmpty(attandanceStat.CheckedOutTime))
                        {
                            attandanceStatus.IsCheckedIn = false;
                            attandanceStatus.IsCheckedOut = true;
                            attandanceStatus.CheckedInTime = attandanceStat.CheckedInTime;
                            attandanceStatus.CheckedOutTime = attandanceStat.CheckedOutTime;
                            
                            attandanceStatus.AttandanceStatusMessage = "Forgot To Check In !!";
                        }

                       else if (!string.IsNullOrEmpty(attandanceStat.CheckedInTime) && !string.IsNullOrEmpty(attandanceStat.CheckedOutTime))
                        {
                            attandanceStatus.IsCheckedIn = true;
                            attandanceStatus.IsCheckedOut = true;
                            attandanceStatus.CheckedInTime = attandanceStat.CheckedInTime;
                            attandanceStatus.CheckedOutTime = attandanceStat.CheckedOutTime;
                           
                            attandanceStatus.AttandanceStatusMessage = "See you tommorow";
                        }
                    }

                    return attandanceStatus;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
