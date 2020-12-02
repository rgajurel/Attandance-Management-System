using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IClientResultRepository
    {
        #region Client
        List<PublishedTerm> getPublishedTerms(string studentId,string sessionId);
        
        #endregion
    }
}
