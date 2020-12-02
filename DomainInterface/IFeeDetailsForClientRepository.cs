using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IFeeDetailsForClientRepository
    {
        #region Client
        List<FeeDetailsForClient> GetFeeDetails(string studentId, string session);
        List<CollectionDetailsForlient> getCollectionDetails(string studentId, string session);
        #endregion
    }
}
