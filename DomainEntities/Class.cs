using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class Class
    {
        public int? ID { get; set; }
        public int SN { get; set; }

        [DisplayName("Class Type")]
        [Required(ErrorMessage = "Required")]
        public int ClassTypeID  { get; set; }


        public string ClassName { get; set; }
        public string Faculty { get; set; }

        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public int FacultyID { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]      
       // [MaxLength(10)]
        public int ClassID { get; set; }

        [DisplayName("Sections")]
       // [Required(ErrorMessage = "Required")]
        public string SectionID { get; set; }
        public List<string> SectionArray { get; set; }
       
        public string Sections { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }



    }
}
