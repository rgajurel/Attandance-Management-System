using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IClassMasterRepository
    {
        #region Admin
        bool AddUpdateClassMaster(ClassMaster classmaster);
        List<ClassMaster> GetAllClassMaster();
        bool DeleteClassMaster(int ID);
        ClassMaster EditClassMaster(int id);      

        #endregion
    }
}
