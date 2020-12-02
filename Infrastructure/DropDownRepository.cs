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
    public class DropDownRepository : IDropDownRepository
    {
        public List<DropDownCommon> GetSchoolTypeDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> schoolTypeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetSchoolTypeList]", commandType: CommandType.StoredProcedure).ToList();

                    return schoolTypeList;
                }
            }
            catch (Exception ex)
            {
              return null;
            }
        }

        public List<DropDownCommon> GetAllSalaryHead()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> salaryHeadList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetSalaryHeadList]", commandType: CommandType.StoredProcedure).ToList();

                    return salaryHeadList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetClassTypeDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> classTypeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetClassTypeList]", commandType: CommandType.StoredProcedure).ToList();

                    return classTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetSectionDropDown()
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> sectionList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetSectionList]", commandType: CommandType.StoredProcedure).ToList();

                    return sectionList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetFacultyDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> facultyList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetFacultyList]", commandType: CommandType.StoredProcedure).ToList();

                    return facultyList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetDocumentsDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> facultyList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetStudentsDocs]", commandType: CommandType.StoredProcedure).ToList();

                    return facultyList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetStudentsCategoryDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> facultyList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetStudentsCategoryList]", commandType: CommandType.StoredProcedure).ToList();

                    return facultyList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetReligionDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> facultyList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetRelegionList]", commandType: CommandType.StoredProcedure).ToList();

                    return facultyList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetCasteDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> facultyList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetCasteList]", commandType: CommandType.StoredProcedure).ToList();

                    return facultyList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetClasswDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> classTypeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetClassList]", commandType: CommandType.StoredProcedure).ToList();

                    return classTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetSessionDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> classTypeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetSessionList]", commandType: CommandType.StoredProcedure).ToList();

                    return classTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetBloodGroupDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> classTypeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetBloodGroupList]", commandType: CommandType.StoredProcedure).ToList();

                    return classTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetHouseDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> classTypeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetHouseList]", commandType: CommandType.StoredProcedure).ToList();

                    return classTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetErrorList()
        {
            var listdata = new DropDownCommon()
            {  ID=-1,
                Name = "Error",
            };
            var errorlist = new List<DropDownCommon>();
            errorlist.Add(listdata);
            return errorlist;
        }

        public List<DropDownCommon> GetActiveSessionDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> activeSession = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetActiveSessionList]", commandType: CommandType.StoredProcedure).ToList();

                    return activeSession;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetTermDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> termList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetTerm]", commandType: CommandType.StoredProcedure).ToList();

                    return termList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetAllOrganisation()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin",new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@IsClient", new LoginUser().IsClient);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<DropDownCommon> termList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetAllOrganisation]",param, commandType: CommandType.StoredProcedure).ToList();

                    return termList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetJobTypeDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> termList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetJobType]", commandType: CommandType.StoredProcedure).ToList();

                    return termList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetAllLeaveType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> termList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetLeaveType]", commandType: CommandType.StoredProcedure).ToList();

                    return termList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetMonthDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> monthList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetMonthList]", commandType: CommandType.StoredProcedure).ToList();

                    return monthList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetAllMonthDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<DropDownCommon> monthList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetAllMonthList]", commandType: CommandType.StoredProcedure).ToList();

                    return monthList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetTypeDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    DynamicParameters param = new DynamicParameters();                  
                    List<DropDownCommon> typeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetFeeTypeList]", param, commandType: CommandType.StoredProcedure).ToList();

                    return typeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DropDownCommon> GetPersonnelTypeDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    DynamicParameters param = new DynamicParameters();
                    List<DropDownCommon> typeList = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetPersonnelFeeTypeList]", param, commandType: CommandType.StoredProcedure).ToList();

                    return typeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UserGroup> GetUserGroup()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    var userGroups = SqlMapper.Query<UserGroup>(
                                      connection, "[dbo].[GetAllUserGroupForDropDown]", commandType: CommandType.StoredProcedure).ToList();

                    return userGroups;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<Role> RoleGet(int? roleID = default(int?))
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@roleID", roleID);
                    var returnType = SqlMapper.Query<Role>(
                                      connection, "GetAllRole", param, commandType: CommandType.StoredProcedure).ToList();
                    return returnType;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<NotificationTypes> GetNotificationTypes()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    var notificationTypes = SqlMapper.Query<NotificationTypes>(
                                      connection, "[dbo].[GetAllNotificationTypesForDropDown]", commandType: CommandType.StoredProcedure).ToList();

                    return notificationTypes;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownCommon> GetTakeLeaveDaysMaster()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    var leavedaysmaster = SqlMapper.Query<DropDownCommon>(
                                      connection, "[dbo].[GetAllleaveDaysMasterWithValue]", commandType: CommandType.StoredProcedure).ToList();

                    return leavedaysmaster;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownCommon> GetLoginEmployeeName()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", new LoginUser().LoggedInuserID); 

                    var loginFullName = SqlMapper.Query<DropDownCommon>(
                                      connection, "[dbo].[GetLoginEmployeeName]",param, commandType: CommandType.StoredProcedure).ToList();

                    return loginFullName;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownCommon> GetLeaveTypeBasedOnEmployee(string employeeid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EmployeeID",employeeid );

                    var loginFullName = SqlMapper.Query<DropDownCommon>(
                                      connection, "[dbo].[GetLeaveTypeForEmployee]", param, commandType: CommandType.StoredProcedure).ToList();

                    return loginFullName;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownCommon> GetSuperAdminAndAdminNames()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parm = new DynamicParameters();
                    parm.Add("@ID",new LoginUser().LoggedInuserID);
                     var adminorSuperAdminName = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetAllAdminAndSuperAdmin]",parm,commandType: CommandType.StoredProcedure).ToList();

                    return adminorSuperAdminName;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownCommon> GetAllLanguage()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    var languageList = SqlMapper.Query<Language>(connection, "[dbo].[GetAllLanguage]",param, commandType: CommandType.StoredProcedure).ToList().Select(x => new DropDownCommon()
                    {
                        Name = x.Name,
                        ID = x.ID
                    }).ToList(); ;
                    return languageList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<DropDownCommon> GetSalartTypeDropDown()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    var salarttype = SqlMapper.Query<DropDownCommon>(
                                      connection, "[dbo].[GetAlllSalarySavingType]", commandType: CommandType.StoredProcedure).ToList();
                    return salarttype;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region mobile
        public List<DropDownCommon> GetSuperAdminAndAdminNames(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parm = new DynamicParameters();
                    parm.Add("@ID", id);
                    var adminorSuperAdminName = SqlMapper.Query<DropDownCommon>(connection, "[dbo].[GetAllAdminAndSuperAdmin]", parm, commandType: CommandType.StoredProcedure).ToList();

                    return adminorSuperAdminName;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DropDownCommon> GetLeaveTypeBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                   var leaveTypeOrganisation = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetAttandanceLeave]", param, commandType: CommandType.StoredProcedure).ToList().Select(x=>new DropDownCommon()
                    {
                        Name=x.LeaveTypeName,
                        ID=x.ID
                    }).ToList(); ;
                    return leaveTypeOrganisation;
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

