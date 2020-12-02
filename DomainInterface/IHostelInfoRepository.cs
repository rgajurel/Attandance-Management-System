using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IHostelInfoRepository
    {
        #region Admin
        bool AddUpdateHostelInfo(HostelInfo hostelInfo);
        List<HostelInfo> GetAllHostelInfo();
        bool DeleteHostelInfo(int id);
        HostelInfo EditHostelInfo(int id);
        #endregion
    }
}
