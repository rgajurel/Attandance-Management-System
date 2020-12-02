using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IPublishResultRepository
    {
        #region Admin
        bool DeletePublishedResult(int id);

        string SavePublishedResult(string SessionID, string ClassID, string FacultyId, string TermId, string Section, string Format);

        List<PublishResult> GetAllPublishedResult();
        #endregion
    }
}
