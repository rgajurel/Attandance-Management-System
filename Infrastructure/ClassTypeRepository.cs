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
    public class ClassTypeRepository : IClassTypeRepository
    {
        public bool AddUpdateClassType(ClassType classType)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", classType.ID);
                    parameters.Add("@Type", classType.Type);
                    parameters.Add("@AddedBy", classType.AddedBy);
                    parameters.Add("@UpdatedBy", classType.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateClassType]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteClassType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteClassType]", parameters, commandType: CommandType.StoredProcedure);
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

        public ClassType EditClassType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    ClassType classedit = SqlMapper.Query<ClassType>(connection, "[dbo].[EditClassType]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return classedit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ClassType> GetAllClassType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<ClassType> schoolTypeList = SqlMapper.Query<ClassType>(connection, "[dbo].[GetAllClassType]", commandType: CommandType.StoredProcedure).ToList();

                    return schoolTypeList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
