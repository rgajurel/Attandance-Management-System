using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class Attandance
    {
        public int? ID { get; set; }
        public int SN { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "This Field is Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Employee")]
        [Required(ErrorMessage = "This Field is Required")]
        public int EmployeeID { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public string EmployeeName { get; set; }
        public string Organisation { get; set; }
        public int NotificationType { get; set; }

        public string LeaveTypeName { get; set; }
        public string Years { get; set; }
        public string Months { get; set; }

        [DisplayName("Leave Type")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveTypeID { get; set; }

        [DisplayName("Leave Days")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveDaysID { get; set; }

        [DisplayName("Date From")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Nepali Date From")]
        public DateTime NepaliDateFrom { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        
        public DateTime DateTo { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Nepali Date To")]
        public DateTime NepaliDateTo { get; set; }

        [DisplayName("Days")]
        [Required(ErrorMessage = "This Field is Required")]
        public int Days { get; set; }

        public bool IsKaaj { get; set; }

        public bool IsDailyAttandance { get; set; }
        public bool IsManualAttandance { get; set; }

        public string AttandanceType { get; set; }
        public string EntryLocation { get; set; }
        public string ExitLocation { get; set; }


        public string StartDate { get; set; }
        public string EndDate { get; set; }


        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
     
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }

        [DisplayName("To Be Approved By")]
        public string ApprovedBy { get; set; }
        public string Statuss { get { if (Status == "0") { return "Approved"; } else if (Status == "1") { return "Rejected"; } else if (Status == "2") { return "Pending"; } else if (Status == "4") { return "Present"; } else if (Status == "5") { return "Absent"; } else { return "Others"; } } }


        //Search
        public int OrganisationIDSearch { get; set; }   
        public int EmployerIDSearch { get; set; }
        public int MonthSearch { get; set; }
        public int YearSearch { get; set; }

        public DateTime? DateSearch { get; set; }
        public int LeaveTypeIDsearch { get; set; }

        public string StatusSearch { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset { get; set; }
        public int Total { get; set; }

        public BiometricDevice BiometricDevice { get; set; }
        //Search
    }

    public class BiometricDevice
    {
        [DisplayName("IP Address")]
        [Required(ErrorMessage = "This Field is Required")]
        public string IpAddress { get; set; }

        [DisplayName("Port")]
        [Required(ErrorMessage = "This Field is Required")]
        public int Port { get; set; }

        public string connectDevice { get; set; }
    }

    public class AttandanceHistory
    {
        public string UserID { get; set; }
        public DateTime DateTime { get; set; }   
        
        public string Year { get; set; }
        public string Month { get; set; }

    }

   
}
