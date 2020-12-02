using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public  class DataHolder
    {
        public int Code { get; set; }
        public bool isAuthorized { get; set; }
        public string Message { get; set; }
        public bool ErrorOccured { get; set; }
        public dynamic data { get; set; }
    }
}
