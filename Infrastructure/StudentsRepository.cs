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
    public class StudentsRepository : IStudentsRepository
    {
        public bool AddUpdateStudents(Students studs)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", studs.ID);
                    parameters.Add("@AcademicYear", studs.AcademicYear);
                    parameters.Add("@RegistrationNo", studs.RegistrationNo);
                    parameters.Add("@EnglishJoinningDate", studs.EnglishJoinningDate);
                    parameters.Add("@NepaliJoinningDate", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(studs.EnglishJoinningDate));
                    parameters.Add("@FacultyID", studs.FacultyID);
                    parameters.Add("@ClassID", studs.ClassID);
                    parameters.Add("@Section", studs.Section);
                    parameters.Add("@RollNo", studs.RollNo);
                    parameters.Add("@Batch", studs.Batch);
                    parameters.Add("@SymbolNo", studs.SymbolNo);

                    parameters.Add("@UserID", studs.UserID);

                    parameters.Add("@HouseID", studs.HouseID);
                    parameters.Add("@Email", studs.Email);
                    parameters.Add("@MobileNo", studs.MobileNo);
                    parameters.Add("@PhoneNo", studs.PhoneNo);
                    parameters.Add("@StudentName", studs.StudentName);
                    parameters.Add("@EnglishDateOfBirth", studs.EnglishDateOfBirth);
                    parameters.Add("@NepaliDateOfBirth",DateConversionHelper.GetEnglsihTimeToNepaliDateTime(studs.EnglishDateOfBirth));
                    parameters.Add("@Gender", studs.Gender);
                    parameters.Add("@Status", studs.Status);
                    parameters.Add("@BloodGroupID", studs.BloodGroupID);

                    parameters.Add("@CategoryID", studs.CategoryID);
                    parameters.Add("@ReligionID", studs.ReligionID);
                    parameters.Add("@CasteID", studs.CasteID);
                    parameters.Add("@CitizenShipNumber", studs.CitizenShipNumber);
                    parameters.Add("@TemporaryAddress", studs.TemporaryAddress);
                    parameters.Add("@PermanentAddress", studs.PermanentAddress);
                    parameters.Add("@LastSchoolAttended", studs.LastSchoolAttended);
                    parameters.Add("@Result", studs.Result);
                   
                    parameters.Add("@FatherName", studs.FatherName);
                    parameters.Add("@FatherMobileNo", studs.FatherMobileNo);
                    parameters.Add("@FatherEmail", studs.FatherEmail);                
                
                    parameters.Add("@MotherName", studs.MotherName);
                   
                    parameters.Add("@Image", studs.Image);
                    parameters.Add("@AddedBy", studs.AddedBy);
                    parameters.Add("@UpdatedBy", studs.UpdatedBy);
                    parameters.Add("@File", studs.DocumentsSubmitted);


                    var savechanges = connection.Execute("[dbo].[AddUpdateStudents]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteStudents(int id)
        {
            throw new NotImplementedException();
        }

        public Students DetailsStudents(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Students studentDetails = SqlMapper.Query<Students>(connection, "[dbo].[DetailsStudents]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return studentDetails;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Students EditStudents(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Students studentsedit = SqlMapper.Query<Students>(connection, "[dbo].[EditStudentsInfo]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return studentsedit;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<StudentsSearch> GetAllStudents(string prefix)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("prefix", prefix);                   
                    List<StudentsSearch> studentsList = SqlMapper.Query<StudentsSearch>(connection, "[dbo].[GetAllStudentsAutoComplete]",param, commandType: CommandType.StoredProcedure).ToList();

                    return studentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<StudentsSearch> GetAllStudents(StudentsSearch search)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@offset", search.offset);
                    param.Add("@PageSize", search.pageSize);
                    param.Add("@FacultySearchID", search.FacultySearchID);
                    param.Add("@SectionSearch", search.SectionSearch == null ? "" : search.SectionSearch);
                    param.Add("@ClassSearchID", search.ClassSearchID);
                    param.Add("@StudentsSearchName", search.StudentsSearchName == null ? "" : search.StudentsSearchName);
                    param.Add("@BatchSearch", search.BatchSearch == null ? "" : search.BatchSearch);
                    param.Add("@RegistratioNoSearch", search.RegistratioNoSearch== null ? "" : search.RegistratioNoSearch);

                    //param.Add("@SearchParameter", iNotification.searchParam);
                
                    List<StudentsSearch> studentsList = SqlMapper.Query<StudentsSearch>(connection, "[dbo].[GetAllStudetns]",param, commandType: CommandType.StoredProcedure).ToList();

                    return studentsList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetClassRollNoCount(string faculty, string classs, string section, string rollno)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@faculty", faculty);
                    parameters.Add("@classs", classs);
                    parameters.Add("@section", section);
                    parameters.Add("@rollno", rollno);

                    parameters.Add("@Count", 0, dbType: DbType.Int16, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[GetRollNoCount]", parameters, commandType: CommandType.StoredProcedure);
                    var sectioncount = parameters.Get<dynamic>("@Count");
                    return sectioncount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Facultys> GetFacultyBasedOnClass(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<Facultys> facultys = SqlMapper.Query<Facultys>(connection, "[dbo].[GetFacultyBasedOnClass]", param, commandType: CommandType.StoredProcedure).ToList(); ;

                    return facultys;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Class GetSectionBasedOnClass(string classid,string facultyid)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ClassID", classid);
                    param.Add("@FacultyID", facultyid);
                    Class sections = SqlMapper.Query<Class>(connection, "[dbo].[GetSectionBasedOnClass]", param, commandType: CommandType.StoredProcedure).FirstOrDefault(); ;

                    return sections;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UniqueNoGeneration GetUniqueRegistrationNo(string batch)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Batch", batch);
                    UniqueNoGeneration registrationno = SqlMapper.Query<UniqueNoGeneration>(connection, "[dbo].[GetUniqueNumberGenerator]", param,commandType: CommandType.StoredProcedure).FirstOrDefault(); ;

                    return registrationno;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
