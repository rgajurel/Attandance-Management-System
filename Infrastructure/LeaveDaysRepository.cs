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
    public class LeaveDaysRepository : ILeaveDaysRepository
    {
        public bool AddUpdateLeaveType(LeaveDays leaveDays)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", leaveDays.ID);
                    parameters.Add("@Name", leaveDays.Name);
                    parameters.Add("@Weight", leaveDays.Weight);
                    parameters.Add("@AddedBy", leaveDays.AddedBy);
                    parameters.Add("@UpdatedBy", leaveDays.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateLeaveDays]", parameters, commandType: CommandType.StoredProcedure);
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
                    connection.Execute("[dbo].[DeleteLeaveDays]", parameters, commandType: CommandType.StoredProcedure);
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

        public LeaveDays EditLeaveType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    LeaveDays leaveDaysEdit = SqlMapper.Query<LeaveDays>(connection, "[dbo].[EditLeaveDays]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return leaveDaysEdit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<LeaveDays> GetAllLeaveType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<LeaveDays> leaveDaysList = SqlMapper.Query<LeaveDays>(connection, "[dbo].[GetAllLeaveDays]", commandType: CommandType.StoredProcedure).ToList();
                    return leaveDaysList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
