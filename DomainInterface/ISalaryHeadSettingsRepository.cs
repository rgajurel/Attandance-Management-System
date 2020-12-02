using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ISalaryHeadSettingsRepository
    {
        #region Admin
        bool AddUpdateSalaryHeading(SalaryHeadingSettings salaryHeadingSettings);
        List<SalaryHeadingSettings> GetAllSalaryHeadingSettings(SalaryHeadingSettings salaryHeadingSettings);
        bool DeleteSalaryHeadingSettings(int id);
        SalaryHeadingSettings EditSalaryHeadingSettings(int id);
        int DeleteData(SalaryHeadingSettings salaryHeadingsett);

        int SalaryHeadingsSettingsBatchUpload(List<SalaryHeadingSettings> salHeadSettings);
        #endregion
    }
}
