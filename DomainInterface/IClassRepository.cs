using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IClassRepository
    {
        #region Admin
        bool AddUpdateClass(Class classs);
        List<Class> GetAllClass();
        bool DeleteClass(int id);
        Class EditClass(int id);
        #endregion
    }
}

