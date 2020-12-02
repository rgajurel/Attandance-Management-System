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
    public class MonthsRepository : IMonthsRepository
    {
        public bool AddUpdateMonthsInfo(Months monthinfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", monthinfo.ID);
                    parameters.Add("@Month", monthinfo.Month);
                    parameters.Add("@IsActive", monthinfo.IsActive);
                    parameters.Add("@AddedBy", monthinfo.AddedBy);
                    parameters.Add("@UpdatedBy", monthinfo.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateMonthInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteMonthsInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteMonthInfo]", parameters, commandType: CommandType.StoredProcedure);
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

        public Months EditMonthsInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Months monthinfo = SqlMapper.Query<Months>(connection, "[dbo].[EditMonthInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return monthinfo;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Months> GetAllMonthsInfo()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<Months> monthinfoList = SqlMapper.Query<Months>(connection, "[dbo].[GetAllMonthInfo]", commandType: CommandType.StoredProcedure).ToList();

                    return monthinfoList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
