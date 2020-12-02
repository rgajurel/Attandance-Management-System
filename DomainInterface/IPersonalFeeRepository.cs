using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IPersonalFeeRepository
    {
        #region Admin
        Class GetSectionBasedOnClass(string Class, string Faculty);
        List<PersonalFee> GetClassBasedOnFaculty(string faculty);
        string AddUpdatePersonalFee(List<PersonalFee> PersonalFees, string facultyID, string sessionId, string classId, string section, string type, string month);
        List<PersonalFee> GetAllPersonalFee(PersonalFee personalFee);
        bool DeletePersonalFee(int id);
        #endregion
    }
}
