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
    public class CommonRepository : ICommonRepository
    {
        public LoginDetails GetLoginInfo(string key)
        {
            try
            {
                var keys = key.ToUpper();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@DeviceKey",keys);                   
                    LoginDetails userLoginInfo = SqlMapper.Query<LoginDetails>(connection, "[dbo].[GetUserLoginInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return userLoginInfo;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
