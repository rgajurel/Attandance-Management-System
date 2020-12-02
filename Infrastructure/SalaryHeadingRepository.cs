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
    public class SalaryHeadingRepository : ISalaryHeadRepository
    {
        public bool AddUpdateSalaryHeading(SalaryHeading salaryHeading)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", salaryHeading.ID);
                    parameters.Add("@HeadName", salaryHeading.HeadName);
                    parameters.Add("@IsAdd", salaryHeading.IsAdd);
                    parameters.Add("@IsSaving", salaryHeading.IsSaving);
                    parameters.Add("@SortOrder", salaryHeading.SortOrder);
                    parameters.Add("@IsTax", salaryHeading.IsTax);
                    parameters.Add("@IsBasicSalary", salaryHeading.IsBasicSalary);
                    parameters.Add("@IsSalaryCalculatePoint", salaryHeading.IsSalaryCalculatePoint);
                    parameters.Add("@AddedBy", salaryHeading.AddedBy);
                    parameters.Add("@UpdatedBy", salaryHeading.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateSalaryHeadings]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteSalaryHeading(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteSalaryHeading]", parameters, commandType: CommandType.StoredProcedure);
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

        public SalaryHeading EditSalaryHeading(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    SalaryHeading SalaryHeading = SqlMapper.Query<SalaryHeading>(connection, "[dbo].[EditSalaryHeading]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return SalaryHeading;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SalaryHeading> GetAllSalaryHeading()
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<SalaryHeading> departmentList = SqlMapper.Query<SalaryHeading>(connection, "[dbo].[GetAllSalaryHeading]", commandType: CommandType.StoredProcedure).ToList();

                    return departmentList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
