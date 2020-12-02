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
    public class ManageCalendarRepository : IManageCalendarRepository
    {
        public bool AddUpdateManageCalendar(ManageCalendar manageCalendar)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", manageCalendar.ID);
                    parameters.Add("@YearID", manageCalendar.YearID);
                    parameters.Add("@MonthID", manageCalendar.MonthID);
                    parameters.Add("@Days", manageCalendar.Days);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateManageCalendar]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteManageCalendar(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteManageCalendar]", parameters, commandType: CommandType.StoredProcedure);
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

        public ManageCalendar EditManageCalendar(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    ManageCalendar managercalendar = SqlMapper.Query<ManageCalendar>(connection, "[dbo].[EditManageCalendar]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return managercalendar;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ManageCalendar> GetAllManageCalendar()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<ManageCalendar> calendarList = SqlMapper.Query<ManageCalendar>(connection, "[dbo].[GetAllManageCalendar]", commandType: CommandType.StoredProcedure).ToList();

                    return calendarList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
