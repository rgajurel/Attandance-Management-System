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
    public class FeeTypeRepository : IFeeTypeRepository
    {
        public bool AddUpdateFeeType(FeeType feeType)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", feeType.ID);
                    parameters.Add("@Type", feeType.Type);
                    parameters.Add("@IsCommon", feeType.IsCommon);
                    parameters.Add("@AddedBy", feeType.AddedBy);
                    parameters.Add("@UpdatedBy", feeType.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateFeeType]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteFeeType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteFeeType]", parameters, commandType: CommandType.StoredProcedure);
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

        public FeeType EditFeeType(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    FeeType feeTypeedit = SqlMapper.Query<FeeType>(connection, "[dbo].[EditFeeType]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return feeTypeedit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<FeeType> GetAllFeeType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<FeeType> feeTypeList = SqlMapper.Query<FeeType>(connection, "[dbo].[GetAllFeeType]", commandType: CommandType.StoredProcedure).ToList();

                    return feeTypeList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
