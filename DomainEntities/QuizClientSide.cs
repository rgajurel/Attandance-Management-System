using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public class QuizClientSide
    {
        public int QuizID { get; set; }
        public string QuizTitle { get; set; }
        public int TotalQuestionInQuiz { get; set; }
        public int TotalQuizAnswered { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string StatusName { get; set; }
        public int RowNum { get; set; }
        public int TotalQuiz { get; set; }
        public int CategoryID { get; set; }
        public string QuizSlug { get; set; }
        public string QuizDescription { get; set; }
        public string QuizImage { get; set; }
        public bool CanShowAllQuestions { get; set; }
        public string ProgressPercentage { get; set; }

        #region HTML content of Quiz
        public string RenderQuizInfo { get; set; }
        #endregion
    }
    #region Quiz Search
    public class QuizSearchingClientSide
    {

        //public int SearchQuizCategoryID { get; set; }
        public string SearchQuizTitle { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int SortBy { get; set; }
        public string UserName { get; set; }

    }
    #endregion
    #region Start Quiz Details
    public class QuizStartInfo
    {
        public string QuizID { get; set; }
        public string QuestionID { get; set; }
        public string StartPageDescription { get; set; }
        public string EndPageDescription { get; set; }
        public bool IsLast { get; set; }
        public bool IsFirst { get; set; }
        public string QuizQuestion { get; set; }
        public int RowNum { get; set; }
        public List<QuizAnswerInfo> QuizAnswerList { get; set; }
        public string[] CustomData { get; set; }
        public int TotalQuestion { get; set; }
        public string QuizTitle { get; set; }
        public List<string> AnswerID { get; set; }
        public string UserName { get; set; }
        public bool IsMultipleChoice { get; set; }
        public bool IsFreeWriting { get; set; }
        public string FreeWritingAnswer { get; set; }
        public bool CanShowCorrectAnswer { get; set; }
        public int Duration { get; set; }
        public bool IsMandatory { get; set; }
        public int TimeElapsed { get; set; }
        public bool HasQuizStarted { get; set; }
        public bool CanSeePreviousAnswer { get; set; }
        public bool IsPauseAllowed { get; set; }
        #region For FreeWriting Question
        public bool FreeWritingSkip { get; set; }
        public bool FreeWritingTimeOut { get; set; }
        #endregion


    }
    #endregion
    #region Start Quiz Question's Answer Information
    public class QuizAnswerInfo
    {
        public int AnswerPoolID { get; set; }
        public string QuizOption { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string UserSelectedOption { get; set; }
        public string CorrectOptions { get; set; }
        public bool IsObjective { get; set; }
        public bool IsApproved { get; set; }
        public string Detail { get; set; }
    }
    #endregion
    #region Quiz Report After Quiz Completion
    public class QuizCompletionReport
    {
        public int RowNum { get; set; }
        public int QuestionID { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsAnswerApproved { get; set; }
        public bool IsCorrect { get; set; }
        public int TotalQuestion { get; set; }
        public string QuizQuestion { get; set; }
        public int CorrectAnswerCount { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsTimeOut { get; set; }
        public bool IsQuizExpired { get; set; }
    }
    #endregion
    #region Pending Quiz and Survey Notification
    public class QuizAndSurveyPending
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
    #endregion
}
