using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DomainEntities
{
    #region Entrance Add 
    public class EntranceEntity
    {
        [Required]
        public int EntranceID { get; set; }
        public int? CourseID { get; set; }
        public int? ChapterID { get; set; }
        [Required(ErrorMessage = "Entrance Title Required")]
        public string EntranceTitle { get; set; }
        [Required(ErrorMessage = "StartDate Required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate Required")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "TotalQuestion Required")]
        public int TotalQuestion { get; set; }
        [Required(ErrorMessage = "EntranceAppearingPoints Required")]
        public decimal EntranceAppearingPoints { get; set; }
        [Required(ErrorMessage = "Status Required")]
        public int StatusValue { get; set; }
        [Required(ErrorMessage = "CanShowCorrectAnswer Required")]
        public bool CanShowCorrectAnswer { get; set; }
        [Required(ErrorMessage = "CanSeePreviousAnswer Required")]
        public bool CanSeePreviousAnswer { get; set; }
        [Required(ErrorMessage = "IsPauseAllowed Required")]
        public bool IsPauseAllowed { get; set; }
        [Required(ErrorMessage = "CanShowAllQuestions Required")]
        public bool CanShowAllQuestions { get; set; }
        [Required(ErrorMessage = "StartPageDescription Required")]
        [AllowHtml]
        public string StartPageDescription { get; set; }
        [Required(ErrorMessage = "EndPageDescription Required")]
        [AllowHtml]
        public string EndPageDescription { get; set; }
        public string AddedBy { get; set; }
        public int? SortOrder { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int EntranceQuestionID { get; set; }
        public int QuestionID { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Notification Required")]
        public string NotificationID { get; set; }
        public string SelectedAnswers { get; set; }
        public string StatusName { get; set; }
        public int RowTotal { get; set; }
        public string JSONDATA { get; set; }
        public string NotifyBy { get; set; }
        public bool IsQuestionManual { get; set; }
        [Required(ErrorMessage = "QuizDescription Required")]
        [AllowHtml]
        public string EntranceDescription { get; set; }
        public string EntranceImage { get; set; }
        public QuizDynamicQuestion QuestionDynamicList { get; set; }
        public string DateFormat { get; set; }
        public string CourseCode { get; set; }
        public string UserGroup { get; set; }
        [Required(ErrorMessage = "MetaTitle Required")]
        public string MetaTitle { get; set; }
        [DisplayName("Meta Description")]
        public string MetaDescription { get; set; }
        [DisplayName("Meta Keyword")]
        public string MetaKeyword { get; set; }
        [DisplayName("Tag")]
        public string Tag { get; set; }
        public string QuizSlug { get; set; }
        public bool IsUpdatable { get; set; }
        public int Priority { get; set; }
        public bool NotifyNow { get; set; }
        #region For Entrance Report
        public int TotalEntranceQuestionForAll { get; set; }
        public int TotalCorrectAnswerForAll { get; set; }
        public int TotalInCorrectAnswerForAll { get; set; }
        public float AverageTimeOnEntrance { get; set; }
        public int TotalUserInEntrance { get; set; }
        public string JsonUserInfo { get; set; }
        #endregion

    }
    #endregion
    //#region Entrance Course Dropdown
    //public class QuizCourse
    //{
    //    public int CourseID { get; set; }
    //    public string FullCourseName { get; set; }
    //}
    //#endregion
    //#region Quiz Notification Dropdown
    //public class QuizNotification
    //{
    //    public int NotificationID { get; set; }
    //    public string NotificationTitle { get; set; }
    //}
    //#endregion
    #region Entrance Searching
    public class SearchEntranceParam
    {
        public string SearchStartedFrom { get; set; }
        public string SearchStartedTo { get; set; }
        public string SearchEndFrom { get; set; }
        public string SearchEndTo { get; set; }
        public int SearchStatusID { get; set; }
        public List<int> SearchUserGroup { get; set; }
        public string SearchEntranceTitle { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
    #endregion
    #region Entrance Question Dynamic
    public class EntranceDynamicQuestion
    {
        public int[] ID { get; set; }
        public int[] EntranceQuestionMandatoryNo { get; set; }
        public int[] EntranceQuestionOptionalNo { get; set; }
        public int[] QuestionCategory { get; set; }
        public int[] QuestionDifficulty { get; set; }
    }
    #endregion
}
