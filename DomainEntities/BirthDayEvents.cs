using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BirthDayEvents
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string Panel { get; set; }

        public string dates
        {
            get
            { 
                return Date.ToShortDateString();
            }
        }
        public bool IsToday
        {
            get
            {
                if (Date.Date == DateTime.Now.Date)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
    }
}

