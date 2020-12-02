using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ISchoolInformationRepository
    {
        #region Admin
        bool AddUpdateSchoolInformation(SchoolInformation schoolInformation);
        List<SchoolInformation> GetAllSchoolInformation();
        bool DeleteSchoolInformation(int id);
        SchoolInformation EditSchoolInformation(int id);
        #endregion
    }
}
