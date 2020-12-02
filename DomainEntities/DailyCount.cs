using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DailyCount
    {
        public int TotalAttend { get; set; }
        public DateTime Date { get; set; }

        public string Category { get; set; }
       
      public string Dates { get { return Date.ToShortDateString(); } }
    }

    public class StudentTotalByClass
    {
        public int Total { get; set; }
        public string  Class { get; set; }
       
    }
}
