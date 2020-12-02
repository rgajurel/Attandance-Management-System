using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IEmployeeDailyAttandanceRepository
    {
        int DeleteData(EmployeeDailyAttandance employeeDailyAttandance);
        List<EmployeeDailyAttandance> GetDailyAttandance(EmployeeDailyAttandance search);
        int AttandanceEntryBatchUpload(List<EmployeeDailyAttandance> attandanceEntry);
    }
}
