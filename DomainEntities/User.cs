using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DomainEntities
{
   public class User
    {
        public int SN { get; set; }
        public int? ID { get; set; }

        public string EmployeeID { get; set; }

        [DisplayName("Device UserID")]

      //  [Required(ErrorMessage = "UserID is Required")]
        public string UserID { get; set; }

        [DisplayName("Organisation")]
        [Required(ErrorMessage = "Organisation is Required")]
        public string OrganisationID { get; set; }

        public string OrganisationIDSearch { get; set; }

        public string OrganisationName { get; set; }

        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Full Name Required")]
        public string Name { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name Required")]
        [Remote("CheckExistingUserName", "Users", ErrorMessage = "UserName already exists!",AdditionalFields = "ID")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DisplayName("Password")]
       // [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
       // [RegularExpression("^((?=.*[a-z])(?=.*[A-Z])(?=.*\\d)).+$", ErrorMessage = "Enter atleast one upper case ,one lower case and a number")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password",
                    ErrorMessage = "The password and confirmation password do not match.")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is Required")]
       
        public string ConformPassword { get; set; }
        [DisplayName("Email")]
     //   [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public int Status { get; set; }


        public string ImageUrl { get; set; }
        public string StatusString {
           get { if (Status == 1) { return "Active"; } else { return "InActive"; } }
        }
        public bool IsClientUser { get; set; }
        public string Address { get; set; }
        public string UserGroupID { get; set; }
       
        [DisplayName("Role")]
        [Required(ErrorMessage = "Role Required")]
        public string RoleID { get; set; }
        public bool IsParentUser { get; set; }
        public bool IsStudentUser { get; set; }
        public bool IsAdmin { get; set; }       
        
        public string AddedBy { get; set; }
        public bool IsSuperAdmin { get; set; }

       
        //users search

        public string NameSearch { get; set; }
        public string UserNameSearch { get; set; }
        public int SearchStatus { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int offset { get; set; }
        public int Total { get; set; }

        //user device key
        public string DeviceAuthToken { get; set; }

    }

   
}
