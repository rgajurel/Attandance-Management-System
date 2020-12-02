using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IFeeDailyCollectionRepository
    {
        DataSet getAllData(FeeDailyCollection fee);
    }
}
