using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IMarksSheetPrintRepository
    {
        List<MarkSheetPrint> GetAllStudents(MarkSheetPrint marksheetprint);

        List<MarkShitStudentsPrint> GetAllMarkSheets(MarkSheetPrint marksheetprint);
        MarkSheetPrint GetStudentInfoForClient(string studentId, string session, string faculty, string section, string termId);
    }
}
