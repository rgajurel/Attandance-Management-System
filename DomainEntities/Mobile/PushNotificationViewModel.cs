using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Mobile
{
   public class PushNotificationViewModel
    {
        public Notification notification { get; set; }
        public List<DeviceTokenViewModel> DeviceToken { get; set; }

    }

    public class DeviceTokenViewModel
    {
        public string DeviceToken { get; set; }
    }

    public class DeviceLog
    {
        public string EmployeeId { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
