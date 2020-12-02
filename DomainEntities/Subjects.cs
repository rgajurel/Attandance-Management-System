using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class Subjects
    {
        public int? ID { get; set; }

        public int SN { get; set; }
        [DisplayName("Subject Code")]
        [Required(ErrorMessage = " This field is Required")]
        public string SubjectCode { get; set; }

        [DisplayName("Subject Name")]
        [Required(ErrorMessage = " This field is Required")]
      
        public string SubjectName { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = " This field is Required")]
        public int ClassID { get; set; }

        public string Name { get; set; }

        [DisplayName("Full Marks (T)")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public string FullMarksTheory { get; set; }


        [DisplayName("Full Marks (P)")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public string FullMarksPractical { get; set; }


        [DisplayName("Pass Marks (T)")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public string PassMarksTheory { get; set; }


        [DisplayName("Pass Marks (P)")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public string PassMarksPractical { get; set; }

        [Required(ErrorMessage = " This field is Required")]
        [DisplayName("Credit Points")]
        public string CreditPoints { get; set; }

        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public List<Subjects> SubjectList { get; set; }

    }
}
