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
    public class StudentsAssignmentsRepository : IStudentAssignmentsRepository
    {
        public bool AddUpdateStudentsAssignments(StudentsAssignments studentsAssignments)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@ID", studentsAssignments.ID==null?0: studentsAssignments.ID);
                    parameters.Add("@SessionID", studentsAssignments.SessionID);
                    parameters.Add("@ClassID", studentsAssignments.ClassID);
                    parameters.Add("@FacultyID", studentsAssignments.FacultyID);
                    parameters.Add("@Section", studentsAssignments.Section);
                    parameters.Add("@SubjectID", studentsAssignments.SubjectID);
                    parameters.Add("@Image", studentsAssignments.Image);
                    parameters.Add("@NotificationType", studentsAssignments.NotificationType);

                    parameters.Add("@Deadline", studentsAssignments.Deadline);
                    parameters.Add("@NepaliDeadline", studentsAssignments.NepaliDeadline);

                    parameters.Add("@GroupID", studentsAssignments.GroupID);
                    parameters.Add("@UserID", new LoginUser().LoggedInuserID);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateStudentsAssignments]", parameters, commandType: CommandType.StoredProcedure);
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
                throw ex;
            }
        }

        public bool DeleteStudentsAssignments(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteStudentsAssignments]", parameters, commandType: CommandType.StoredProcedure);
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

        public StudentsAssignments EditStudentAssignments(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    StudentsAssignments studentsAssignmentsEdit = SqlMapper.Query<StudentsAssignments >(connection, "[dbo].[EditStudentsAssignments]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return studentsAssignmentsEdit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StudentAssignmentsDetails> GetAllStudentsAssignments(StudentAssignmentsDetails search)
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);
                    param.Add("@PageSize", search.pageSize);
                    param.Add("@SectionSearch", search.SectionSearch == null ? "" : search.SectionSearch);
                    param.Add("@SearchClassID", search.SearchClassID);
                    param.Add("@SearchFacultyID", search.SearchFacultyID);
                    param.Add("@SearchSubjectID", search.SearchSubjectID);
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);


                    //param.Add("@SearchParameter", iNotification.searchParam);

                    List<StudentAssignmentsDetails> assignmentsList = SqlMapper.Query<StudentAssignmentsDetails>(connection, "[dbo].[GetAllStudentsAssignments]", param, commandType: CommandType.StoredProcedure).ToList();

                    return assignmentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
