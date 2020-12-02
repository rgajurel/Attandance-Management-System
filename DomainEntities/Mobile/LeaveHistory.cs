using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Mobile
{
   public class LeaveHistory
    {
        public string LeaveType { get; set; }
        public string RemainingDays { get; set; }         
        public string Total { get; set; }   


    }

    public class LeaveHistoryList
    {
        public List<LeaveHistory> LeaveHistory { get; set; }
        public bool IsHistoryAvailiable { get; set; }
        public string LeaveMessage { get; set; }
    }

    public class LeaveHistoryStatusList
    {
        public List<LeaveHistoryStatus> LeaveHistoryStatus { get; set; }
        public bool IsHistoryAvailiable { get; set; }
        public string LeaveMessage { get; set; }
    }

    public class LeaveHistoryStatus
    {
        public string LeaveType { get; set; }
        public string Days { get; set; }
        public string Status { get; set; }        
        public string FromDate { get; set; }  
       public string ToDate { get; set; } 
        public string LeaveStatusCode { get; set; }

    }


   


    public class TravelRequest
    {
        public string Days { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TravelStatusCode { get; set; }
        public string Description { get; set; }

    }


    public class TravelRequestList
    {
        public List<TravelRequest> TravelList { get; set; }
        public bool IsListAvailiable { get; set; }
        public string TravelMessage { get; set; }

    }

}
