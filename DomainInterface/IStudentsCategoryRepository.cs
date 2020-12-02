using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface IStudentsCategoryRepository
    {
        #region Admin
        bool AddUpdateStudentsCategory(StudentsCategorys studentCategory);
        List<StudentsCategorys> GetAllStudentsCategory();
        bool DeleteStudentsCategory(int id);
        StudentsCategorys EditStudentsCategory(int id);
        #endregion
    }
}
