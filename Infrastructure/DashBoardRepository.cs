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
    public class DashBoardRepository : IDashBoardRepository
    {
        public IQueryable<Holidays> GetAllOrganisationHolidaysList()
        {
            try
            {
                
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", 18);
                    var holidaysList = SqlMapper.Query<Holidays>(connection, "[dbo].[GetAllHolidaysList]", param, commandType: CommandType.StoredProcedure).AsQueryable();
                   
                    return holidaysList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IQueryable<DailyCount> GetAllDailyAttandanceCount()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())                {
                   
                   
                    var organisationEventListDashBoard = SqlMapper.Query<DailyCount>(connection, "[dbo].[StudentsDailyAttandanceTotal]", commandType: CommandType.StoredProcedure).AsQueryable();
                    return organisationEventListDashBoard;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IQueryable<OrganisationEvents> GetAllOrganisationEvents()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);                 
                   var organisationEventListDashBoard = SqlMapper.Query<OrganisationEvents>(connection, "[dbo].[GetAllOrganisationDashBoard]",param, commandType: CommandType.StoredProcedure).AsQueryable();
                    return organisationEventListDashBoard;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IQueryable<StudentTotalByClass> GetAllTotalStudentbyClass()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {                                    
                    var totalStudentByClass = SqlMapper.Query<StudentTotalByClass>(connection, "[dbo].[GetAllStudentsByClass]", commandType: CommandType.StoredProcedure).AsQueryable();
                    return totalStudentByClass;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IQueryable<BirthDayEvents> GetAllUpcomingBirthdays()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    var organisationEventListDashBoard = SqlMapper.Query<BirthDayEvents>(connection, "[dbo].[EmployeeComingBirthDays]", param, commandType: CommandType.StoredProcedure).AsQueryable();
                    return organisationEventListDashBoard;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
