using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;

namespace DomainInterface
{
    public interface IClientLoginRepository
    {

        #region Client        
        string loginClient(string email, string password);
        #endregion
    }
}
