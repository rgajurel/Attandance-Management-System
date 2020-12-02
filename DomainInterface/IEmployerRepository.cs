using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IEmployerRepository
    {
        #region Admin


        List<Department> GetDepartmentBasedOnOrganisation(string id);

        List<Designations> GetDesignationBasedOnOrganisation(string id);
        bool AddUpdateEmployee(Employee employer);

        Employee GetEmployeeByUserID(string userid);
        List<EmployeeSearch> GetAllEmployee(EmployeeSearch search);


        Employee EditEmployeeDeviceUserID(int id);
        Employee EditEmployee(int id);

        UniqueNoGeneration GetUniqueDeivceID();

        Employee DetailsEmployer(int id);
        #endregion


    }
}
