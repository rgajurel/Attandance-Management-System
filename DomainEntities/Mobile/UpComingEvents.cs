using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Mobile
{
   public class UpComingEvents
    {
        public string EventName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string Message { get; set; }
    }

    public class UpComingEventsList
    {
        public List<UpComingEvents> Events { get; set; }
        public string Message { get; set; }
    }

    public class Holidays
    {
        public string Title { get; set; }
        public string Date { get; set; }
        
    }

    public class HolidayList
    {
        public string Message { get; set; }
        public List<Holidays> Holidays { get; set; }
    }

   
}
