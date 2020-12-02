using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class StudentsSubject
    {
        public int ID { get; set; }
        public int SN { get; set; }
        public int ClassID { get; set; }
        public string StudentName { get; set; }
        public int StudentID { get; set; }
        public int FacultyID { get; set; }        
        public string Section { get; set; }             
        public int SubjectID { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

    }
}
