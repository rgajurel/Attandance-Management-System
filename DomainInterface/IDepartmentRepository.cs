using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IDepartmentRepository
    {
        #region Admin
        bool AddUpdateDepartment(Department department);
        List<Department> GetAllDepartment();
        bool DeleteDepartment(int id);
        Department EditDepartment(int id);
        #endregion
    }
}
