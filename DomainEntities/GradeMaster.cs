using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SubSubject
    {
        public int? ID { get; set; }
        public int ClassID{get;set;}
        public string SubSubjectName { get; set; }
    }
   public class GradeMaster
    {
        public int? ID { get; set; }
        public int SN { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Grade")]
        public string Grade { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Grade Point")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Grade")]
        public string GradePoint { get; set; }
   

        [DisplayName("Marks From")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9][0-9]?$|^100$", ErrorMessage = "Only Numbers upto 100 ")]
        public int MarksFrom { get; set; }

        [DisplayName("Marks To")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9][0-9]?$|^100$", ErrorMessage = "Only Numbers upto 100")]
        public int MarksTo { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }


    }
}
