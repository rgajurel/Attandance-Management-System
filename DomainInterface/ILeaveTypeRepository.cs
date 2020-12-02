using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface ILeaveTypeRepository
    {
        #region Admin
        bool AddUpdateLeaveType(LeaveType leaveType);
        List<LeaveType> GetAllLeaveType();
        bool DeleteLeaveType(int id);
        LeaveType EditLeaveType(int id);
        #endregion
    }
}
