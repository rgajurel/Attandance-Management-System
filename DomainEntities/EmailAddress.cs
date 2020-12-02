using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class EmailAddress
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }

    public class EmailSenderReceiverData
    {
        public string SMTPHost { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public int SMTPPort { get; set; }
        public string EmailTo { get; set; }
    }
}
