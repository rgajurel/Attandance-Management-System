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
    public class TaxMasterRepository : ITaxMasterRepository
    {
        public bool AddUpdateTextMaster(TaxMaster taxMaster)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", taxMaster.ID);
                    parameters.Add("@AmountFrom", taxMaster.AmountFrom);
                    parameters.Add("@AmountTo", taxMaster.AmountTo);
                    parameters.Add("@SortOrder", taxMaster.SortOrder);
                    parameters.Add("@TaxPercentage", taxMaster.TaxPercentage);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateTaxMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteTaxMaster(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteTaxMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public TaxMaster EditTaxMaster(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    TaxMaster tax = SqlMapper.Query<TaxMaster>(connection, "[dbo].[EdittaxMaster]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return tax;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TaxMaster> GetAllTaxMaster()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<TaxMaster> taxMasterList = SqlMapper.Query<TaxMaster>(connection, "[dbo].[GetAllTax]", commandType: CommandType.StoredProcedure).ToList();

                    return taxMasterList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
