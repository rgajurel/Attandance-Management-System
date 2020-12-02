using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IBusInfoRepository
    {
        #region Admin
        bool AddUpdateBusInfo(BusInfo busInfo);
        List<BusInfo> GetAllBusInfo();
        bool DeleteBusInfo(int id);
        BusInfo EditBusInfo(int id);
        #endregion
    }
}
