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
    public class StudentsAttandanceRepository : IStudentsAttandanceRepository
    {
        public List<StudentsAttandance> GetAllMarksStudentsAttandacne(StudentsAttandance marksentry)
         {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@Section", marksentry.Section);                   
                    param.Add("@FacultyID", marksentry.FacultyID);               
                   List<StudentsAttandance> studentsAttandanceList = SqlMapper.Query<StudentsAttandance>(connection, "[dbo].[GetAllStudetntsAttandance]", param, commandType: CommandType.StoredProcedure).ToList();
                    return studentsAttandanceList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
               // return null;
            }
        }

        public int DeleteData(StudentsAttandance marksentry)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@Section", marksentry.Section);
                    param.Add("@FacultyID", marksentry.FacultyID);
                    var deletesuccess = connection.Execute("[dbo].[DeleteStudentsAttandance]", param, commandType: CommandType.StoredProcedure);
                    return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AttandanceEntryBatchUpload(List<StudentsAttandance> attandanceEntry)
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
                                DestinationTableName = "[StudentsAttandance]",
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


        private DataTable GetDataTableForAttandanceEntry(List<StudentsAttandance> attandanceEntry)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("RollNo", typeof(string));
            table.Columns.Add("PresentDays", typeof(string));
            table.Columns.Add("TotalDays", typeof(string));
            table.Columns.Add("SessionID", typeof(int));
            table.Columns.Add("ClassID", typeof(int));
            table.Columns.Add("FacultyID", typeof(int));              
            table.Columns.Add("Section", typeof(string));
            table.Columns.Add("IsAttend", typeof(bool));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));                    

            // note : the order of the field is very important
            // and should be same as the defined in table structure.
            attandanceEntry.ForEach(data => table.Rows.Add(
                                               data.ID,
                                                 data.StudentID
                                                  , data.RollNo
                                                  ,data.PresentDays
                                                  ,data.TotalDays
                                                  ,data.SessionID
                                                  ,data.ClassID
                                                  , data.FacultyID                                       
                                                  , data.Section
                                               ,data.IsAttend,
                                               data.AddedBy,
                                               DateTime.Now,
                                               data.UpdatedBy,
                                               data.UpdatedOn
                                              
                                                ));
            return table;
        }
    }
}
