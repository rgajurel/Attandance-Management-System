using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class SettingsTime
    {
        public int ID { get; set; }
        public string ValidTimeBeforeEntry { get; set; }
        public string ValidTimeAfterEntry { get; set; }
        public string ValidTimeAfterLeave { get; set; }
        public string ValidTimeBeforeLeave { get; set; }
    }
}
