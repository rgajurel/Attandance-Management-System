using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ITaxMasterRepository
    {
        #region Admin
        bool AddUpdateTextMaster(TaxMaster taxMaster);
        List<TaxMaster> GetAllTaxMaster();
        bool DeleteTaxMaster(int id);
        TaxMaster EditTaxMaster(int id);
        #endregion
    }
}
