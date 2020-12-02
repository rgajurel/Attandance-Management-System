using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class HostelInfo
    {
        public int SN { get; set; }
        public int? ID { get; set; }

        [DisplayName("Hostel Name")]
        [Required(ErrorMessage = "Required")]
        public string HostelName { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage = "Required")]
        public string Address { get; set; }
        [DisplayName("Contact  Number")]
        [Required(ErrorMessage = "Required")]
        public string ContactNo { get; set; }

        [DisplayName("Incharge Name")]
        [Required(ErrorMessage = "Required")]
        public string PersonIncharge { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Incharge Phone Number")]
        public string InchargePhoneNo { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
