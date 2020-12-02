using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ForgotPassword
    {
        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }


        [DisplayName("Student Name")]
        [Required(ErrorMessage = "Required")]
        public string studentName { get; set; }
        
    }
}
