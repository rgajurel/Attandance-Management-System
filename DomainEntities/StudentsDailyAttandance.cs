using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class StudentsDailyAttandance
    {
        public int? ID { get; set; }
        public int SN { get; set; }
        [DisplayName("Academic Year")]
        [Required(ErrorMessage = "Required")]
        public int SessionID { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]
        public int ClassID { get; set; }

        public string Class { get; set; }

        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public int FacultyID { get; set; }      

        public string Faculty { get; set; }

        [DisplayName("Section")]
        [Required(ErrorMessage = "Required")]
        public string Section { get; set; }

        [DisplayName("English Date")]
        public DateTime Date { get; set; }
       
        [DisplayName("Date")]
        [Required(ErrorMessage = "Date is Required")]
        public DateTime NepaliDate { get; set; }       
        public string StudentName { get; set; }
        public int RollNo { get; set; }
        public bool IsAttend { get; set; }
        public int StudentID { get; set; }

        public string InTime { get; set; }
        public string OutTime { get; set; }

        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

    }
}
