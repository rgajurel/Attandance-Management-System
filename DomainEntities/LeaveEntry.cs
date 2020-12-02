using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class LeaveEntry
    {
        public int ID { get; set; }
        public int SN { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage ="Organisation is Required")]
        public int OrganisationID { get; set;}

        [DisplayName("Leave Type")]
        [Required(ErrorMessage = "LeaveType is Required")]
        public int LeaveTypeID { get; set; }
        public int EmployeeID { get; set; }

        [DisplayName("Year")]
        [Required(ErrorMessage = "Year is Required")]
        public int YearID { get; set; }

        [DisplayName("Total Days in Year")]
        [Required(ErrorMessage = "Total Days in Year is Required")]
        public double TotalDays { get; set; }

        public bool IsMonthRule { get; set; }

        [DisplayName("Total Days in Month")]

        [Required(ErrorMessage = "Total Days in Month is Required")]
        public double TotalDayInMonth { get; set; }
        public string Name { get; set; }

        public string OrganisationName { get; set; }

        public string LeaveTypeName { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
}
