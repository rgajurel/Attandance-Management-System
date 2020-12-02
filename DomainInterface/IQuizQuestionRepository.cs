using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IQuizQuestionRepository
    {
        IEnumerable<QuizQuestionEntity> GetAllQuizQuestion(QuizSearchQuestionEntity objInfo);

        IEnumerable<QuizQuestionEntity> GetAllQuizQuestionForQuiz(QuizSearchQuestionEntity objInfo);

        bool AddUpdateQuizQuestion(QuizQuestionEntity objInfo);
        int DeleteQuizQuestion(QuizQuestionEntity objInfo);
        QuizQuestionEntity GetQuizQuestionByID(int QuestionID);
        IEnumerable<QuizQuestionTypeEntity> GetAllQuizQuestionType();
        IEnumerable<QuizQuestionWeightageEntity> GetAllQuizQuestionWeight();
        IEnumerable<QuizQuestionDifficultyEntity> GetAllQuizQuestionDifficulty();
        bool BatchUpdateQuizQuestionStatus(string JsonObject);
        string GetStatusForBatchUpdateQuestionUpdate(string JsonObject);
    }
}
