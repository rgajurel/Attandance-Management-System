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
using FastMember;
using System.Transactions;

namespace Infrastructure
{
    public class SubjectsRepository : ISubjectsRepository
    {
        public bool AddUpdateSubject(Subjects subject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", subject.ID);
                    parameters.Add("@SubjectCode", subject.SubjectCode.ToUpper());
                    parameters.Add("@ClassID", subject.ClassID);
                    parameters.Add("@SubjectName", subject.SubjectName);
                    parameters.Add("@CreditPoint", subject.CreditPoints);               
                    parameters.Add("@AddedBy", subject.AddedBy);
                    parameters.Add("@UpdatedBy", subject.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateSubjects]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteSubjects(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteSubject]", parameters, commandType: CommandType.StoredProcedure);
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

        public Subjects EditSubjects(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Subjects subjectedit = SqlMapper.Query<Subjects>(connection, "[dbo].[EditSubjectMaster]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return subjectedit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Subjects> GetAllSubjects()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<Subjects> subjectList = SqlMapper.Query<Subjects>(connection, "[dbo].[GetAllSubjectMaster]", commandType: CommandType.StoredProcedure).ToList();

                    return subjectList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SubjectBatchUpload(List<Subjects> ListSubject)
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
                                        DestinationTableName = "[SubjectsMaster]",
                                        BulkCopyTimeout = 6000,
                                        BatchSize = ListSubject.Count()
                                    };
                                    var dataTable = GetDataTableForEmployees(ListSubject);
                                  //  connection.Open();

                                    sqlBulkCopy.WriteToServer(dataTable);

                                    scope.Complete();

                                    sqlBulkCopy.Close();
                                   
                                    return ListSubject.Count();


                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }                 
                                              
                    }
                   
              
            }
            catch(Exception ex)
            {
                throw ex;
            }
    }

        private DataTable GetDataTableForEmployees(List<Subjects> employees)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("SubjectCode", typeof(string));
            table.Columns.Add("SubjectName", typeof(string));
            table.Columns.Add("CreditPoints", typeof(string));
            table.Columns.Add("ClassID", typeof(int));
            table.Columns.Add("AddedBy", typeof(string));
            table.Columns.Add("AddedOn", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedOn", typeof(DateTime));

            // note : the order of the field is very important
            // and should be same as the defined in table structure.
            employees.ForEach(data => table.Rows.Add(
                                                data.ID
                                                , data.SubjectCode.ToUpper()
                                                , data.SubjectName
                                                ,data.CreditPoints
                                                ,data.ClassID
                                                ,data.AddedBy
                                                ,DateTime.Now
                                                ,data.UpdatedBy,
                                                data.UpdatedOn
                                                ));
            return table;
        }
    }
}   
