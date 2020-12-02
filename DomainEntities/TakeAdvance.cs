using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class TakeAdvance
    {
        public int? ID { get; set; }
        public int? SN { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Employee")]
        [Required(ErrorMessage = "Required")]
        public int EmployeeID { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime Date { get; set; }

        [DisplayName("Nepali Date")]
       
        public DateTime NepaliDate { get; set; }
        [DisplayName("Amount")]
        [Required(ErrorMessage = "Required")]
        public decimal Amount { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Required")]
        public int Month { get; set; }

        [DisplayName("Year")]
        [Required(ErrorMessage = "Required")]
        public int Year { get; set; }

        public string Organisation { get; set; }
        public string Employee { get; set; }
        public string Years { get; set; }
        public string Months { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
