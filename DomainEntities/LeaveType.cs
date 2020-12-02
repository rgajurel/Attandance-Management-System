using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class LeaveType
    {
        public int SN { get; set; }

        [DisplayName("Organisation ")]       
        [Required(ErrorMessage = "Required")]
        public int OrganisationID { get; set; }
        public int? ID { get; set; }
        [DisplayName("Leave Type")]
        
        [Required(ErrorMessage = "Required")]
        public string LeaveTypeName { get; set; }

        [DisplayName("IsAccumulative")]
        public bool IsAccumulativeLeave { get; set; }
        public string IsAccumulativeLeaveOrNot { get { if (IsAccumulativeLeave == true) { return "Yes"; } else { return "No"; } } }

        [DisplayName("Attandance Leave")]
        public bool IsAttandanceLeave { get; set; }

        [DisplayName("Expire Leave")]
        public bool IsExpireLeave { get; set; }

        public string IsExpireLeaveOrNot { get { if (IsExpireLeave == true) { return "Yes"; } else { return "No"; } } }
        public string IsAttandanceLeaveOrNot { get { if (IsAttandanceLeave == true) { return "Yes"; } else { return "No"; } } }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
