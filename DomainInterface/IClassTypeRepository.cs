using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface IClassTypeRepository
    {
        #region Admin
        bool AddUpdateClassType(ClassType classType);
        List<ClassType> GetAllClassType();
        bool DeleteClassType(int id);
        ClassType EditClassType(int id);
        #endregion
    }
}
