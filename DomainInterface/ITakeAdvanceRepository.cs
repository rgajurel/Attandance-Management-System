using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface ITakeAdvanceRepository
    {
        bool AddUpdateTakeAdvance(TakeAdvance takeAdvance);
        List<TakeAdvance> GetAllTakeAdvance();
        TakeAdvance EditTakeAdvance(int id);
        bool DeleteTakeadvance(int id);
    }
}
