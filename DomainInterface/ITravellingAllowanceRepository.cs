using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface ITravellingAllowanceRepository
    {
        bool AddUpdateTravellingAllowance(TravellingAllowance leave);
        List<TravellingAllowance> GetAllTravellingAllownace(TravellingAllowance search);
    }
}
