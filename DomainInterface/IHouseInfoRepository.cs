
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IHouseInfoRepository
    {
        #region Admin
        bool AddUpdateHouseInfo(HouseInfo houseName);
        List<HouseInfo> GetAllHouseInfo();
        bool DeleteHouseInfo(int id);
        HouseInfo EditHouseInfo(int id);
        #endregion
    }
}
