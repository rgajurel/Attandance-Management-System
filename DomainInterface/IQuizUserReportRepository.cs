using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IQuizUserReportRepository
    {
        IEnumerable<QuizUserReport> GetAllQuizUserListing(SearchParamQuizUserReport objInfo);
        IEnumerable<QuizAllUser> GetAllUserForQuiz();
        QuizQuestionUserReport UserQuizAnswerByUserID(int QuizUserID);
        bool AssignMarktoUser(int UserScore, int ID);
    }
}
