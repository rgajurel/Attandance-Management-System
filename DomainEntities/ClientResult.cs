using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ClientResult
    {

    }
    public class PublishedTerm{
        public int ID { get; set; }

        [DisplayName("Select Exam")]
        public string Name { get; set; }
}
}
