using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    #region Quiz Report Listing 
    public class QuizReport
    {
        public int QuizID { get; set; }
        public string QuizTitle { get; set; }
        public int TotalQuestion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int RowTotal { get; set; }
    }
    #region Quiz Report Search Param 
    public class QuizReportSearch
    {
        public string SearchQuizTitle { get; set; }
        public int SearchStatusID { get; set; } = -1;
        public string SearchStartedFrom { get; set; }
        public string SearchStartedTo { get; set; }
        public string SearchEndFrom { get; set; }
        public string SearchEndTo { get; set; }
        public string UserGroupID { get; set; } = "";
        public int QuizCategory { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    #endregion
    #endregion

    #region Quiz Category Report
    public class QuizCategoryReport
    {
        public string CategoryName { get; set; }
        public string QuizTitle { get; set; }
        public int CorrectAnswer { get; set; }
        public int IncorrectAnswer { get; set; }
        public int SkippedAnswer { get; set; }
        public int RowTotal { get; set; }
    }
    #region Search Param For QuizCategoryreport
    public class SearchParamQuizCategoryreport
    {
        public int CategoryID { get; set; } = -1;
        public string Quiztitle { get; set; } = "";
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    #endregion
    #endregion

    #region Quiz Question Report
    public class QuizQuestionReport
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public int CorrectAnswer { get; set; }
        public int IncorrectAnswer { get; set; }
        public int SkippedAnswer { get; set; }
        public int RowTotal { get; set; }
    }
    #region Search Param For QuizQuestionReport
    public class SearchParamQuizQuestionreport
    {
        public string Question { get; set; } = "";
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    #endregion
    #endregion


    #region Quiz UserReport Report
    public class QuizUserReport
    {
        public int UserID { get; set; }
        public int QuizID { get; set; }
        public string UserName { get; set; }
        public string QuizTitle { get; set; }
        public string QuizStatus { get; set; }
        public DateTime JoinedDate { get; set; }
        public string CompletedDate { get; set; }
        public int CompletedTime { get; set; }
        public int Score { get; set; }
        public int RowTotal { get; set; }
        public int QuizUserID { get; set; }
    }
    #region Search Param For QuizUserReport
    public class SearchParamQuizUserReport
    {
        public string SearchQuizName { get; set; } = "";
        public string SearchUserGroup { get; set; } = "";
        public int SearchUserID { get; set; } = -1;
        public int SearchCompletionTime { get; set; } = -1;
        public string SearchJoinedFrom { get; set; } = "";
        public string SearchJoinedTo { get; set; } = "";
        public string SearchCompletedFrom { get; set; } = "";
        public string SearchCompletedTo { get; set; } = "";
        public string SearchQuizStatus { get; set; } = "";
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    #endregion

    #region Get UserName And UserID
    public class QuizAllUser
    {
        public string UserName { get; set; }
        public int UserID { get; set; }
    }
    #endregion
    #region Get User Quiz Question
    public class QuizQuestionUserReport
    {
        public string QuizTitle { get; set; }
        public string FullName { get; set; }
        public int UserID { get; set; }
        public int QuizID { get; set; }
        public int QuizUserCurrentQuestionID { get; set; }
        public string JoinedDate { get; set; }
        public string CompletedDate { get; set; }
        public string QuizStatus { get; set; }
        public int TotalQuestion { get; set; }
        public int CorrectAnswer { get; set; }
        public int IncorrectAnswer { get; set; }
        public int FinalScore { get; set; }
        public string QuizQuestion { get; set; }
        public string QuestionType { get; set; }
        public string  UserSelectedAnswer { get; set; }
        public int UserQuizQuestionAnswerID { get; set; }
        public int TotalQuizScore { get; set; }
        public int TotalUserScore { get; set; }
        public string UserQuizQuestion { get; set; }
    }
    #endregion
    #endregion
}
