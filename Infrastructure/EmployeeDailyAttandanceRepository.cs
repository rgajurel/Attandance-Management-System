using Dapper;
using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure
{
   public class EmployeeDailyAttandanceRepository: IEmployeeDailyAttandanceRepository
    {
        public List<EmployeeDailyAttandance> GetDailyAttandance(EmployeeDailyAttandance search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", search.OrganisationID);
                    param.Add("@Month", search.Month);
                    param.Add("@Year", search.Year);
                    param.Add("@LeaveDaysID", search.LeaveDaysID);
                    param.Add("@DateFrom", search.DateFrom);
                    param.Add("@NepaliDateFrom", search.NepaliDateFrom.ToShortDateString());
                    param.Add("@DateTo", search.DateTo);
                    param.Add("@NepaliDateTo", search.NepaliDateTo.ToShortDateString());
                    List<EmployeeDailyAttandance> employeeDailyAttandanceList = SqlMapper.Query<EmployeeDailyAttandance>(connection, "[dbo].[GetAllEmployeeDailyAttandance]", param, commandType: CommandType.StoredProcedure).ToList();
                    return employeeDailyAttandanceList;
                }
            }
            catch (Exception ex)
            {
                throw null;
                // return null;
            }
        }

        public int DeleteData(EmployeeDailyAttandance employeeDailyAttandance)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", employeeDailyAttandance.OrganisationID);
                    param.Add("@Year", employeeDailyAttandance.Year);
                    param.Add("@Month", employeeDailyAttandance.Month);
                    param.Add("@DateFrom", employeeDailyAttandance.DateFrom);                   
                    var deletesuccess = connection.Execute("[dbo].[DeleteEmployeeDailyAttandance]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AttandanceEntryBatchUpload(List<EmployeeDailyAttandance> attandanceEntry)
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
                                DestinationTableName = "[Attandance]",
                                BulkCopyTimeout = 6000,
                                BatchSize = attandanceEntry.Count()
                            };
                            var dataTable = GetDataTableForAttandanceEntry(attandanceEntry);

                            sqlBulkCopy.WriteToServer(dataTable);

                            scope.Complete();

                            sqlBulkCopy.Close();


                            return attandanceEntry.Count();


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

        private DataTable GetDataTableForAttandanceEntry(List<EmployeeDailyAttandance> attandanceEntry)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("OrganisationID", typeof(int));
            table.Columns.Add("LeaveTypeID", typeof(int));
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("YearID", typeof(int));
            table.Columns.Add("MonthID", typeof(int));
            table.Columns.Add("DateFrom", typeof(DateTime));
            table.Columns.Add("NepaliDateFrom", typeof(DateTime));
            table.Columns.Add("DateTo", typeof(DateTime));
            table.Columns.Add("NepaliDateTo", typeof(DateTime));
            table.Columns.Add("LeaveDaysID", typeof(int));
            table.Columns.Add("IsDailyAttandance", typeof(bool));
            table.Columns.Add("IsKaajLeave", typeof(bool));
            table.Columns.Add("IsManualAttandance", typeof(bool));
            table.Columns.Add("EntryTime", typeof(TimeSpan));
            table.Columns.Add("ExitTime", typeof(TimeSpan));
            table.Columns.Add("Days", typeof(decimal));
            table.Columns.Add("UserID", typeof(int));
            table.Columns.Add("Hours", typeof(decimal));
            table.Columns.Add("ExtraHours", typeof(decimal));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));



            //  note: the order of the field is very important
            //and should be same as the defined in table structure.
            attandanceEntry.ForEach(data => table.Rows.Add(
                                               data.ID,
                                                 data.OrganisationID
                                                  , data.LeaveTypeID
                                                  , data.EmployeeID
                                                  , data.Year
                                                  , data.Month
                                               , data.DateFrom,
                                                 data.NepaliDateFrom,
                                                 data.DateTo,
                                                 data.NepaliDateTo,
                                                 data.LeaveDaysID,
                                                 data.IsDailyAttandance,
                                                 data.IsKaaj,
                                                 data.IsManualAttandance,
                                                null,
                                                 null,
                                                 data.Days,
                                                 data.UserID,
                                                 data.Hours,
                                                 data.ExtraHours,
                                                 new LoginUser().UserName,
                                                 DateTime.Now,
                                                 new LoginUser().UserName,
                                                 DateTime.Now                                             

                                                ));
            return table;
        }
    }
}
