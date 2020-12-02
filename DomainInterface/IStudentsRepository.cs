using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IStudentsRepository
    {
        #region Admin
        UniqueNoGeneration GetUniqueRegistrationNo(string batch);
        Class GetSectionBasedOnClass(string classid,string facultyid);

       List<Facultys> GetFacultyBasedOnClass(string id);

        int GetClassRollNoCount(string faculty,string classs,string section, string rollno);
        bool AddUpdateStudents(Students classs);
        List<StudentsSearch> GetAllStudents(StudentsSearch search);
        bool DeleteStudents(int id);
        Students EditStudents(int id);
        List<StudentsSearch> GetAllStudents(string prefix);
        Students DetailsStudents(int id);

        #endregion
    }
}
