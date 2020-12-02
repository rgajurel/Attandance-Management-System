using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class OrganisationEvents
    {
        public int? ID { get; set; }
        public int SN { get; set; }

        [DisplayName("Event Name")]

        [Required(ErrorMessage = "Event Name is Required")]
        public string EventName { get; set; }

        [DisplayName("Event Description")]
        public string EventDescription { get; set; }

        [DisplayName("Notification Type")]

        [Required(ErrorMessage = "NotificationType is Required")]

        public string NotificationType { get; set; }


        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Organisation is Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group is Required")]
        public string GroupID { get; set; }


        [DisplayName("English Date From")]
        [Required(ErrorMessage = "English Date From is Required")]
        public DateTime DateFrom { get; set; }

        [DisplayName("English Date To")]
        [Required(ErrorMessage = "English Date To is Required")]
        
        public string datefroms { get { return DateFrom.ToShortDateString(); } }
        public DateTime DateTo { get; set; }

        [DisplayName("Nepali Date From")]
       // [Required(ErrorMessage = "Nepali Date From is Required")]

        public string datetos { get { return DateTo.ToShortDateString();} }
        public DateTime NepaliDateFrom { get; set; }

        [DisplayName("Nepali Date To")]
       // [Required(ErrorMessage = "Nepali DateTo is Required")]
        public DateTime NepaliDateTo { get; set; }

        public List<string> GroupArray { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        //
        public string OrganisationName { get; set; }

        [DisplayName("Title")]
        public string EventNameSearch { get; set; }

        [DisplayName("Notification Type")]
        public int NotificationTypeSearch { get; set; }

        [DisplayName("Organisation")]
        public int OrganisationIDSearch { get; set; }

        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

        public int Total { get; set; }

        //



    }

}
