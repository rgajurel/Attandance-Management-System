using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class UserLogin
    {
        
        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        public string  DeviceIdentifier { get; set; }
   
       
    }

    public class ChangePassword    {

        public string ID { get; set; }
        [DisplayName("Old Password")]
        [Required(ErrorMessage = "Required")]       
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [DisplayName("New Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password Must Be Atleast 6 To 30 Characters Long.")]
        [Compare(nameof(comparenewPassword), ErrorMessage = "New Password And Confirm New Password Doesn't match")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [DisplayName("Confirm New Password")]
        [Required(ErrorMessage = "Required")]
        [Compare(nameof(newPassword), ErrorMessage = "New Password And Confirm New Password Doesn't match")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password Must Be Atleast 6 To 30 Characters Long.")]
        [DataType(DataType.Password)]
        public string comparenewPassword { get; set; }

    }

    public class Image
    {
         public string UserImage { get; set; }

    }
}
