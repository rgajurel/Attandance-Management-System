using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using DomainEntities.Mobile;

namespace Infrastructure
{
    public class TakeLeaveRepository : ITakeLeaveRepository
    {
        #region Admin
        public List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);                  
                    List<LeaveType> leaveTypeOrganisation = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetExpireAndAccumulativeLeave]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return leaveTypeOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LeaveType> GetAccumulativeLeaveTypeBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<LeaveType> leaveTypeOrganisation = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetAccumulativeLeavetype]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return leaveTypeOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<DropDownCommon> GetEmployeeBasedOnOrganisationAndLeaveType(string organisationid, string leavetypeid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", organisationid);
                    param.Add("@LeaveTypeID", leavetypeid);
                    List<DropDownCommon> employeeDropdown = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetEmployeeBasedOnOrganisationAndLeaveType]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return employeeDropdown;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string CalculateRemainingLeave(TakeLeave takeleave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", takeleave.OrganisationID);
                    param.Add("@EmployeeID", takeleave.EmployeeID);
                    param.Add("@LeaveTypeID", takeleave.LeaveTypeID);
                    param.Add("@Year", takeleave.Year);
                    param.Add("@Month", takeleave.Month);
                    TakeLeave remainingleave = SqlMapper.Query<TakeLeave>(connection, "[dbo].[CalculateRemainingLeave]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                  return   remainingleave.TotalLeaveTaken;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CalculateRemainingLeave(GeneralViewModel<ClientTakeLeave> takeleave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", takeleave.LoginInfo.OrganisationID);
                    param.Add("@EmployeeID", takeleave.LoginInfo.EmployeeID);
                    param.Add("@LeaveTypeID", takeleave.Model.LeaveTypeID);
                    param.Add("@Year", takeleave.Model.Year);
                    param.Add("@Month", takeleave.Model.Month);
                    TakeLeave remainingleave = SqlMapper.Query<TakeLeave>(connection, "[dbo].[CalculateRemainingLeave]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return remainingleave.TotalLeaveTaken;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddUpdateTakeLeave(TakeLeave leave)
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
                    parameters.Add("@Month", leave.Month);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateTakeLeave]", parameters, commandType: CommandType.StoredProcedure);
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

        public List<DropDownCommon> GetEmployeeBasedOnOrganisation(string organisationid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", organisationid);                   
                    List<DropDownCommon> employeeDropdown = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetEmployeeBasedOnOrganisation]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return employeeDropdown;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TakeLeave> GetAllTakeLeave(TakeLeave search)
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
                    param.Add("@LeaveTypeIDsearch", search.LeaveTypeIDsearch);
                    param.Add("@YearSearch", search.YearSearch);
                    param.Add("@MonthSearch", search.MonthSearch);

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<TakeLeave> employeeList = SqlMapper.Query<TakeLeave>(connection, "[dbo].[GetAllTakeLeave]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TakeLeave EditTakeLeave(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    TakeLeave employeeedit = SqlMapper.Query<TakeLeave>(connection, "[dbo].[EditTakeLeave]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return employeeedit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteTakeLeave(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteTakeLeave]", parameters, commandType: CommandType.StoredProcedure);
                    var savechanges = parameters.Get<Boolean>("@DeleteSuccess");
                    if (savechanges)
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

        public bool ApproveLeave(string status, string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@status",status);                   
                    parameters.Add("@ID", id);
                    parameters.Add("@UserID", new LoginUser().LoggedInuserID);

                    var savechanges = connection.Execute("[dbo].[UpdateApprovedLeaves]", parameters, commandType: CommandType.StoredProcedure);
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
                return false;
            }
        }

        public bool AddUpdateNotificationTakeLeave(Notification notification,string employeeid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", notification.ID);
                    parameters.Add("@Title", notification.Title);
                    parameters.Add("@Link", notification.Link);
                    parameters.Add("@OrganisationID", notification.OrganisationID);
                    parameters.Add("@NotificationType", notification.NotificationType);
                    parameters.Add("@TriggerNow", notification.TriggerNow);
                    parameters.Add("@TriggerDate", notification.TriggerDate);
                    parameters.Add("@ExpiryDate", notification.ExpiryDate);
                    parameters.Add("@Description", notification.Description);
                    parameters.Add("@IsInternal", notification.IsInternal);
                    parameters.Add("@GroupID", notification.GroupID);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    parameters.Add("@EmployeeID", employeeid);
                    var savechanges = connection.Execute("[dbo].[AddUpdateNotificationLeaveTaken]", parameters, commandType: CommandType.StoredProcedure);
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
                return false;
            }
        }

        #endregion

