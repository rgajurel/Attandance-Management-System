using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Transactions;

namespace Infrastructure
{
    public class SalaryHeadingSettingsRepository : ISalaryHeadSettingsRepository
    {
        public bool AddUpdateSalaryHeading(SalaryHeadingSettings salaryHeadingSettings)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSalaryHeadingSettings(int id)
        {
            throw new NotImplementedException();
        }

        public SalaryHeadingSettings EditSalaryHeadingSettings(int id)
        {
            throw new NotImplementedException();
        }

        public List<SalaryHeadingSettings> GetAllSalaryHeadingSettings(SalaryHeadingSettings salaryHeadingSettings)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@JobTypeID", salaryHeadingSettings.JobTypeID);                 
                 

                    List<SalaryHeadingSettings> salarySettingHeadingList = SqlMapper.Query<SalaryHeadingSettings>(connection, "[dbo].[GetAllSalaryHeadingSettings]", param, commandType: CommandType.StoredProcedure).ToList();

                    return salarySettingHeadingList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public int DeleteData(SalaryHeadingSettings salaryHeadingsett)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@JobTypeID", salaryHeadingsett.JobTypeID);                 
                   
                    var deletesuccess = connection.Execute("[dbo].[DeleteSalaryHeadingsByJobType]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SalaryHeadingsSettingsBatchUpload(List<SalaryHeadingSettings> salHeadSettings)
        {
            try
            {

                using (SqlConnection connection = DBManager.DbConnect1())
                {
                    connection.Open();

                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            var sqlBulkCopy = new SqlBulkCopy(connection)
                            {
                                DestinationTableName = "[SalaryHeadingSettings]",
                                BulkCopyTimeout = 6000,
                                BatchSize = salHeadSettings.Count()
                            };
                            var dataTable = GetDataTableForEmployees(salHeadSettings);
                            sqlBulkCopy.WriteToServer(dataTable);
                            scope.Complete();
                            sqlBulkCopy.Close();
                            return salHeadSettings.Count();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetDataTableForEmployees(List<SalaryHeadingSettings> salHeadSettings)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
           
            table.Columns.Add("HeadName", typeof(string));

            table.Columns.Add("IsAdd", typeof(bool));
            table.Columns.Add("IsSaving", typeof(bool));
            table.Columns.Add("IsTax", typeof(bool));
            table.Columns.Add("SortOrder", typeof(int));
            table.Columns.Add("IsSalaryCalculatePoint", typeof(bool));
            table.Columns.Add("JobTypeID", typeof(int));         
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));


            // note : the order of the field is very important
            // and should be same as the defined in table structure.
            salHeadSettings.ForEach(data => table.Rows.Add(
                                               data.ID,     
                                               data.HeadName,
                                               data.IsAdd,
                                               data.IsSaving,
                                               data.IsTax,
                                               data.SortOrder,
                                               data.IsSalaryCalculatePoint,
                                               data.JobTypeID,                                         
                                               data.AddedBy,
                                               DateTime.Now,
                                               data.UpdatedBy,
                                               data.UpdatedOn                                             
                                                ));
            return table;
        }
    }
}
