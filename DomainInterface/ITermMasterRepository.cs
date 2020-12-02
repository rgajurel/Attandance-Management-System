using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface ITermMasterRepository
    {
        #region Admin
        bool AddUpdateTermMaster(TermMaster termMaster);
        List<TermMaster> GetAllTermMaster();
        bool DeleteTermMaster(int id);
        TermMaster EditTermMaster(int id);
        #endregion
    }
}
