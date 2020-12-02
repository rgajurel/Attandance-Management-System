using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IShoolTypeRepository
    {
        #region Admin
        bool AddUpdateSchoolType(SchoolType schoolType);
        List<SchoolType> GetAllSchoolType();
        bool DeleteSchoolType(int id);
        SchoolType EditSchoolType(int id);
        #endregion

    }
}
