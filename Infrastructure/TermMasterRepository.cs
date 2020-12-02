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
    public class TermMasterRepository : ITermMasterRepository
    {
        public bool AddUpdateTermMaster(TermMaster termMaster)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", termMaster.ID);
                    parameters.Add("@TermName", termMaster.TermName);
                    parameters.Add("@TermPercentage", termMaster.TermPercentage);
                    parameters.Add("@IsFinalTerm", termMaster.IsFinalTerm);
                    parameters.Add("@AddedBy", termMaster.AddedBy);
                    parameters.Add("@UpdatedBy", termMaster.UpdatedBy);
                    var savechanges = connection.Execute("[dbo].[AddUpdateTermMaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteTermMaster(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteTermmaster]", parameters, commandType: CommandType.StoredProcedure);
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

        public TermMaster EditTermMaster(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    TermMaster termMasterEdit = SqlMapper.Query<TermMaster>(connection, "[dbo].[EditTermMaster]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return termMasterEdit;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TermMaster> GetAllTermMaster()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    List<TermMaster> termMasterList = SqlMapper.Query<TermMaster>(connection, "[dbo].[GetAllTermMaster]", commandType: CommandType.StoredProcedure).ToList();

                    return termMasterList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
