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
   public class OfficialLeaveRepository: IOfficialLeaveRepository
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
                    List<LeaveType> leaveTypeOrganisation = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetAttandanceLeave]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return leaveTypeOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
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

        public bool AddUpdateOfficialLeave(Attandance leave)
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
                    parameters.Add("@IsKaaj", true);                                        
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    parameters.Add("@Description", leave.Description);
                    parameters.Add("@EntryLocation","From BackOffice");
                    parameters.Add("@ExitLocation", "From BackOffice");
                    parameters.Add("@AttandanceType", "Kaaj");
                    var savechanges = connection.Execute("[dbo].[AddUpdateOfficialLeave]", parameters, commandType: CommandType.StoredProcedure);
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
        public List<Attandance> GetAllOfficialLeave(Attandance search)
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
                    param.Add("@MonthSearch", search.MonthSearch);
                    param.Add("@DateSearch", search.DateSearch);

                    List<Attandance> employeeList = SqlMapper.Query<Attandance>(connection, "[dbo].[GetAllAttandanceLeave]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ApproveLeave(string status, string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@status", status);
                    parameters.Add("@ID", id);
                    parameters.Add("@UserID", new LoginUser().LoggedInuserID);

                    var savechanges = connection.Execute("[dbo].[UpdateApprovedLeavesOfficial]", parameters, commandType: CommandType.StoredProcedure);
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

        public Attandance EditOfficialLeave(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Attandance officialleaveedit = SqlMapper.Query<Attandance>(connection, "[dbo].[EditOfficialLeave]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return officialleaveedit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteOfficialLeave(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteOfficialLeave]", parameters, commandType: CommandType.StoredProcedure);
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

        #endregion Admin



        #region Client

        public List<TakeLeave> GetAllOfficialLeaveClient(Attandance search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);

                    param.Add("@PageSize", search.pageSize);
                    param.Add("@EmployerIDSearch", new LoginUser().LoggedInEmployeeID);
                    param.Add("@YearSearch", search.YearSearch);
                    param.Add("@MonthSearch", search.MonthSearch);
                    param.Add("@Status", search.StatusSearch);

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<TakeLeave> employeeList = SqlMapper.Query<TakeLeave>(connection, "[dbo].[GetAllAttandanceLeaveClient]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool AddUpdateTravellRequest(Attandance leave)
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
                    parameters.Add("@NepaliDateFrom",DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateFrom));
                    parameters.Add("@NepaliDateTo", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(leave.DateTo));
                    parameters.Add("@Days", leave.Days);
                    parameters.Add("@Year", leave.Year);
                    parameters.Add("@Month", leave.Month);
                    parameters.Add("@IsKaaj", true);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    parameters.Add("@Description", leave.Description);
                    parameters.Add("@AttandanceType","Kaaj");
                    parameters.Add("@EntryLocation", "Self Login");
                    parameters.Add("@ExitLocation", "Self Login");
                    var savechanges = connection.Execute("[dbo].[AddUpdateOfficialLeaveClient]", parameters, commandType: CommandType.StoredProcedure);
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

        public TravelRequestList TravelRequestList(GeneralViewModel<string> model)
        {
            try
            {
                var travelRequestList = new TravelRequestList();
                travelRequestList.TravelList = new List<TravelRequest>();

                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EmployeeID", model.LoginInfo.EmployeeID);
                    param.Add("@OrganisationID", model.LoginInfo.OrganisationID);
                    param.Add("@Year", model.LoginInfo.Year);

                    List<TravelRequest> travelList = SqlMapper.Query<TravelRequest>(connection, "[dbo].[GetAllTravelHistoryList]", param, commandType: CommandType.StoredProcedure).ToList();
                    if (travelList.Count() > 0)
                    {
                        travelRequestList.TravelList = travelList;
                        travelRequestList.IsListAvailiable = true;
                    }
                    else
                    {
                        travelRequestList.TravelList = null;
                        travelRequestList.IsListAvailiable = false;
                        travelRequestList.TravelMessage = "No Travel Request Availiable";
                    }

                    return travelRequestList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddUpdateMobileTravellRequest(Attandance leave)
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
                    parameters.Add("@IsKaaj", true);
                    parameters.Add("@AddedBy", leave.AddedBy);
                    parameters.Add("@UpdatedBy", leave.UpdatedBy);
                    parameters.Add("@Description", leave.Description);
                    parameters.Add("@EntryLocation", "");
                    parameters.Add("@ExitLocation", "");
                    parameters.Add("@AttandanceType", leave.AttandanceType);
                    var savechanges = connection.Execute("[dbo].[AddUpdateOfficialLeaveClient]", parameters, commandType: CommandType.StoredProcedure);
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



        #endregion Client


    }
}
