using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface IStudentAssignmentsRepository
    {
       #region Admin
        bool AddUpdateStudentsAssignments(StudentsAssignments studentsAssignments);
        List<StudentAssignmentsDetails> GetAllStudentsAssignments(StudentAssignmentsDetails search);
        bool DeleteStudentsAssignments(int id);
        StudentsAssignments EditStudentAssignments(int id);
        #endregion
    }
}
