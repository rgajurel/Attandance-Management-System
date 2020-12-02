using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IStudentsAttandanceRepository
    {
        #region Admin
        int DeleteData(StudentsAttandance marksEntry);
        int AttandanceEntryBatchUpload(List<StudentsAttandance> marksEntry);
        List<StudentsAttandance> GetAllMarksStudentsAttandacne(StudentsAttandance marksentry);
        #endregion
    }
}
