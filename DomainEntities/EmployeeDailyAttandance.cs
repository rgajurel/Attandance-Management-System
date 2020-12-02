using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class EmployeeDailyAttandance
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

        [DisplayName("Date From")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime NepaliDateFrom { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime DateTo { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime NepaliDateTo { get; set; }
        public int Year { get; set; }

        [DisplayName("Leave Days")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveDaysID { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "This Field is Required")]
        public int Month { get; set; }
        public int Days { get; set; }
        public int UserID { get; set; }
        public double Hours { get; set; }
       
        public int LeaveTypeID { get; set; }

        public double ExtraHours { get; set; }

        //public TimeSpan EntryTime { get; set; }   

      
        //public TimeSpan ExitTime { get; set; }

        //public string EntryimeString { get { return EntryTime.Hours + ":" + EntryTime.Minutes + ":" + EntryTime.Seconds; } }

        //public string ExitimeString { get { return ExitTime.Hours + ":" + ExitTime.Minutes + ":" + ExitTime.Seconds; } }


        public bool IsAttend { get; set; }
        public bool IsDailyAttandance { get; set; }
        public bool IsManualAttandance { get; set; }

        public bool IsKaaj { get; set; }

        public string Employee { get; set; }

        public string Organisation { get; set; }

    }
}
