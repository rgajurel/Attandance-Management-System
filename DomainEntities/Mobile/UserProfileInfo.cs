using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Mobile
{
   public class UserProfileInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string TemporaryAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ImageUrl { get; set; }
    }
}
