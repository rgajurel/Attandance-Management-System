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
    public class FacultyRepository : IFacultyRepository
    {
        public bool AddUpdateFaculty(Facultys faculty)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", faculty.ID);
                    parameters.Add("@Faculty", faculty.Faculty);
                    parameters.Add("@AddedBy", faculty.AddedBy);
                    parameters.Add("@UpdatedBy", faculty.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateFaculty]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool Deleteaculty(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteFaculty]", parameters, commandType: CommandType.StoredProcedure);
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

        public Facultys EditFaculty(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Facultys facultyEdit = SqlMapper.Query<Facultys>(connection, "[dbo].[EditFaculty]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return facultyEdit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Facultys> GetAllFaculty()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<Facultys> facultyList = SqlMapper.Query<Facultys>(connection, "[dbo].[GetAllFaculty]", commandType: CommandType.StoredProcedure).ToList();

                    return facultyList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
