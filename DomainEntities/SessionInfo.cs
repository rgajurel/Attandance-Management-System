using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DomainEntities
{
   public class SessionInfo
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Academic Year")]
        [Required(ErrorMessage = "Academic Year is Required")]
        
        public string Session { get; set; }

        //  [Remote("IsActiveSessionExist", "SessionInfo", "Admin", HttpMethod = "POST", ErrorMessage = "Active Academic Year Already Exist")]

      
        [Required(ErrorMessage = "Required")]
        public string IsActive { get; set; } 
        public string ActiveOrNot { get { if (IsActive == "1") { return "Yes"; } else { return "No"; } } }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        
    }

    public class Months
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [DisplayName("Academic Year")]
        [Required(ErrorMessage = "Month is Required")]
        public string Month { get; set; }            


        [Required(ErrorMessage = "Required")]
        public string IsActive { get; set; }
        public string ActiveOrNot { get { if (IsActive == "1") { return "Yes"; } else { return "No"; } } }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }




    }
}
