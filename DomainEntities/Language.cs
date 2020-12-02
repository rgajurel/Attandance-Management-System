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
  public  class Language
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = "Special character should not be entered")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        public string Status { get; set; }
        public string Statuss { get { if (Status == "0") { return "Active"; } else { return "InActive"; } } }


        [Required(ErrorMessage = "Required")]
        public HttpPostedFileBase ImageFile { get; set; }

        public string Image { get; set; }

        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
