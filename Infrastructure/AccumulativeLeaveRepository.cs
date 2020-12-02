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
    public class AccumulativeLeaveRepository : IAccumulativeLeaveRepository
    {
        public List<Employee> GetAllEmployee(string prefix,int organisation)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@prefix", prefix);
                    param.Add("@organisation", organisation);
                    List<Employee> employeesList = SqlMapper.Query<Employee>(connection, "[dbo].[GetAllEmployeesAutoComplete]", param, commandType: CommandType.StoredProcedure).ToList();
                    return employeesList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<LeaveType> leaveTypeOrganisation = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetLeaveTypebasedOnOrganisation]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return leaveTypeOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddUpdateAccumulativeLeave(AccumulativeLeave accumulative)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", accumulative.ID);
                    parameters.Add("@OrganisationID", accumulative.OrganisationID);
                    parameters.Add("@EmployeeID", accumulative.EmployeeID);
                    parameters.Add("@LeaveTypeID", accumulative.LeaveTypeID);
                    parameters.Add("@UserID", accumulative.UserID);
                    parameters.Add("@Days", accumulative.Days);
                    parameters.Add("@YearID", accumulative.YearID);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateAccumulativeLeave]", parameters, commandType: CommandType.StoredProcedure);
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

        public List<AccumulativeLeave> GetAllAccumulativeLeave()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<AccumulativeLeave> AccumulativeleaveList = SqlMapper.Query<AccumulativeLeave>(connection, "[dbo].[GetAllAccumulativeLeave]", param, commandType: CommandType.StoredProcedure).ToList();

                    return AccumulativeleaveList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public AccumulativeLeave EditAccumulativeLeave(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    AccumulativeLeave accumulativeEdit = SqlMapper.Query<AccumulativeLeave>(connection, "[dbo].[EditAccumulativeLeave]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return accumulativeEdit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
