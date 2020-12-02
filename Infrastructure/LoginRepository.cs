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
    public class LoginRepository : ILoginRepository
    {
        public string GetUserImage(double phoneno)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@PhoneNo", phoneno);
                    Image image = SqlMapper.Query<Image>(connection, "[dbo].[GetLoggedInUserImageByPhoneNo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return image.UserImage;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GetUserImage(string employeeid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                   parameters.Add("@EmployeeID", employeeid);                

                    Image userImage = SqlMapper.Query<Image>(connection, "[dbo].[GetLoggedInUserImageByemployeeID]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return userImage.UserImage;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
