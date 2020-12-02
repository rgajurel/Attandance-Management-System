using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IManagePublicHolidayAndSaturday
    {
        List<ManagePublicHoliday> GetDailyAttandance(ManagePublicHoliday search);
        int DeleteData(ManagePublicHoliday employeeDailyAttandance);      
        int AttandanceEntryBatchUpload(List<ManagePublicHoliday> attandanceEntry);

        ManagePublicHoliday GetDescription(ManagePublicHoliday search);
    }
}
