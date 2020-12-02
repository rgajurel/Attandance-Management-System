using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class Notification
    {
        public int? ID { get; set; }
        public int SN { get; set; }

        [DisplayName("Title")]
        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        [DisplayName("Notification Type")]
        [Required(ErrorMessage = "NotificationType is Required")]
        public string NotificationType { get; set; }
        [DisplayName("Organisation")]
        [Required(ErrorMessage ="Organisation is Required")]
        public int OrganisationID { get; set; }      
        public string OrganisationName { get; set; }
        [DisplayName("Group")]
        [Required(ErrorMessage = "Group is Required")]
        public string GroupID { get; set; }
        public List<string> GroupArray { get; set; }
        public int UserID { get; set; }
        public bool TriggerNow { get; set; }
        public DateTime TriggerDate { get; set; }

        public string tDate { get { return TriggerDate.ToString(); } }

        public DateTime ExpiryDate { get; set; }
        public string eDate { get { return ExpiryDate.ToShortDateString(); } }

        public int Status { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }

        //Search

        [DisplayName("Title")]
        public string TitleSearch { get; set; }

        [DisplayName("Notification Type")]
        public int NotificationTypeSearch { get; set; }

        [DisplayName("Organisation")]
        public int OrganisationIDSearch { get; set; }

        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

        public int Total { get; set; }

        public string EmployeeID { get; set; }

        public bool IsInternal { get; set; }

        //
        public int UserNotificationID { get; set; }

        //
    }

    public class UserNotification
    {
        public string UserName { get; set; }
        public int ID { get; set; }
        public int UserAllowedGroup { get; set; }

    }
}
