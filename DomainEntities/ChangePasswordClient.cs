using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    class ChangePasswordClient
    {
        public string email { get; set; }

        [DisplayName("Old Password")]
        public string oldPassword { get; set; }

        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Invalid password format")]
        public string newPassword { get; set; }

        [DisplayName("Retype Password")]
        [DataType(DataType.Password)]
        //[RegularExpression("(?=.*\d)(?=.*[a-z])(?=.*[!@#$%&\/=?_.-])).{7,15}$", ErrorMessage = "Invalid password format")]
        public string reNewPassword { get; set; }
    }
}
