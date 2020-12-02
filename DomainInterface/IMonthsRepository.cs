using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface IMonthsRepository
    {
        #region Admin
        bool AddUpdateMonthsInfo(Months sessionInfo);
        List<Months> GetAllMonthsInfo();
        bool DeleteMonthsInfo(int id);
        Months EditMonthsInfo(int id);
        #endregion
    }
}
