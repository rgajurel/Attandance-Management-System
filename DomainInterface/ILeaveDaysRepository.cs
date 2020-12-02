using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface ILeaveDaysRepository
    {
        #region Admin
        bool AddUpdateLeaveType(LeaveDays leaveDays);
        List<LeaveDays> GetAllLeaveType();
        bool DeleteLeaveType(int id);
        LeaveDays EditLeaveType(int id);
        #endregion
    }
}
