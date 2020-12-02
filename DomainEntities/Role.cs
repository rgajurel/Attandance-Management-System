using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
     public class Role
    {
        public int RoleID { get; set; }
        [DisplayName("Role")]
        [Required(ErrorMessage = "Role Required")]
        public string Name { get; set; }
        public string Remarks { get; set; }
    }

    public class MenuRole
    {
        public string Name { get; set; }
        public string Options { get; set; }
        public string MenuName { get; set; }
        public int RoleID { get; set; }
        public int MenuID { get; set; }
    }
}
