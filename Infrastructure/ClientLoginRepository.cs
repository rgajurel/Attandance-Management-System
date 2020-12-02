using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DomainInterface;
using System.Data;
using Dapper;

namespace Infrastructure
{
    public class ClientLoginRepository : IClientLoginRepository
    {
        public string loginClient(string email, string password)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@email", email);
                    parameters.Add("@Password", password);
                    parameters.Add("@parentEmail", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
                    connection.Execute("[dbo].[checkParent]", parameters, commandType: CommandType.StoredProcedure);
                    var parentEmail = parameters.Get<dynamic>("@parentEmail");
                    return parentEmail.ToString();
                }

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        
    }
}
