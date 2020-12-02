using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IQuizReport
    {
        IEnumerable<QuizReport> GetAllQuizListing(QuizReportSearch objInfo);
        QuizEntity GetQuizByID(int QuizID);
    }
}
