using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ITakeAccumulativeLeaveRepository
    {
        string CalculateRemainingLeave(TakeLeave takeleave);
        List<TakeLeave> GetAllAccumulativeLeave(TakeLeave search);
        bool AddUpdateTakeAccumulativeLeave(TakeLeave leave);
        bool DeleteTakeAccumulativeLeave(int id);
    }
}
