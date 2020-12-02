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
    public class ClassRepository : IClassRepository
    {
        public bool AddUpdateClass(Class classs)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", classs.ID);
                    parameters.Add("@ClassTypeID", classs.ClassTypeID);
                    parameters.Add("@ClassID", classs.ClassID);
                    parameters.Add("@FacultyID", classs.FacultyID);
                    parameters.Add("@Sections", classs.Sections);
                    parameters.Add("@AddedBy", classs.AddedBy);
                    parameters.Add("@UpdatedBy", classs.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateClass]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteClass(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteClass]", parameters, commandType: CommandType.StoredProcedure);
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

        public Class EditClass(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Class classedit = SqlMapper.Query<Class>(connection, "[dbo].[EditClass]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return classedit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Class> GetAllClass()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<Class> classList = SqlMapper.Query<Class>(connection, "[dbo].[GetAllClass]", commandType: CommandType.StoredProcedure).ToList();

                    return classList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