#region Client
        public bool AddUpdateTakeLeave(ClientTakeLeave leave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", leave.ID);
                    parameters.Add("@OrganisationID",leave.OrganisationID);
                    parameters.Add("@LeaveTypeID", leave.LeaveTypeID);
                    parameters.Add("@EmployeeID",leave.EmployeeID);
                    parameters.Add("@LeaveDaysID", leave.LeaveDaysID);
                    parameters.Add("@DateFrom", leave.DateFrom);
                    parameters.Add("@DateTo", leave.DateTo);
                    parameters.Add("@NepaliDateFrom", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateFrom));
                    parameters.Add("@NepaliDateTo", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateTo));
                    parameters.Add("@Days", leave.Days);
                    parameters.Add("@Year", leave.Year);
                    parameters.Add("@Month", leave.Month);                  
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                  
                    var savechanges = connection.Execute("[dbo].[AddUpdateTakeLeaveClient]", parameters, commandType: CommandType.StoredProcedure);
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

        public List<TakeLeave> GetAllTakeLeave(ClientTakeLeave search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);                 
                   
                    param.Add("@PageSize", search.pageSize);                   
                    param.Add("@EmployerIDSearch",new LoginUser().LoggedInEmployeeID);
                    param.Add("@LeaveTypeIDsearch", search.LeaveTypeIDsearch);
                    param.Add("@YearSearch", search.YearSearch);
                    param.Add("@MonthSearch", search.MonthSearch);
                    param.Add("@Status", search.StatusSearch);

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<TakeLeave> employeeList = SqlMapper.Query<TakeLeave>(connection, "[dbo].[GetAllTakeLeaveClient]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        #endregion

        #region Mobile


        public bool AddUpdateTakeLeave(GeneralViewModel<ClientTakeLeave> leave)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", leave.Model.ID);
                    parameters.Add("@OrganisationID", leave.LoginInfo.OrganisationID);
                    parameters.Add("@LeaveTypeID", leave.Model.LeaveTypeID);
                    parameters.Add("@EmployeeID", leave.LoginInfo.EmployeeID);
                    parameters.Add("@LeaveDaysID", leave.Model.LeaveDaysID);
                    parameters.Add("@DateFrom", leave.Model.DateFrom);
                    parameters.Add("@DateTo", leave.Model.DateTo);
                    parameters.Add("@NepaliDateFrom", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.Model.DateFrom));
                    parameters.Add("@NepaliDateTo", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.Model.DateTo));
                    parameters.Add("@Days", leave.Model.Days);
                    parameters.Add("@Year", leave.Model.Year);
                    parameters.Add("@Month", leave.Model.Month);
                    parameters.Add("@Description", leave.Model.Description);
                    parameters.Add("@AddedBy","" );
                    parameters.Add("@UpdatedBy","");

                    var savechanges = connection.Execute("[dbo].[AddUpdateTakeLeaveClient]", parameters, commandType: CommandType.StoredProcedure);
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
        public  LeaveHistoryList LeaveHistoryList(GeneralViewModel<string>model)
        {
            try
            {
                var leavehistorylist = new LeaveHistoryList();
                leavehistorylist.LeaveHistory = new List<LeaveHistory>();                

                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EmployeeID",model.LoginInfo.EmployeeID);
                    param.Add("@OrganisationID", model.LoginInfo.OrganisationID);
                    param.Add("@YearID", model.LoginInfo.Year);

                    List<LeaveHistory> leaveList = SqlMapper.Query<LeaveHistory>(connection, "[dbo].[GetAllLeaveHistory]", param, commandType: CommandType.StoredProcedure).ToList();
                    if (leaveList.Count() > 0)
                    {
                        leavehistorylist.LeaveHistory = leaveList;
                        leavehistorylist.IsHistoryAvailiable = true;
                    }
                    else
                    {
                        leavehistorylist.LeaveHistory = null;
                        leavehistorylist.IsHistoryAvailiable = false;
                        leavehistorylist.LeaveMessage = "No Leave History Availiable";
                    }

                    return leavehistorylist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


       public LeaveHistoryStatusList LeaveHistoryStatus(GeneralViewModel<string>model)
        {
            try
            {
                var leavehistoryliststatus = new LeaveHistoryStatusList();
                leavehistoryliststatus.LeaveHistoryStatus = new List<LeaveHistoryStatus>();
                              
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EmployeeID", model.LoginInfo.EmployeeID);
                    param.Add("@OrganisationID", model.LoginInfo.OrganisationID);
                    param.Add("@Year", model.LoginInfo.Year);

                    List<LeaveHistoryStatus> leaveList = SqlMapper.Query<LeaveHistoryStatus>(connection, "[dbo].[GetAllLeaveHistoryUserList]", param, commandType: CommandType.StoredProcedure).ToList();
                    if (leaveList.Count() > 0)
                    {
                        leavehistoryliststatus.LeaveHistoryStatus = leaveList;
                        leavehistoryliststatus.IsHistoryAvailiable = true;
                    }
                    else
                    {
                        leavehistoryliststatus.LeaveHistoryStatus = null;
                        leavehistoryliststatus.IsHistoryAvailiable = false;
                        leavehistoryliststatus.LeaveMessage = "No Leave History Status Availiable";
                    }

                    return leavehistoryliststatus;
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
