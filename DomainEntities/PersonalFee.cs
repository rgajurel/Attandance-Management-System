using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class PersonalFee
    {
        public int? ID { get; set; }

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

        [DisplayName("Fee Type")]
        [Required(ErrorMessage = "Required")]
        public string Type { get; set; }

        [DisplayName("Month")]
        [Required(ErrorMessage = "Required")]
        public string Month { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RollNo { get; set; }

        [RegularExpression("^[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Only Numbers are Allowed")]
        [Required(ErrorMessage = "Required")]
        [Range(0.0, Double.MaxValue)]
        public decimal Fee { get; set; }

        [RegularExpression("^[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Only Numbers are Allowed")]
        [Required(ErrorMessage = "Required")]        
        public decimal Discount { get; set; }

        public string SN { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }


        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

    }
}
