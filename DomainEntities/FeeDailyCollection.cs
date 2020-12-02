using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foolproof;
namespace DomainEntities
{
    public class FeeDailyCollection
    {
        [DisplayName("Session")]
        [Required(ErrorMessage = "Required")]
        public string Session { get; set; }


        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public string Faculty { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]
        public string Class { get; set; }


        [DisplayName("Section")]
        [Required(ErrorMessage = "Required")]
        public string Section { get; set; }

        [DisplayName("Overall")]
        [Required(ErrorMessage = "Required")]
        public bool Overall { get; set; }

        [DisplayName("Date From")]
        [Required(ErrorMessage = "Required")]
        
        //[LessThanOrEqualTo("DateTo", ErrorMessage = "Must Be Less than Date To.")]
        public string DateFrom { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "Required")]
      
        //[GreaterThanOrEqualTo("DateFrom", ErrorMessage = "Must Be Greater than Date From.")]
        public string DateTo { get; set; }
    }
}
