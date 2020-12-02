using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
 public class BusInfo
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Bus Number")]
        [Required(ErrorMessage = "Required")]
        public string BusNo { get; set; }
        [DisplayName("Driver Name")]
        [Required(ErrorMessage = "Required")]
        public string DriverName { get; set; }
        [DisplayName("Driver Phone Number")]
        [Required(ErrorMessage = "Required")]
        public string DriverPhoneNo { get; set; }

        [DisplayName("Supporter Name")]
      
        public string SupporterName { get; set; }
        [DisplayName("Supporter Phone Number")]
        public string SupporterPhoneNo { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
