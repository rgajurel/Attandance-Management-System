using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IStudentsDailyAttandanceRepository
    {
        List<StudentsDailyAttandance> GetDailyAttandance(StudentsDailyAttandance search);
        int DeleteData(StudentsDailyAttandance studentsdailyAttandance);

       int  AttandanceEntryBatchUpload(List<StudentsDailyAttandance> attandanceEntry);
    }
}
