using Dapper;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ChangePasswordRepository : IChangePasswordRepository
    {
        public bool changePassword(string email,string oldPassword, string newPassword)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@email", email);
                    parameters.Add("@Password", newPassword);
                    parameters.Add("@oldPassword",oldPassword);
                    var savechanges = connection.Execute("[dbo].[changePasswordClient]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges>0)
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

        public bool checkUser(string email, string oldPassword)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@email", email);
                    parameters.Add("@Password", oldPassword);
                    parameters.Add("@status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[checkClientUser]", parameters, commandType: CommandType.StoredProcedure);
                    var status = parameters.Get<Boolean>("@status");
                    if (status)
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
