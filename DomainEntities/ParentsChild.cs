using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class ParentsChild
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }
        public string AcademicYear { get; set; }
        public string RegistrationNo { get; set; }

        public string Faculty { get; set; }

        public string Class { get; set; }

        public string Section { get; set; }

        public string RollNo { get; set; }

        public string Batch { get; set; }

        public string Email { get; set; }

        public string StudentName { get; set; }

        public string EnglishDateOfBirth { get; set; }

        public string NepaliDateOfBirth { get; set; }

        public string Gender { get; set; }

        public string BloodGroup { get; set; }
    }
}
