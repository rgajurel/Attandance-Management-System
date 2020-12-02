using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface ISubjectsRepository
    {
        #region Admin
        bool AddUpdateSubject(Subjects subject);
        List<Subjects> GetAllSubjects();
        bool DeleteSubjects(int id);
        Subjects EditSubjects(int id);

        int SubjectBatchUpload(List<Subjects>ListSubject);
        #endregion
    }
}

