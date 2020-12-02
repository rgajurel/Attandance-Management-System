using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class CommonFeeDiscount
    {
        public int? ID {get;set;}

        [DisplayName("Session")]
        [Required(ErrorMessage ="Required")]
        public string Session { get; set; }

        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public string Faculty { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]
        public string Class { get; set; }

        [DisplayName("Section")]
        [Required(ErrorMessage ="Required")]
        public string Section { get; set; }

        [DisplayName("Fee Type")]
        [Required(ErrorMessage ="Required")]
        public string Type { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Required")]
        public string Month { get; set; }

        
        public int SN { get; set; }
        public int StudentId { get; set; }
        public string RollNo { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }

        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        [LessThanOrEqualTo("Fee", ErrorMessage = "Must Be Less than Fee.")]
        public decimal Discount { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

    }
}
