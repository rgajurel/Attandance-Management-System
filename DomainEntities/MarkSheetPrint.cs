using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class MarkSheetPrint
    {
        public int SN { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public bool IsCheck { get; set; }

        [DisplayName("Session")]
        [Required(ErrorMessage ="This Field is Required")]
        public int SessionID { get; set; }
        public string Session { get; set; }
        public string Faculty { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "This Field is Required")]
        public int ClassID { get; set; }

        [DisplayName("Faculty")]
        [Required(ErrorMessage = "This Field is Required")]
        public int FacultyID { get; set; }
       
        public string  Class { get; set; }
        public string RollNo { get; set; }        
        public string PresentDays { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string Section { get; set; }

        [DisplayName("Term")]
        [Required(ErrorMessage = "This Field is Required")]
        public int TermID { get; set; }
        public string Term { get; set; }
        public int ResultType { get; set; }



    }

    public class MarkShitStudentsPrint
    {

        public bool IsFinal { get; set; }
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public string  Class { get; set; }
        public string TermName { get; set; }
        public string StudentName { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public string  FatherName { get; set; }
        public string MotherName { get; set; }
        public string Section { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }

        public string RollNo { get; set; }
        public string ActiveSession { get; set; }
        public string SchoolName { get; set; }
        public decimal TotalFM { get; set; }
        public decimal TotalPM { get; set; }
        public decimal TotalObtained { get; set; }
        public decimal Percentage { get; set; }

        public string FinalGrade { get; set; }

        public string GradePoint { get; set; }

        public List<ResulTypeMarkSheet> AllResultData { get; set; }

        public List<TermMaster> AllTermsForHeadings { get; set; }


    }

    public class ResulTypeMarkSheet
    {
        public int StudentID { get; set; }
        public string SubjectName { get; set; }
        public string FM { get; set; }

        public int FinalTotal { get; set; }

        public int TermPercentage { get; set; }
        public string PM { get; set; }
        public decimal Obtained { get; set; }
        public string Grade { get; set; }
        public string GradePoint { get; set; }
        public string HighestMarksObtained { get; set; }
        public int TermID { get; set; }
        public decimal TotalObtained { get; set; }
        public decimal TotalMarks { get; set; }
        public bool  isFinalTerm {get;set;}
        public string CreditPoint { get; set; }

        public string ObtainedGradeTheory { get; set; }

        public string ObtaindedGradePractical { get; set; }

        public string HighestGradeObtained { get; set; }

        public List<TermMaster> AllTerms { get; set; }


    }


}
