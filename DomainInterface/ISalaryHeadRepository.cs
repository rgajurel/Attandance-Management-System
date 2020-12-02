using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ISalaryHeadRepository
    {
        #region Admin
        bool AddUpdateSalaryHeading(SalaryHeading salaryHeading);
        List<SalaryHeading> GetAllSalaryHeading();
        bool DeleteSalaryHeading(int id);
        SalaryHeading EditSalaryHeading(int id);
        #endregion
    }
}
