using Dapper;
using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
   public class TakeAdvanceRepository:ITakeAdvanceRepository
    {
        public bool AddUpdateTakeAdvance(TakeAdvance takeAdvance)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", takeAdvance.ID);
                    parameters.Add("@OrganisationID", takeAdvance.OrganisationID);
                    parameters.Add("@EmployeeID", takeAdvance.EmployeeID);
                    parameters.Add("@Year", takeAdvance.Year);
                    parameters.Add("@Month", takeAdvance.Month);
                    parameters.Add("@NepaliDate",DateConversionHelper.GetEnglsihTimeToNepaliDateTime(takeAdvance.Date));
                    parameters.Add("@Date", takeAdvance.Date);
                    parameters.Add("@Amount", takeAdvance.Amount);               

                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateTakeAdvance]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteTakeadvance(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteTakeAdvance]", parameters, commandType: CommandType.StoredProcedure);
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

        public TakeAdvance EditTakeAdvance(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    TakeAdvance editTakeAdvance = SqlMapper.Query<TakeAdvance>(connection, "[dbo].[EditTakeAdvance]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return editTakeAdvance;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TakeAdvance> GetAllTakeAdvance()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                  
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);                 

                    List<TakeAdvance> userGroupList = SqlMapper.Query<TakeAdvance>(connection, "[dbo].[GetAllTakeAdvance]", param, commandType: CommandType.StoredProcedure).ToList();

                    return userGroupList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
