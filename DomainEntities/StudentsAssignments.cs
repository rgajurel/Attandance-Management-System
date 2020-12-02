using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DomainEntities
{
  public class StudentsAssignments
    {
       
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
        [DisplayName("Notification Type")]
        [Required(ErrorMessage = "Required")]
        public string NotificationType { get; set; }

        [DisplayName("User Group")]
        [Required(ErrorMessage = "Required")]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "Required")]
        public HttpPostedFileBase imageFile { get; set; }

        public string Image { get; set; }
        //for studets marks entry

        public DateTime Deadline { get; set; }

        public DateTime NepaliDeadline { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }  
             

    }

    public class StudentAssignmentsDetails
    {
        public int ID { get; set; }
        public int SN { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Faculty { get; set; }
        public string Subject { get; set; }
        public string FileNmae { get; set; }
        public string file { get { return FileNmae.Split('/')[3]; } }

        public int Total { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

        [DisplayName("Class")]

        public int SearchClassID { get; set; }

        [DisplayName("Faculty")]
        public int SearchFacultyID { get; set; }
       
        public string SectionSearch { get; set; }
        [DisplayName("Subject")]
        public int SearchSubjectID { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime NepaliDeadline { get; set; }


        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }


    }




}
