using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ISalaryHeadAmountRepository
    {
        List<SalaryHeadAmount> GetAllSalaryHeadAmount(SalaryHeadAmount salaryHead);
        int DeleteData(SalaryHeadAmount salaryHeadAmount, int salaryHeadID);
        int SalaryHeadBatchUpload(List<SalaryHeadAmount> leaveEntry, int Year);
    }
}
