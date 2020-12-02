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
    public class ClientResultRepository : IClientResultRepository
    {
        public List<PublishedTerm> getPublishedTerms(string studentId, string sessionId)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@studentId", studentId);
                    param.Add("@session", sessionId);
                    
                    List<PublishedTerm> termList = SqlMapper.Query<PublishedTerm>(connection, "[dbo].[getPublishedTerm]", param, commandType: CommandType.StoredProcedure).ToList();
                    return termList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




    }
}
