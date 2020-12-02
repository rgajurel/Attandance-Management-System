using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainEntities;
using System.Data;
using Dapper;

namespace Infrastructure
{
    public class FeeDetailsForClientRepository : IFeeDetailsForClientRepository
    {
        public List<CollectionDetailsForlient> getCollectionDetails(string studentId, string session)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@studentId", studentId);
                    parameters.Add("@session", session);
                    List<CollectionDetailsForlient> PaymentHistory = SqlMapper.Query<CollectionDetailsForlient>(connection, "[dbo].[PaymentHistoryForClient]", parameters, commandType: CommandType.StoredProcedure).ToList();
                    return PaymentHistory;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FeeDetailsForClient> GetFeeDetails(string studentId, string session)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@studentId", studentId);
                    parameters.Add("@session", session);
                    List<FeeDetailsForClient> FeeList = SqlMapper.Query<FeeDetailsForClient>(connection, "[dbo].[FeeDetailsForClients]", parameters, commandType: CommandType.StoredProcedure).ToList();
                    return FeeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
