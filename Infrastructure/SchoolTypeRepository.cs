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
    public class SchoolTypeRepository : IShoolTypeRepository
    {
        public bool AddUpdateSchoolType(SchoolType schoolType)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", schoolType.ID);
                    parameters.Add("@Type", schoolType.Type);
                    parameters.Add("@AddedBy", schoolType.AddedBy);
                    parameters.Add("@UpdatedBy", schoolType.UpdatedBy);
                    var savechanges= connection.Execute("[dbo].[AddUpdateSchoolType]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges>0)
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

        public bool DeleteSchoolType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID",id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                   connection.Execute("[dbo].[DeleteSchoolType]", parameters, commandType: CommandType.StoredProcedure);
                   var savechanges= parameters.Get<Boolean>("@DeleteSuccess");
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

        public SchoolType EditSchoolType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    SchoolType schooledit = SqlMapper.Query<SchoolType>(connection, "[dbo].[EditSchoolType]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return schooledit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SchoolType> GetAllSchoolType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<SchoolType> schoolTypeList = SqlMapper.Query<SchoolType>(connection, "[dbo].[GetAllSchoolType]", commandType: CommandType.StoredProcedure).ToList();
                
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
