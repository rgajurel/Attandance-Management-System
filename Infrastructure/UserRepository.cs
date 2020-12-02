using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using DomainEntities.Mobile;

namespace Infrastructure
{
  public  class UserRepository : IUserRepository
    {
        public bool AddUpdateUser(User user)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", user.ID);
                    parameters.Add("@IsSuperAdmin", user.IsSuperAdmin);
                    parameters.Add("@IsAdmin", user.IsAdmin);
                    parameters.Add("@IsClientUser", user.IsClientUser);
                    parameters.Add("@IsStudentUser", user.IsStudentUser);
                    parameters.Add("@IsParentUser", user.IsParentUser);
                    parameters.Add("@OrganisationID", user.OrganisationID);
                    parameters.Add("@Name", user.Name);
                    parameters.Add("@Email", user.Email);
                    parameters.Add("@UserName", user.UserName);
                    parameters.Add("@Password", user.Password);
                    parameters.Add("@ConformPassword", user.ConformPassword);
                    parameters.Add("@UserID", user.UserID);
                    parameters.Add("@EmployeeID", user.EmployeeID);
                    parameters.Add("@Status", user.Status);
                    parameters.Add("@UserGroupID", user.UserGroupID);
                    parameters.Add("@RoleID", user.RoleID);
                    parameters.Add("@AddedBy", user.AddedBy);
                   
                    var savechanges = connection.Execute("[dbo].[AddUpdateUser]", parameters, commandType: CommandType.StoredProcedure);
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


        public bool ChekUserAlreadyExist(User user)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();

                    
                    parameters.Add("@EmployeeID", user.EmployeeID);
                    
                    var count = SqlMapper.Query<int>(connection, "[dbo].[CheckUserExist]", parameters, commandType: CommandType.StoredProcedure).ToList();

                    if (count[0] > 0)
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
        public bool DeleteUser(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteUser]", parameters, commandType: CommandType.StoredProcedure);
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
        public bool ChangePassword(ChangePassword changePassword,string mobile)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                                       
                    parameters.Add("@oldPassword", Crypto.OneWayEncryter(changePassword.oldPassword));
                    parameters.Add("@newPassword", Crypto.OneWayEncryter(changePassword.newPassword));
                    parameters.Add("@ID", mobile==null?new LoginUser().LoggedInuserID:mobile);

                    var savechanges = connection.Execute("[dbo].[ChangePassword]", parameters, commandType: CommandType.StoredProcedure);
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
        public List<Parents> GetAllParents(string prefix)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@prefix", prefix);                   
                    List<Parents> parentList = SqlMapper.Query<Parents>(connection, "[dbo].[GetAllParentsAutoComplete]", param, commandType: CommandType.StoredProcedure).ToList();
                    return parentList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<User> GetAllUsers(User search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);
                    param.Add("@PageSize", search.pageSize);                  
                    param.Add("@UserNameSearch", search.UserNameSearch == null ? " " : search.UserNameSearch);
                    param.Add("@NameSearch", search.NameSearch==null?" ":search.NameSearch);
                    param.Add("@SearchStatus", search.SearchStatus);
                    param.Add("@OrganisationIDSearch", search.OrganisationIDSearch);
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID); 
                    //param.Add("@SearchParameter", iNotification.searchParam);
                    List<User> userList = SqlMapper.Query<User>(connection, "[dbo].[GetAllUsers]", param, commandType: CommandType.StoredProcedure).ToList();
                                     

                    return userList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Students> GetAllStudents(string prefix)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@prefix", prefix);                   
                    List<Students> allStudents = SqlMapper.Query<Students>(connection, "[dbo].[GetAllStudentsAutoComplete1]", param, commandType: CommandType.StoredProcedure).ToList();
                    return allStudents;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User GetUserName(string username)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@username", username);
                    User UserName = SqlMapper.Query<User>(connection, "[dbo].[GetUserName]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;
                    return UserName;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User EditUser(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    User userEdit = SqlMapper.Query<User>(connection, "[dbo].[EditUserInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return userEdit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User GetUserByUserNameAndPassword(string UserName, string Password,string identifier,DeviceType deviceType)
        {
            try
            {
                Guid devicetoken = Guid.NewGuid();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserName", UserName);
                    parameters.Add("@Password", Password);                   
                    User userEdit = SqlMapper.Query<User>(connection, "[dbo].[GetUserByUserNameAndPassword]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (userEdit != null && deviceType!=DeviceType.Web)
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@EmployeeID", userEdit.EmployeeID);
                        param.Add("@Identifier", identifier);
                        param.Add("@DeviceAuthToken",devicetoken);
                        param.Add("@RefreshToken", Guid.NewGuid());
                        param.Add("@CreatedDate", DateTime.Now);
                        param.Add("@DeviceAuthTokenExpiryDate", DateTime.Now.AddHours(3));
                        param.Add("@IsActive", true);
                        connection.Execute("[dbo].[AddUserDeviceKey]", param, commandType: CommandType.StoredProcedure);
                        userEdit.DeviceAuthToken = Crypto.Encrypt(devicetoken.ToString());
                    }
                   
                    return userEdit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
       public UserProfileInfo GetUserProfileInfo(string employeeid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@EmployeeID", employeeid);
                    UserProfileInfo userEdit = SqlMapper.Query<UserProfileInfo>(connection, "[dbo].[GetUserInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return userEdit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
