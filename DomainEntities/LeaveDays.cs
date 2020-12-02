using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class LeaveDays
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [DisplayName("Weight")]
        [Required(ErrorMessage = "Required")]
        public float Weight { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
