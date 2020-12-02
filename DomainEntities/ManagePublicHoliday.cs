using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
     public class ManagePublicHoliday
    {
        public int? ID { get; set; }

        public int SN { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "This Field is Required")]
        public int OrganisationID { get; set; }
        public int EmployeeID { get; set; }
        [DisplayName("Date From")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime DateFrom { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime DateTo { get; set; }

        [DisplayName("Date From")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime NepaliDateFrom { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime NepaliDateTo { get; set; }

        [DisplayName("Year")]
        [Required(ErrorMessage = "Date is Required")]
        public int Year { get; set; }       

        [DisplayName("Month")]
        [Required(ErrorMessage = "This Field is Required")]
        public int Month { get; set; }
        public int Days { get; set; }
        public int UserID { get; set; }    

        public int LeaveTypeID { get; set; }

        public bool IsAttend { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string Description { get; set; }
        public string Employee { get; set; }
        public string Organisation { get; set; }

        public string Years { get; set; }
        public string Months { get; set; }







    }
}
