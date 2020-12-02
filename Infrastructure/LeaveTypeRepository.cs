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
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        public bool AddUpdateLeaveType(LeaveType leaveType)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", leaveType.ID);
                    parameters.Add("@LeaveTypeName", leaveType.LeaveTypeName);
                    parameters.Add("@IsAccumulativeLeave", leaveType.IsAccumulativeLeave);
                    parameters.Add("@IsAttandanceLeave", leaveType.IsAttandanceLeave);
                    parameters.Add("@IsExpireLeave", leaveType.IsExpireLeave);
                    parameters.Add("@OrganisationID", leaveType.OrganisationID);

                    parameters.Add("@AddedBy", leaveType.AddedBy);
                    parameters.Add("@UpdatedBy", leaveType.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateLeaveType]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteLeaveType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteLeaveType]", parameters, commandType: CommandType.StoredProcedure);
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

        public LeaveType EditLeaveType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    LeaveType leaveType = SqlMapper.Query<LeaveType>(connection, "[dbo].[EditLeaveType]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return leaveType;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<LeaveType> GetAllLeaveType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<LeaveType> leavetypeList = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetAllLeave]", commandType: CommandType.StoredProcedure).ToList();

                    return leavetypeList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
