using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Mobile
{
   public class MobileAttandance
    {
        public string EntryTime { get; set; }

        public string ExitTime { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }
        public string EntryLocation { get; set; }
        public string ExitLocation { get; set; }
    }

    public class MobileAttandanceList
    {
        public List<MobileAttandance> Attandance { get; set; }
        public bool IsAttandanceAvailiable { get; set; }
        public string AttandanceMessage { get; set; }
    }

    public class AttandanceStatus
    {
        public bool IsCheckedIn { get; set; }
        public bool IsCheckedOut { get; set; }
        public string CheckedInTime { get; set; }
        public string CheckedOutTime { get; set; }     
        public string AttandanceStatusMessage { get; set; }
    }
}
