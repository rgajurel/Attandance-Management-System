using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface  IYearlyHolidaysEntryRepository
    {
        #region Admin
        bool AddUpdateYearlyHolidaysEntry(YearlyHolidaysEntry department);
        List<YearlyHolidaysEntry> GetAllYearlyHolidaysEntry();
        bool DeleteYearlyHolidaysEntry(int id);
        YearlyHolidaysEntry EditYearlyHolidaysEntry(int id);
        #endregion
    }
}
