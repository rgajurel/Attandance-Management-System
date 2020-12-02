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
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        public int ForgotPassword(ForgotPassword fw)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@email", fw.email);
                    parameters.Add("@student", fw.studentName);
                    parameters.Add("@id", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[verifyParent]", parameters, commandType: CommandType.StoredProcedure);
                    var status = parameters.Get<bool>("@id");
                    if (status)
                    {

                        return 1;
                    }
                    else
                    {
                        return 0;
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
