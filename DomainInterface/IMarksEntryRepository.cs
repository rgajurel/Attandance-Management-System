using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IMarksEntryRepository
    {
        #region Admin
        void InsertDataIntoMarksEntry();
        List<Subjects> GetSubjectBasedOnClass(string id);
        MarksEntry GetFullMarksPassMaeks(MarksEntry marksEntry);

        int DeleteData(MarksEntry marksEntry);
        List<MarksEntry> GetAllMarksEntry(MarksEntry marksentry);
       int MarksEntryBatchUpload(List<MarksEntry> marksEntry);

        SessionInfo GetActiveSessionInfo();

        bool DeleteMarksEntryInfo(int id);
        #endregion

    }
}
