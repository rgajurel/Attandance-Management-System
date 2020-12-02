using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface ISettingsRepository
    {
        string GetSettingByIDandGroup(string SettingsID, string OfGroups);
    }
}
