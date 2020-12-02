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
    public class UserGroupRepository : IUserGroupRepository
    {
        public bool AddUpdateUserGroup(UserGroup usergroup)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", usergroup.ID);
                    parameters.Add("@GroupName", usergroup.GroupName);
                    parameters.Add("@OrganisationID", usergroup.OrganisationID);
                    parameters.Add("@StatusValue", usergroup.StatusValue);
                    parameters.Add("@AddedBy",new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateUserGroup]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteUserGroup(int ID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", ID);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteUserGroup]", parameters, commandType: CommandType.StoredProcedure);
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

        public UserGroup EditUserGroup(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    UserGroup userGroup = SqlMapper.Query<UserGroup>(connection, "[dbo].[EditUserGroup]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return userGroup;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<UserGroup> GetAllUserGroup(UserGroup usergroup)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@GroupName", usergroup.GroupName==null? "":usergroup.GroupName);
                    param.Add("@StatusValue",usergroup.StatusValue);
                    param.Add("@OrganisationID", usergroup.OrganisationID);
                    param.Add("@IsAdmin",new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    param.Add("@offset", usergroup.offset);
                    param.Add("@PageSize", usergroup.pageSize);                 

                    List<UserGroup> userGroupList = SqlMapper.Query<UserGroup>(connection, "[dbo].[GetAllUserGroup]", param, commandType: CommandType.StoredProcedure).ToList();

                    return userGroupList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public UserGroup GetUserGroup(string usergroup)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@usergroup", usergroup);
                    UserGroup usergroups = SqlMapper.Query<UserGroup>(connection, "[dbo].[GetUserGroup]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;
                    return usergroups;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
