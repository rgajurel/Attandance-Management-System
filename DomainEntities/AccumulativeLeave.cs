using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class AccumulativeLeave
    {
        public int? ID { get; set; }
        public int? SN { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Employee Name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="This Field is Required")]
        [DisplayName("Leave Type")]
        public int LeaveTypeID { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Year")]
        public int YearID { get; set; }
        public string LeaveType { get; set; }
        public string OrganisationName { get; set; }
        public int? EmployeeID { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Organisation")]
        public int OrganisationID { get; set; }

        [DisplayName("User ID")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Days")]
        public double Days { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }

    }

