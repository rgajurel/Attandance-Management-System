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
  public  class MarksEntry
    {
        //for search
        public bool IsAdmin { get; set; }
        public int? ID { get; set; }
        public int StudentID { get; set; }
        public int SN { get; set; }

        [DisplayName("Academic Year")]
        [Required(ErrorMessage = "Required")]
        public int SessionID { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]
        public int ClassID { get; set; }

        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public int FacultyID { get; set; }

        [DisplayName("Section")]
        [Required(ErrorMessage = "Required")]
        public string Section { get; set; }

        [DisplayName("Subject")]
        [Required(ErrorMessage = "Required")]
        public int SubjectID { get; set; }

        [DisplayName("Term")]
        [Required(ErrorMessage = "Required")]
        public int TermID { get; set; }


        //for studets marks entry

        public string StudentName { get; set; }

        [DisplayName("Full Marks Theory")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int FullMarksTheory { get; set; }

        [DisplayName("Pass Marks Theory")]
        [Required(ErrorMessage = "Required")]       
        [LessThanOrEqualTo("FullMarksTheory",ErrorMessage = "Must be Less than FullMarksTheory")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int PassMarksTheory { get; set; }

        [DisplayName("Full Marks Practical")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int FullMarksPractical { get; set; }

        [DisplayName("Pass Marks Practical")]
        [Required(ErrorMessage = "Required")]
        [LessThanOrEqualTo("FullMarksPractical", ErrorMessage = "Must be Less than FullMarksPractical")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int PassMarksPractical { get; set; }

        [DisplayName("Credit Point")]
        public string CreditPoint { get; set; }

        [DisplayName("Credit Point Update")]
        public string CreditPointUpdate { get; set; }

        public decimal ObtainedMarksTheory { get; set; }

        public decimal ObtainedMarksPractical { get; set; }

        public string ObtainedGradeTheory { get; set; }
        public string ObtaindedGradePractical { get; set; }

        public string FinalGrade { get; set; }

        public string GradePoint { get; set; }

        public string AddedBy { get; set; }

        public DateTime AddedOn { get; set; }


        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        //UpdatedFullMarksPassMarks

        [DisplayName("Full Marks Theory")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int FullMarksTheoryupdate { get; set; }

        [DisplayName("Pass Marks Theory")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        [LessThanOrEqualTo("FullMarksTheoryupdate", ErrorMessage = "Less than FullMarksTheory Update")]
     
        public int PassMarksTheoryUpdate { get; set; }

        [DisplayName("Full Marks Practical")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        public int FullMarksPracticalUpdate { get; set; }

        [DisplayName("Pass Marks Practical")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only Numbers are Allowed")]
        [LessThanOrEqualTo("FullMarksPracticalUpdate", ErrorMessage = "Less than FullMarksPractical Update")]     
        public int PassMarksPracticalUpdtae { get; set; }

    }
}
