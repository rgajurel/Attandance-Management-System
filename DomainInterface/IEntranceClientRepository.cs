using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IEntranceClientRepository
    {
        IEnumerable<EntranceClientSide> GetEntranceListingForClient(EntranceSearchingClientSide objInfo);
        EntranceStartInfo GetEntranceQuestionAndAnswer(int EntranceID, int QuestionID, string Username,string Identifier);
        bool IsAnswerCorrect(EntranceStartInfo objInfo);
        IEnumerable<EntranceCompletionReport> GetEntranceReport(int EntranceID, string UserName,string Identifier);
        void StartEntrance(EntranceStartInfo objInfo);
        bool IsPaused(EntranceStartInfo objInfo);
        void SetTimeElapsed(EntranceStartInfo objInfo);
        EntranceStartInfo GetPreviousQuestion(EntranceStartInfo objInfo);
        EntranceStartInfo GetNextQuestion(EntranceStartInfo objInfo);
        EntranceClientSide GetEntranceDetailsFromSlug(string Slug, string UserName);
        IEnumerable<EntranceClientSide> GetEntranceListingForMyEntrance(EntranceSearchingClientSide objInfo);
        IEnumerable<string> GetAllEntranceQuestion(int EntranceID);
        EntranceClientSide GetEntranceProgress(string UserName);
        IEnumerable<EntranceAndSurveyPending> GetAllPendingEntranceAndSurvey(string UserName);
        string GetErrorMessage(int EntranceID, int QuestionID, string Username,string Identifier);
    }
}
