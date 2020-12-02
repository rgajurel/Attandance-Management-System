using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class Menu
    {
        public int MenuID { get; set; }
        [Column("MenuName")]
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string URI { get; set; }
        public string Slug { get; set; }
        public string Options { get; set; }
        public int ParentID { get; set; }
        public string IconClass { get; set; }
        public bool isAdmin { get; set; }
        public string Access { get; set; }
    }
}
