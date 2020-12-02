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
    public class ClassMasterRepository : IClassMasterRepository
    {
        public bool AddUpdateClassMaster(ClassMaster classmaster)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", classmaster.ID);
                    parameters.Add("@Name", classmaster.Name);
                    parameters.Add("@AddedBy", classmaster.AddedBy);
                    parameters.Add("@UpdatedBy", classmaster.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateClassMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteClassMaster(int ID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", ID);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteClassMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public ClassMaster EditClassMaster(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    ClassMaster classMaster = SqlMapper.Query<ClassMaster>(connection, "[dbo].[EditClassMaster]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return classMaster;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ClassMaster> GetAllClassMaster()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<ClassMaster> classMasterList = SqlMapper.Query<ClassMaster>(connection, "[dbo].[GetAllClassMaster]", commandType: CommandType.StoredProcedure).ToList();

                    return classMasterList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
