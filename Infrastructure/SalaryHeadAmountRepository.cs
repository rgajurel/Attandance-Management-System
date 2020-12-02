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

    public class SalaryHeadAmountRepository : ISalaryHeadAmountRepository
    {
        public List<SalaryHeadAmount> GetAllSalaryHeadAmount(SalaryHeadAmount salaryHead)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", salaryHead.OrganisationID);
                    param.Add("@SalaryHeadID", salaryHead.SalaryHeadID);                  

                    List<SalaryHeadAmount> SalaryHeadList = SqlMapper.Query<SalaryHeadAmount>(connection, "[dbo].[GetAllSalaryHeadAmount]", param, commandType: CommandType.StoredProcedure).ToList();
                    return SalaryHeadList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeleteData(SalaryHeadAmount salaryHeadAmount, int salaryHeadID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", salaryHeadAmount.OrganisationID);
                    param.Add("@salaryHeadID", salaryHeadID);
                    
                    var deletesuccess = connection.Execute("[dbo].[DeleteSalaryHead]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SalaryHeadBatchUpload(List<SalaryHeadAmount> salaryHeadAmount, int salaryHeadID)
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
                                DestinationTableName = "[SalaryHeadAmount]",
                                BulkCopyTimeout = 6000,
                                BatchSize = salaryHeadAmount.Count()
                            };
                            var dataTable = GetDataTableSalaryHeadBatchUpload(salaryHeadAmount, salaryHeadID);
                            sqlBulkCopy.WriteToServer(dataTable);
                            scope.Complete();
                            sqlBulkCopy.Close();
                            return salaryHeadAmount.Count();

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

        private DataTable GetDataTableSalaryHeadBatchUpload(List<SalaryHeadAmount> leaveEntry, int salaryHeadID)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("OrganisationID", typeof(int));
            table.Columns.Add("SalaryHeadID", typeof(int));
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("IsAdded", typeof(bool));           
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
          


            leaveEntry.ForEach(data => table.Rows.Add(
                                                    data.ID,
                                                    data.OrganisationID,
                                                    salaryHeadID,
                                                   data.EmployeeID,  
                                                    data.Amount,
                                                    data.IsAdded ,                                
                                                   data.AddedBy,
                                                   DateTime.Now
                                                  

                                                ));
            return table;
        }
    }
}
