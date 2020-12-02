using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class YearlyHolidaysEntry
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Holiday Name")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = "Special character should not be entered")]
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Required")]
        public int OrganisationID { get; set; }


        [DisplayName("Year")]
        [Required(ErrorMessage = "Required")]
        public int YearID { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime Date { get; set; }

        public string OrganisationName { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
