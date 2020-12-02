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
   public class UserGroup
    {
        public int? ID { get; set; }
        public int SN { get; set; }
        [DisplayName("Group Name")]
        [Required(ErrorMessage = "This Field is Required")]
       // [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = "Special character should not be entered")]       
        [Remote("CheckExistingUserGroup", "UserGroup", ErrorMessage = "UserGroup Already exists!", AdditionalFields = "ID")]
        public string GroupName { get; set; }
        public string Name { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "This Field is Required")]
        public int OrganisationID { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "This Field is Required")]
        public int StatusValue { get; set; }
        public string ActiveOrNot { get { if (StatusValue == 1) { return "Active"; } else { return "InActive"; } } }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }


        public int Total { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }

    }
        
}
