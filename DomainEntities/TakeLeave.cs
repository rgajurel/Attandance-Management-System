using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TakeLeave
    {
        public int? ID { get; set; }
        public int SN { get; set; }
        [DisplayName("Organisation")]
        [Required(ErrorMessage ="This Field is Required")]
        public int OrganisationID{get;set;}

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "This Field is Required")]
        public int EmployeeID { get; set; }

        [DisplayName("Days")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Days { get; set; }

        [DisplayName("Leave Type")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveTypeID { get; set; }

        [DisplayName("Leave For")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveDaysID { get; set; }

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


        [DisplayName("Month")]
        [Required(ErrorMessage = "This Field is Required")]
        public int Month { get; set; }
       public string ApprovedBy { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [DisplayName("Remaining Leave")]
        [Required(ErrorMessage = "This Field is Required")]
        public string RemainingLeave { get; set; }
        public string Status { get; set; }

        public bool IsLeave { get; set; }
        //
        public string TotalLeaveTaken { get; set; }

        //searchtakeleave
        public int OrganisationIDSearch { get; set; }
        public int LeaveTypeIDsearch { get; set; }
        public int EmployerIDSearch { get; set; }

        public int MonthSearch { get; set; }

        public int YearSearch { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset { get; set; }
        public int Total { get; set; }

        //
        //Data For grid
        public string Organisation { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveTypeName { get; set; }

        public string Description { get; set; }

        public string Months { get; set; }
        public string Years { get; set; }

        public string Statuss { get { if (Status == "0") { return "Approved"; } else if (Status == "1") { return "Rejected"; } else {  return "Pending";  } } }

        public string Statusss { get { if (Status == "0") { return "Approved"; } else if (Status == "1") { return "Rejected"; } else { if (!IsLeave) { return "-"; } else { return "Pending"; } } } }

        //

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ClientTakeLeave
    {
        public int? ID { get; set; }
        public int SN { get; set; }
        [DisplayName("Organisation")]
        [Required(ErrorMessage = "This Field is Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "This Field is Required")]
        public int EmployeeID { get; set; }

        [DisplayName("Days")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Days { get; set; }

        [DisplayName("Leave Type")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveTypeID { get; set; }

        [DisplayName("Leave For")]
        [Required(ErrorMessage = "This Field is Required")]
        public int LeaveDaysID { get; set; }

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

        [DisplayName("To Be Approved By")]
        public string ApprovedBy { get; set; }

        [DisplayName("Notificaiton Type")]
        [Required(ErrorMessage = "This Field is Required")]
        public int NotificationType { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "This Field is Required")]
        public int Month { get; set; }


        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [DisplayName("Remaining Leave")]
        [Required(ErrorMessage = "This Field is Required")]
        public string RemainingLeave { get; set; }
        public string Status { get; set; }

        public string Description { get; set; }
        //
        public string TotalLeaveTaken { get; set; }

        //searchtakeleave
        public int OrganisationIDSearch { get; set; }
        public int LeaveTypeIDsearch { get; set; }
        public int EmployerIDSearch { get; set; }
        public int StatusSearch { get; set; }
        public int MonthSearch { get; set; }

        public int YearSearch { get; set; }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int offset { get; set; }
        public int Total { get; set; }

        //
        //Data For grid
        public string Organisation { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveTypeName { get; set; }

        public string Months { get; set; }
        public string Years { get; set; }
        public string Statuss { get { if (Status == "0") { return "Approved"; } else if (Status == "1") { return "Rejected"; } else { return "Pending"; } } }

        //mobile
        public string StartDate { get; set; }
        public string EndDate { get; set; }



    }

    public class MobileDropDownList
    {
        public List<DropDownCommon> Year { get; set; }

        public List<DropDownCommon> Month { get; set; }

        public List<DropDownCommon> LeaveDays { get; set; }

        public List<DropDownCommon> LeaveType { get; set; }
        public List<DropDownCommon> ApprovedBy { get; set; }

    }
}
