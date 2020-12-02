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
   public class ManagePublicHolidayAndSaturday: IManagePublicHolidayAndSaturday
    {
        public List<ManagePublicHoliday> GetDailyAttandance(ManagePublicHoliday search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", search.OrganisationID);
                    param.Add("@Month", search.Month);
                    param.Add("@Year", search.Year);                   
                    param.Add("@DateFrom", search.DateFrom.ToShortDateString());
                    param.Add("@DateTo", search.DateTo.ToShortDateString());
                    List<ManagePublicHoliday> studentsDailyAttandanceList = SqlMapper.Query<ManagePublicHoliday>(connection, "[dbo].[GetAllManagePublicHoliday]", param, commandType: CommandType.StoredProcedure).ToList();
                    return studentsDailyAttandanceList;
                }
            }
            catch (Exception ex)
            {
                throw null;
                // return null;
            }
        }

        public int DeleteData(ManagePublicHoliday employeeDailyAttandance)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", employeeDailyAttandance.OrganisationID);
                    param.Add("@Year", employeeDailyAttandance.Year);
                    param.Add("@Month", employeeDailyAttandance.Month);
                    param.Add("@Date", employeeDailyAttandance.DateFrom);
                    var deletesuccess = connection.Execute("[dbo].[DeleteManagePublicHoliday]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AttandanceEntryBatchUpload(List<ManagePublicHoliday> attandanceEntry)
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
                                DestinationTableName = "[PublicHolidayManage]",
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

        private DataTable GetDataTableForAttandanceEntry(List<ManagePublicHoliday> attandanceEntry)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("OrganisationID", typeof(int));          
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("UserID", typeof(int));
            table.Columns.Add("MonthID", typeof(int));
            table.Columns.Add("YearID", typeof(int));           
            table.Columns.Add("DateFrom", typeof(DateTime));
            table.Columns.Add("DateTo", typeof(DateTime));
            table.Columns.Add("Days", typeof(int));          
            table.Columns.Add("NepaliDateFrom", typeof(string));
            table.Columns.Add("NepaliDateTo", typeof(string));

            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));        



            //  note: the order of the field is very important
            //and should be same as the defined in table structure.
            attandanceEntry.ForEach(data => table.Rows.Add(
                                               data.ID,
                                                 data.OrganisationID                                               
                                                  , data.EmployeeID,
                                                  data.UserID
                                                   , data.Month,
                                                   data.Year                                                 
                                               , data.DateFrom,
                                                   data.DateTo,
                                                 data.Days,
                                                 data.NepaliDateFrom,
                                                 data.NepaliDateTo,
                                                 data.Description,
                                                 new LoginUser().UserName,
                                                 DateTime.Now,
                                                 new LoginUser().UserName,
                                             DateTime.Now                                               

                                                ));
            return table;
        }

        public ManagePublicHoliday GetDescription(ManagePublicHoliday search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", search.OrganisationID);
                    param.Add("@Year", search.Year);
                    param.Add("@Month", search.Month);
                    param.Add("@DateFrom", search.DateFrom.ToShortDateString());
                    param.Add("@DateTo", search.DateTo.ToShortDateString());
                    ManagePublicHoliday managePublicHoliday = SqlMapper.Query<ManagePublicHoliday>(connection, "[dbo].[GetDescriptionPublicHoliday]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return managePublicHoliday;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
