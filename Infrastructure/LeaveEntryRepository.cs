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
    public class LeaveEntryRepository : ILeavEntryRepository
    {
        public int DeleteData(LeaveEntry leaveEntry,int year)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();                    
                    param.Add("@OrganisationID", leaveEntry.OrganisationID);
                    param.Add("@LeaveTypeID", leaveEntry.LeaveTypeID);
                    param.Add("@Year", year);
                    var deletesuccess = connection.Execute("[dbo].[DeleteLeaveEntry]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LeaveEntry> GetAllLeaveEntry(LeaveEntry leaveEntry)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();                   
                    param.Add("@OrganisationID", leaveEntry.OrganisationID);
                    param.Add("@LeaveTypeID", leaveEntry.LeaveTypeID);
                    param.Add("@Year", leaveEntry.YearID);

                    List<LeaveEntry> leaveEntryList = SqlMapper.Query<LeaveEntry>(connection, "[dbo].[GetAllLeaveEntry]", param, commandType: CommandType.StoredProcedure).ToList();
                    return leaveEntryList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<LeaveType> leaveTypeOrganisation = SqlMapper.Query<LeaveType>(connection, "[dbo].[GetLeaveTypebasedOnOrganisation]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return leaveTypeOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         
       

        public int LeaveEntryBatchUpload(List<LeaveEntry> leaveEntry,int Year)
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
                                DestinationTableName = "[LeaveEntry]",
                                BulkCopyTimeout = 6000,
                                BatchSize = leaveEntry.Count()
                            };
                            var dataTable = GetDataTableForAttandanceEntry(leaveEntry,Year);
                            sqlBulkCopy.WriteToServer(dataTable);
                            scope.Complete();
                            sqlBulkCopy.Close();
                            return leaveEntry.Count();

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

        private DataTable GetDataTableForAttandanceEntry(List<LeaveEntry> leaveEntry,int Year)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("OrganisationID", typeof(int));
            table.Columns.Add("LeaveTypeID", typeof(int));
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("TotalDays", typeof(int));
            table.Columns.Add("TotalDayInMonth", typeof(float));
            table.Columns.Add("IsMonthRule", typeof(bool));
            table.Columns.Add("YearID", typeof(int));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));          

       
            leaveEntry.ForEach(data => table.Rows.Add(
                                                    data.ID,
                                                    data.OrganisationID
                                                  , data.LeaveTypeID
                                                  , data.EmployeeID
                                                  , data.TotalDays
                                                  ,data.TotalDayInMonth
                                                  ,data.IsMonthRule,
                                                    Year
                                                  , data.AddedBy
                                                  , DateTime.Now
                                                  , data.UpdatedBy
                                                  , data.UpdatedOn                                            

                                                ));
            return table;
        }
    }
}
