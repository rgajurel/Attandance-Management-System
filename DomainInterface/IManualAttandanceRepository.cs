using DomainEntities;
using DomainEntities.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IManualAttandanceRepository
    {
        SettingsTime GetSettingsTime();
        bool AddUpdateManualAttandance(Attandance leave);

        bool ChekAttandanceAlreadydone(Attandance leave);
        List<Attandance> GetAllManualAttandance(Attandance search);
       List<Attandance> GetAttandanceOfDay(DateTime datetime, int employeeid, bool isdailyattandance);
        bool AddDailyAttandance(Attandance leave);

        bool AddAttandanceHistory(AttandanceHistory history);
        bool UpdateDailyAttandance(Attandance attend);

        List<Attandance> GetAllDailyManualAttandance(Attandance search);

        #region Mobile

        MobileAttandanceList GetAllAttandanceMobile(GeneralViewModel<string>model);

        AttandanceStatus GetAttandanceStatus(GeneralViewModel<DeviceLogViewModel> model);

        #endregion
    }
}
