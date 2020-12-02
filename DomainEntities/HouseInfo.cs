using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    public class HouseInfo
    {
        public int? ID { get; set; }
        [DisplayName("House Name")]
        [Required(ErrorMessage = "House Name is Required")]
        public string HouseName { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
