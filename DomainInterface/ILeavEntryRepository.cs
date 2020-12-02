using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface ILeavEntryRepository
    {
        List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id);
        List<LeaveEntry> GetAllLeaveEntry(LeaveEntry leaveEntry);
        int DeleteData(LeaveEntry leaveEntry,int year);
        int LeaveEntryBatchUpload(List<LeaveEntry> marksEntry,int Year);
        
    }
}
