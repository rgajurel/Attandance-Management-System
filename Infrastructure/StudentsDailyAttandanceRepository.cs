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
    public class StudentsDailyAttandanceRepository : IStudentsDailyAttandanceRepository
    {
        public int DeleteData(StudentsDailyAttandance studentsdailyAttandance)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SessionID", studentsdailyAttandance.SessionID);
                    param.Add("@ClassID", studentsdailyAttandance.ClassID);
                    param.Add("@Section", studentsdailyAttandance.Section);
                    param.Add("@FacultyID", studentsdailyAttandance.FacultyID);
                    param.Add("Date", studentsdailyAttandance.Date.Date);
                    var deletesuccess = connection.Execute("[dbo].[DeleteStudentsDailyAttandance]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StudentsDailyAttandance> GetDailyAttandance(StudentsDailyAttandance search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SessionID", search.SessionID);
                    param.Add("@ClassID", search.ClassID);
                    param.Add("@Section", search.Section);
                    param.Add("@FacultyID", search.FacultyID);
                    param.Add("@Date", search.Date.ToShortDateString());
                    param.Add("@NepaliDate", search.NepaliDate.ToShortDateString());
                    List<StudentsDailyAttandance> studentsDailyAttandanceList = SqlMapper.Query<StudentsDailyAttandance>(connection, "[dbo].[GetAllStudentsDailyAttandance]", param, commandType: CommandType.StoredProcedure).ToList();
                    return studentsDailyAttandanceList;
                }
            }
            catch (Exception ex)
            {
                throw null;
                // return null;
            }
        }

        public int AttandanceEntryBatchUpload(List<StudentsDailyAttandance> attandanceEntry)
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
                                DestinationTableName = "[StudentsDailyAttandance]",
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

        private DataTable GetDataTableForAttandanceEntry(List<StudentsDailyAttandance> attandanceEntry)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("StudentID", typeof(int));          
            table.Columns.Add("SessionID", typeof(int));
            table.Columns.Add("ClassID", typeof(int));
            table.Columns.Add("FacultyID", typeof(int));
            table.Columns.Add("Section", typeof(string));
            table.Columns.Add("IsAttend", typeof(bool));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("NepaliDate", typeof(DateTime));
            table.Columns.Add("InTime", typeof(string));
            table.Columns.Add("OutTime", typeof(string));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));       

            // note : the order of the field is very important
            // and should be same as the defined in table structure.
            attandanceEntry.ForEach(data => table.Rows.Add(
                                               data.ID,
                                                 data.StudentID                                                                                                                                        
                                                  , data.SessionID
                                                  , data.ClassID
                                                  , data.FacultyID
                                                  , data.Section
                                               , data.IsAttend,
                                                 data.Date,
                                                 data.NepaliDate,
                                                 data.InTime,
                                                 data.OutTime,
                                              new LoginUser().UserName,
                                               DateTime.Now                                            

                                                ));
            return table;
        }
    }
}
