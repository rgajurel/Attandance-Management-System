using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntranceClientSide
    {
        public int EntranceID { get; set; }
        public string EntranceTitle { get; set; }
        public int TotalQuestionInEntrance { get; set; }
        public int TotalEntranceAnswered { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string StatusName { get; set; }
        public int RowNum { get; set; }
        public int TotalEntrance { get; set; }
        public int CategoryID { get; set; }
        public string EntranceSlug { get; set; }
        public string EntranceDescription { get; set; }
        public string EntranceImage { get; set; }
        public bool CanShowAllQuestions { get; set; }
        public string ProgressPercentage { get; set; }

        #region HTML content of Entrance
        public string RenderEntranceInfo { get; set; }
        #endregion
    }
    #region Entrance Search
    public class EntranceSearchingClientSide
    {

        //public int SearchQuizCategoryID { get; set; }
        public string SearchEntranceTitle { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int SortBy { get; set; }
        public string UserName { get; set; }

    }
    #endregion
    #region Start Entrance Details
    public class EntranceStartInfo
    {
        public string EntranceID { get; set; }
        public string QuestionID { get; set; }
        public string StartPageDescription { get; set; }
        public string EndPageDescription { get; set; }
        public bool IsLast { get; set; }
        public bool IsFirst { get; set; }
        public string EntranceQuestion { get; set; }
        public int RowNum { get; set; }
        public List<EntranceAnswerInfo> EntranceAnswerList { get; set; }
        public string[] CustomData { get; set; }
        public int TotalQuestion { get; set; }
        public string EntranceTitle { get; set; }
        public List<string> AnswerID { get; set; }
        public string UserName { get; set; }
        public bool IsMultipleChoice { get; set; }
        public bool IsFreeWriting { get; set; }
        public string FreeWritingAnswer { get; set; }
        public bool CanShowCorrectAnswer { get; set; }
        public int Duration { get; set; }
        public bool IsMandatory { get; set; }
        public int TimeElapsed { get; set; }
        public bool HasEntranceStarted { get; set; }
        public bool CanSeePreviousAnswer { get; set; }
        public bool IsPauseAllowed { get; set; }
        #region For FreeWriting Question
        public bool FreeWritingSkip { get; set; }
        public bool FreeWritingTimeOut { get; set; }
        public string Identifier { get; set; }
        public string Examinee { get; set; }
        #endregion


    }
    #endregion
    #region Start Entrance Question's Answer Information
    public class EntranceAnswerInfo
    {
        public int AnswerPoolID { get; set; }
        public string EntranceOption { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string UserSelectedOption { get; set; }
        public string CorrectOptions { get; set; }
        public bool IsObjective { get; set; }
        public bool IsApproved { get; set; }
        public string Detail { get; set; }
    }
    #endregion
    #region Entrance Report After Entrance Completion
    public class EntranceCompletionReport
    {
        public int RowNum { get; set; }
        public int QuestionID { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsAnswerApproved { get; set; }
        public bool IsCorrect { get; set; }
        public int TotalQuestion { get; set; }
        public string EntranceQuestion { get; set; }
        public int CorrectAnswerCount { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsTimeOut { get; set; }
        public bool IsEntranceExpired { get; set; }
    }
    #endregion
    #region Pending Entrance and Survey Notification
    public class EntranceAndSurveyPending
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
    #endregion
}
