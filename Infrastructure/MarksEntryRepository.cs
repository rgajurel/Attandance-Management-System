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
    public class MarksEntryRepository : IMarksEntryRepository
    {
        public void InsertDataIntoMarksEntry()
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                                      

               connection.Execute( "[dbo].[InsertIntoMarksEntry]", commandType: CommandType.StoredProcedure);

                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SessionInfo GetActiveSessionInfo()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                                    //  param.Add("@TermID", marksentry.TermID);                  

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    SessionInfo sessionInfo = SqlMapper.Query<SessionInfo>(connection, "[dbo].[GetActiveSession]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return sessionInfo;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<MarksEntry> GetAllMarksEntry(MarksEntry marksentry)
         {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                   
                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@Section", marksentry.Section);
                   param.Add("@SubjectID", marksentry.SubjectID);
                    param.Add("@FacultyID", marksentry.FacultyID);
                    param.Add("@TermID", marksentry.TermID);
                    param.Add("@FullMarksTheory", marksentry.FullMarksTheory);
                    param.Add("@FullMarksPractical", marksentry.FullMarksPractical);
                    param.Add("@PassMarksTheory", marksentry.PassMarksTheory);
                    param.Add("@PassMarksPractical", marksentry.PassMarksPractical);
                    //  param.Add("@TermID", marksentry.TermID);                  

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<MarksEntry> studentsList = SqlMapper.Query<MarksEntry>(connection, "[dbo].[GetAllMarksEntry]", param, commandType: CommandType.StoredProcedure).ToList();
                  
                    return studentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Subjects> GetSubjectBasedOnClass(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    var sections = SqlMapper.Query<Subjects>(connection, "[dbo].[GetSujectBasedOnClass]", param, commandType: CommandType.StoredProcedure).ToList(); ;

                    return sections;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int MarksEntryBatchUpload(List<MarksEntry> marksEntry)
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
                                DestinationTableName = "[MarksEntry]",
                                BulkCopyTimeout = 6000,
                                BatchSize = marksEntry.Count()
                            };
                            var dataTable =GetDataTableForEmployees(marksEntry);
                          
                            sqlBulkCopy.WriteToServer(dataTable);                         

                            scope.Complete();

                            sqlBulkCopy.Close();
                          

                            return marksEntry.Count();


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


        private DataTable GetDataTableForEmployees(List<MarksEntry> marksEntry)
        {
            var table = new DataTable();
           table.Columns.Add("ID", typeof(int));
            table.Columns.Add("ClassID", typeof(int));
            table.Columns.Add("FacultyID", typeof(int));
            table.Columns.Add("SubjectID", typeof(int));
            table.Columns.Add("TermID", typeof(int));

            table.Columns.Add("SessionID", typeof(int));
            table.Columns.Add("Section", typeof(string));

            table.Columns.Add("StudentID", typeof(int));
            table.Columns.Add("FullMarksTheory", typeof(string));
            table.Columns.Add("PassMarksTheory", typeof(string));
            table.Columns.Add("CreditPoint", typeof(string));
            table.Columns.Add("FullMarksPractical", typeof(string));
            

            table.Columns.Add("PassMarksPractical", typeof(string));
            table.Columns.Add("ObtainedMarksTheory", typeof(string));
            table.Columns.Add("ObtainedMarksPractical", typeof(string));
            table.Columns.Add("ObtainedGradeTheory", typeof(string));
            table.Columns.Add("ObtaindedGradePractical", typeof(string));
            table.Columns.Add("FinalGrade", typeof(string));
            table.Columns.Add("GradePoint", typeof(string));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));
            table.Columns.Add("IsAdmin", typeof(bool));

            // note : the order of the field is very important
            // and should be same as the defined in table structure.
            marksEntry.ForEach(data => table.Rows.Add(
                                               data.ID,
                                                 data.ClassID
                                                  , data.FacultyID
                                                , data.SubjectID                                               
                                                , data.TermID
                                                , GetActiveSessionInfo().ID
                                                , data.Section
                                                ,data.StudentID
                                                , data.FullMarksTheory,
                                                data.PassMarksTheory
                                                , data.CreditPoint
                                                , data.FullMarksPractical
                                                , data.PassMarksPractical
                                                , data.ObtainedMarksTheory
                                                , data.ObtainedMarksPractical
                                                ,data.ObtainedGradeTheory
                                                , data.ObtaindedGradePractical,
                                                data.FinalGrade,
                                                data.GradePoint,
                                               new LoginUser().UserName,
                                               DateTime.Now,
                                               data.UpdatedBy,
                                               data.UpdatedOn,
                                               data.IsAdmin
                                                ));
            return table;
        }

        public MarksEntry GetFullMarksPassMaeks(MarksEntry marksentry)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@Section", marksentry.Section);
                    param.Add("@SubjectID", marksentry.SubjectID);
                    param.Add("@TermID", marksentry.TermID);
                    param.Add("@FacultyID", marksentry.FacultyID);

                    //  param.Add("@TermID", marksentry.TermID);                  

                    //param.Add("@SearchParameter", iNotification.searchParam);

                    MarksEntry studentsList = SqlMapper.Query<MarksEntry>(connection, "[dbo].[GetFullMarksPassMarks]", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    return studentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeleteData(MarksEntry marksentry)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {

                    DynamicParameters param = new DynamicParameters();

                    param.Add("@SessionID", marksentry.SessionID);
                    param.Add("@ClassID", marksentry.ClassID);
                    param.Add("@FacultyID", marksentry.FacultyID);
                    param.Add("@Section", marksentry.Section);
                    param.Add("@SubjectID", marksentry.SubjectID);
                    param.Add("@TermID", marksentry.TermID);                  
                    var deletesuccess=  connection.Execute("[dbo].[DeleteMarksEntry]",param, commandType: CommandType.StoredProcedure);
                   return deletesuccess;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteMarksEntryInfo(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteMarksEntrySingle]", parameters, commandType: CommandType.StoredProcedure);
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
    }
}
