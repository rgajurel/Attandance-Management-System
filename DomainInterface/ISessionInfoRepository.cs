using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ISessionInfoRepository
    {
        #region Admin
        bool AddUpdateSessionInfo(SessionInfo sessionInfo);
        List<SessionInfo> GetAllSessionInfo();
        bool DeleteSessionInfo(int id);
        SessionInfo EditSessionInfo(int id);
        #endregion
    }
}
