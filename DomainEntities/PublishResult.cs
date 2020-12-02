using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class PublishResult
    {
        public int? ID { get; set; }

        [DisplayName("Session")]
        [Required(ErrorMessage = "Required")]
        public string SessionID { get; set; }

        [DisplayName("Class")]
        [Required(ErrorMessage = "Required")]
        public string ClassID { get; set; }

        [DisplayName("Faculty")]
        [Required(ErrorMessage = "Required")]
        public string FacultyID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Section { get; set; }

        [DisplayName("Term")]
        [Required(ErrorMessage = "Required")]
        public string TermID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Format { get; set; }

        public string AddedBy { get; set; }
    }
}
