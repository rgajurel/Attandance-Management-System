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
  public   class SchoolInformation
    {
        public int SN { get; set; }
        public int? ID { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email is Required")]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required")]
        public string Mobile { get; set; }      
        public string Fax { get; set; }
        [Required(ErrorMessage = "Admin Contact Person is Required")]
        [DisplayName(" Admin Contact Person")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Registration No is Required")]
        [DisplayName("Registration No")]
        public string RegistrationNo { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Established Year")]
        public int EstablishedYear { get; set; }
        [Required(ErrorMessage = "Organisation Type Required")]
        [DisplayName("Organisation Type")]
        public int SchoolTypeID { get; set; }
        public HttpPostedFileBase imageFile { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Main Branch")]
        public int IsMainBranch { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }

    public static class DefaultImages
    {
        public static readonly string schoolImage = "/Content/Images/School/school.png";

        public static readonly string studentImage = "/Content/Images/Students/Students.png";

    }
}
