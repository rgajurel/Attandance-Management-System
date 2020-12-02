using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IGradeMasterRepository
    {
        #region Admin
        bool AddUpdateGradeMaster(GradeMaster grademaster);
        List<GradeMaster> GetAllGradeMaster();
        bool DeleteGradeMaster(string grade);
        GradeMaster EditGrademaster(int id);
        List<SubSubject> GetAllSubSubject();

        #endregion
    }
}
