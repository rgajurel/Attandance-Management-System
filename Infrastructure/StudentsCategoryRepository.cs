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
    public class StudentsCategoryRepository : IStudentsCategoryRepository
    {
        public bool AddUpdateStudentsCategory(StudentsCategorys studentCategory)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", studentCategory.ID);
                    parameters.Add("@StudentsCategory", studentCategory.StudentsCategory);
                    parameters.Add("@AddedBy", studentCategory.AddedBy);
                    parameters.Add("@UpdatedBy", studentCategory.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateStudentCategory]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteStudentsCategory(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteStudentCategory]", parameters, commandType: CommandType.StoredProcedure);
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

        public StudentsCategorys EditStudentsCategory(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    StudentsCategorys studentsCateegory = SqlMapper.Query<StudentsCategorys>(connection, "[dbo].[EditStudentCategory]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return studentsCateegory;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<StudentsCategorys> GetAllStudentsCategory()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<StudentsCategorys> studentsCategoryList = SqlMapper.Query<StudentsCategorys>(connection, "[dbo].[GetAllStudentsCategory]", commandType: CommandType.StoredProcedure).ToList();

                    return studentsCategoryList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
