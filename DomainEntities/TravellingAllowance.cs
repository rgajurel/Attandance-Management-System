using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TravellingAllowance
    {
        public int? ID { get; set; }
        public int SN { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "This Field is Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Employee")]
        [Required(ErrorMessage = "This Field is Required")]
        public int EmployeeID { get; set; }

        [DisplayName("Date From")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime DateFrom { get; set; }

        [DisplayName("Date From")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime NepaliDateFrom { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime DateTo { get; set; }

        [DisplayName("Date To")]
        [Required(ErrorMessage = "This Field is Required")]
        public DateTime NepaliDateTo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Status { get; set; }
        public string Statuss { get { if (Status == 0) { return "Paid"; } else { return "Not Paid"; } } }
        public decimal Amount { get; set; }
        public string Description { get; set; }



    }
}
