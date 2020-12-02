using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;

namespace Infrastructure
{
    public class TakeAccumulativeLeaveRepository : ITakeAccumulativeLeaveRepository
    {
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
                    TakeLeave remainingleave = SqlMapper.Query<TakeLeave>(connection, "[dbo].[CalculateRemainingTakeLevaeAccumulative]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return remainingleave.TotalLeaveTaken;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TakeLeave> GetAllAccumulativeLeave(TakeLeave search)
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

                    List<TakeLeave> employeeList = SqlMapper.Query<TakeLeave>(connection, "[dbo].[GetAllTakeLeaveAccumulative]", param, commandType: CommandType.StoredProcedure).ToList();

                    return employeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddUpdateTakeAccumulativeLeave(TakeLeave leave)
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
                    parameters.Add("@IsAccumulative", true);

                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateTakeAccumulativeLeave]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteTakeAccumulativeLeave(int id)
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
    }
}
