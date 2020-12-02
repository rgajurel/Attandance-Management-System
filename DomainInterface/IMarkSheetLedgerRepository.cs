using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IMarkSheetLedgerRepository
    {
        List<MarksSheetLedger> GetAllMarksSheetLedger(MarksEntry marksentry);
    }
}
