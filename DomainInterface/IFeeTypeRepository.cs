using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
 public interface IFeeTypeRepository
    {
        #region Admin
        bool AddUpdateFeeType(FeeType feeType);
        List<FeeType> GetAllFeeType();
        bool DeleteFeeType(int id);
        FeeType EditFeeType(int id);
        #endregion
    }
}
