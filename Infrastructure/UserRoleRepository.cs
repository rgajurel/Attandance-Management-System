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
  public class UserRoleRepository:IUserRole
    {
        #region RoleGet
        public IEnumerable<Role> RoleGet(int? roleID = null)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleID", roleID);
                    var returnType = SqlMapper.Query<Role>(
                                      connection, "usp_UserRoleGet", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region RoleMenuGet
        public IEnumerable<MenuRole> RoleMenuGet(int? roleID = null)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleID", roleID);
                    string cond = roleID > 0 ? "where R.RoleID=" + roleID : string.Empty;
                    var returnType = connection.Query<MenuRole>(
                                      $@"SELECT R.Name,MR.Access Options,M.Name MenuName,R.RoleID,M.MenuID FROM dbo.UserRole R
                                            LEFT JOIN dbo.MenuRole MR ON R.RoleID = MR.RoleID
                                            LEFT JOIN dbo.Menu M ON M.MenuID = MR.MenuID {cond}").ToList();

                    return returnType;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region RoleMenuSave
        public ReturnType RoleMenuSave(Role oRole, string xml, string userName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@RoleID", oRole.RoleID);
                    param.Add("@RoleName", oRole.Name);
                    param.Add("@xml", xml);
                    param.Add("@userName", userName);
                    var returnType = SqlMapper.Query<ReturnType>(
                                     connection, "usp_UserRoleMenuSave", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
                    return returnType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region RoleSave
        //public ReturnType RoleSave(Role oRole, string userName)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = DBManager.DbConnect())
        //        {
        //            DynamicParameters param = new DynamicParameters();
        //            param.Add("@name", oRole.Name);
        //            param.Add("@remarks", oRole.Remarks);
        //            param.Add("@userName", userName);
        //            var returnType = SqlMapper.Query<ReturnType>(
        //                             connection, "usp_RoleSave", param, commandType: CommandType.StoredProcedure).ToList()?.FirstOrDefault() ?? null;
        //            return returnType;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
        #region MenuGet
        public IEnumerable<Menu> MenuGet(bool isAdmin)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@isAdmin", isAdmin);
                    //var returnType = SqlMapper.Query<Menu>(
                    //                  connection, "usp_MenuGet", param, commandType: CommandType.StoredProcedure).ToList();
                    var returnType = SqlMapper.Query<Menu>(
                                    connection, string.Format("select * from Menu where IsAdmin='{0}' order by ParentID,OrderBy", isAdmin));
                    return returnType;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Menu> MenuGet(int roleID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    //DynamicParameters param = new DynamicParameters();
                    //param.Add("@isAdmin", isAdmin);
                    //var returnType = SqlMapper.Query<Menu>(
                    //                  connection, "usp_MenuGet", param, commandType: CommandType.StoredProcedure).ToList();
                    var returnType = SqlMapper.Query<Menu>(
                                    connection, string.Format("SELECT M.*,isnull(m1.Name,'') as ParentName FROM dbo.MenuRole MR LEFT JOIN Menu M ON MR.MenuID=M.MenuID left JOIN Menu m1 on m1.MenuID = m.ParentID WHERE MR.RoleID={0} order by OrderBy", roleID));
                    return returnType;

                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Menu> MenuGetBasedOnLoggedInUserRole(string userName)
        {
            try
            {
                //using (IDbConnection connection = DBManager.DbConnect())
                //{
                //    DynamicParameters param = new DynamicParameters();
                //    param.Add("@userName", userName);
                //    var menusForUser = SqlMapper.Query<Menu>(
                //                      connection, "[dbo].[usp_MenuGetBasedOnUser]", param, commandType: CommandType.StoredProcedure).ToList();

                //    return menusForUser;

                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@userName", userName);
                    var menusForUser = SqlMapper.Query<Menu>(
                                      connection, "[dbo].[usp_MenuExternalGetBasedOnUser]", param, commandType: CommandType.StoredProcedure);
                    List<Menu> menulist = new List<Menu>();
                    List<string> name = new List<string>();
                    foreach (var item in menusForUser)
                    {
                        if (menusForUser.Where(x => x.MenuID == item.MenuID).Select(x => x.Name).ToList().Count > 1)
                        {
                            if (menulist.Where(x => x.MenuID == item.MenuID).ToList().Count < 1)
                            {
                                foreach (var r in menusForUser.Where(x => x.MenuID == item.MenuID).Select(x => x.Access).ToList())
                                {
                                    name.Add(r);
                                }
                                if (name.Count > 0)
                                {
                                    string names = string.Join(",", name);
                                    List<string> result = names.Split(',').Distinct().ToList();
                                    menulist.Add(new Menu() { MenuID = item.MenuID, isAdmin = item.isAdmin, Name = item.Name, ParentName = item.ParentName, URI = item.URI, Slug = item.Slug, IconClass = item.IconClass, Access = string.Join(",", result) });
                                    name.Clear();
                                }
                            }
                        }
                        else
                        {
                            menulist.Add(new Menu() { MenuID = item.MenuID, Name = item.Name, ParentName = item.ParentName, isAdmin = item.isAdmin, URI = item.URI, Slug = item.Slug, IconClass = item.IconClass, Access = item.Access });
                        }
                    }
                    return menulist;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
