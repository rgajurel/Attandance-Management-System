using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    #region Entrance Report Listing 
    public class EntranceReport
    {
        public int EntranceID { get; set; }
        public string EntranceTitle { get; set; }
        public int TotalQuestion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int RowTotal { get; set; }
    }
    #region Entrance Report Search Param 
    public class EntranceReportSearch
    {
        public string SearchEntranceTitle { get; set; }
        public int SearchStatusID { get; set; } = -1;
        public string SearchStartedFrom { get; set; }
        public string SearchStartedTo { get; set; }
        public string SearchEndFrom { get; set; }
        public string SearchEndTo { get; set; }
        public string UserGroupID { get; set; } = "";
        public int EntranceCategory { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    #endregion
    #endregion

    #region Entrance Category Report
    public class EntranceCategoryReport
    {
        public string CategoryName { get; set; }
        public string EntranceTitle { get; set; }
        public int CorrectAnswer { get; set; }
        public int IncorrectAnswer { get; set; }
        public int SkippedAnswer { get; set; }
        public int RowTotal { get; set; }
    }
    #region Search Param For EntranceCategoryreport
    public class SearchParamEntranceCategoryreport
    {
        public int CategoryID { get; set; } = -1;
        public string Entrancetitle { get; set; } = "";
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    #endregion
    #endregion

    #region Entrance Question Report
    public class EntranceQuestionReport
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public int CorrectAnswer { get; set; }
        public int IncorrectAnswer { get; set; }
        public int SkippedAnswer { get; set; }
        public int RowTotal { get; set; }
    }
    #region Search Param For EntranceQuestionReport
    public class SearchParamEntranceQuestionreport
    {
        public string Question { get; set; } = "";
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    #endregion
    #endregion


    #region Entrance UserReport Report
    public class EntranceUserReport
    {
        public int UserID { get; set; }
        public int EntranceID { get; set; }
        public string UserName { get; set; }
        public string EntranceTitle { get; set; }
        public string EntranceStatus { get; set; }
        public DateTime JoinedDate { get; set; }
        public string CompletedDate { get; set; }
        public int CompletedTime { get; set; }
        public int Score { get; set; }
        public int RowTotal { get; set; }
        public int EntranceUserID { get; set; }
    }
    #region Search Param For EntranceUserReport
    public class SearchParamEntranceUserReport
    {
        public string SearchEntranceName { get; set; } = "";
        public string SearchUserGroup { get; set; } = "";
        public int SearchUserID { get; set; } = -1;
        public int SearchCompletionTime { get; set; } = -1;
        public string SearchJoinedFrom { get; set; } = "";
        public string SearchJoinedTo { get; set; } = "";
        public string SearchCompletedFrom { get; set; } = "";
        public string SearchCompletedTo { get; set; } = "";
        public string SearchEntranceStatus { get; set; } = "";
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    #endregion

    #region Get UserName And UserID
    public class EntranceAllUser
    {
        public string UserName { get; set; }
        public int UserID { get; set; }
    }
    #endregion
    #region Get User Entrance Question
    public class EntranceQuestionUserReport
    {
        public string EntranceTitle { get; set; }
        public string FullName { get; set; }
        public int UserID { get; set; }
        public int EntranceID { get; set; }
        public int EntranceUserCurrentQuestionID { get; set; }
        public string JoinedDate { get; set; }
        public string CompletedDate { get; set; }
        public string EntranceStatus { get; set; }
        public int TotalQuestion { get; set; }
        public int CorrectAnswer { get; set; }
        public int IncorrectAnswer { get; set; }
        public int FinalScore { get; set; }
        public string EntranceQuestion { get; set; }
        public string QuestionType { get; set; }
        public string UserSelectedAnswer { get; set; }
        public int UserEntranceQuestionAnswerID { get; set; }
        public int TotalEntranceScore { get; set; }
        public int TotalUserScore { get; set; }
        public string UserEntranceQuestion { get; set; }
    }
    #endregion
    #endregion
}
