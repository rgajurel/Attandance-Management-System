using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using Dapper;
using System.Data;

namespace Infrastructure
{
    public class YearlyHolidaysEntryRepository : IYearlyHolidaysEntryRepository
    {
        public bool AddUpdateYearlyHolidaysEntry(YearlyHolidaysEntry yearlyHoliday)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", yearlyHoliday.ID);
                    parameters.Add("@Title", yearlyHoliday.Title);
                    parameters.Add("@OrganisationID", yearlyHoliday.OrganisationID);
                    parameters.Add("@Date", yearlyHoliday.Date);
                    parameters.Add("@NepaliDate", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(yearlyHoliday.Date));
                    parameters.Add("@YearID", yearlyHoliday.YearID);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy",new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateYearlyHoliday]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteYearlyHolidaysEntry(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteYearlyHoliday]", parameters, commandType: CommandType.StoredProcedure);
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

        public YearlyHolidaysEntry EditYearlyHolidaysEntry(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    YearlyHolidaysEntry Department = SqlMapper.Query<YearlyHolidaysEntry>(connection, "[dbo].[EditYearlyHoliday]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return Department;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<YearlyHolidaysEntry> GetAllYearlyHolidaysEntry()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<YearlyHolidaysEntry> yearlyHolidaysList = SqlMapper.Query<YearlyHolidaysEntry>(connection, "[dbo].[GetAllYearlyHolidays]", param, commandType: CommandType.StoredProcedure).ToList();
                    return yearlyHolidaysList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
