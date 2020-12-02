using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IAccumulativeLeaveRepository
    {
        #region Admin
        List<Employee> GetAllEmployee(string prefix,int organisation);    

        List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id);
        bool AddUpdateAccumulativeLeave(AccumulativeLeave accumulative);
        List<AccumulativeLeave> GetAllAccumulativeLeave();
        AccumulativeLeave EditAccumulativeLeave(int id);
        #endregion
    }
}
