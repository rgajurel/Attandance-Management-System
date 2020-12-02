using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IEntranceQuestionRepository
    {
        IEnumerable<EntranceQuestionEntity> GetAllEntranceQuestion(EntranceSearchQuestionEntity objInfo);

        IEnumerable<EntranceQuestionEntity> GetAllEntranceQuestionForEntrance(EntranceSearchQuestionEntity objInfo);

        bool AddUpdateEntranceQuestion(EntranceQuestionEntity objInfo);
        int DeleteEntranceQuestion(EntranceQuestionEntity objInfo);
        EntranceQuestionEntity GetEntranceQuestionByID(int QuestionID);
        IEnumerable<EntranceQuestionTypeEntity> GetAllEntranceQuestionType();
        IEnumerable<EntranceQuestionWeightageEntity> GetAllEntranceQuestionWeight();
        IEnumerable<EntranceQuestionDifficultyEntity> GetAllEntranceQuestionDifficulty();
        bool BatchUpdateEntranceQuestionStatus(string JsonObject);
        string GetStatusForBatchUpdateQuestionUpdate(string JsonObject);
        IEnumerable<CategoryTree> GetAllEntraceQuestionCategory(string CategoryType);
    }
}
