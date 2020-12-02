using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IQuizRepository
    {
        IEnumerable<QuizNotification> GetAllNotification(int Identifier);
        IEnumerable<QuizCourse> GetAllCourseForQuiz();
        QuizEntity AddUpdateQuiz(QuizEntity objInfo);
        IEnumerable<QuizEntity> GetAllQuizListing(SearchQuizParam objInfo);
        QuizEntity GetQuizByID(int QuizID);
        int DeleteQuizByID(int QuizID);
        bool BatchUpdateQuiz(string JsonObject);
        string GetBatchUploadStatus(string JsonObject);



    }
}
