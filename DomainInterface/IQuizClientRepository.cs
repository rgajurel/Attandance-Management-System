using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IQuizClientRepository
    {
        IEnumerable<QuizClientSide> GetQuizListingForClient(QuizSearchingClientSide objInfo);
        QuizStartInfo GetQuizQuestionAndAnswer(int QuizID, int QuestionID,string Username);
        bool IsAnswerCorrect(QuizStartInfo objInfo);
        IEnumerable<QuizCompletionReport> GetQuizReport(int QuizID,string UserName);
        void StartQuiz(QuizStartInfo objInfo);
        bool IsPaused(QuizStartInfo objInfo);
        void SetTimeElapsed(QuizStartInfo objInfo);
        QuizStartInfo GetPreviousQuestion(QuizStartInfo objInfo);
        QuizStartInfo GetNextQuestion(QuizStartInfo objInfo);
        QuizClientSide GetQuizDetailsFromSlug(string Slug,string UserName);
        IEnumerable<QuizClientSide> GetQuizListingForMyQuiz(QuizSearchingClientSide objInfo);
        IEnumerable<string> GetAllQuizQuestion(int QuizID);
        QuizClientSide GetQuizProgress(string UserName);
        IEnumerable<QuizAndSurveyPending> GetAllPendingQuizAndSurvey(string UserName);
        string GetErrorMessage(int QuizID, int QuestionID, string Username);
    }
}
