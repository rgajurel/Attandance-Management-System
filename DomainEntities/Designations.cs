using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class Designations
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Designation")]
        [Required(ErrorMessage = "Required")]
        public string Designation { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Required")]
        public int OrganisationID { get; set; }

        public string Name { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
