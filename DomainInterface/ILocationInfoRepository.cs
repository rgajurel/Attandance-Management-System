using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ILocationInfoRepository
    {
        #region Admin
        bool AddUpdateLocationInfo(Location location);
        List<Location> GetAllLocationInfo();
        bool DeleteLocationInfo(int id);
        Location EditLocationInfo(int id);
        #endregion
    }
}
