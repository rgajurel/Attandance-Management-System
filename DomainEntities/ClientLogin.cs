using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ClientLogin
    {
        public int? ID { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password Must Be Atleast 6 To 30 Characters Long.")]
        public string Password { get; set; }

    }
}
