using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
 public interface INotificationRepository
    {
        #region Admin
        bool AddUpdateNotification(Notification notification);
        List<Notification> GetAllNotification(Notification notificationSearch);
        bool DeleteNotification(int id);
        Notification EditNotification(int id);
        List<UserGroup> GetUserGroupBasedOnOrganisation(string id);
        IQueryable<Notification> GetAllNotificationByloginUser(string userid);

        void DisableNotification(string UserNotificationID);
        #endregion

        #region MobilePushNotification

        void PushNotificationToUser(Notification notification);
        #endregion
    }
}
