using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class QuizQuestionEntity
    {
        public int QuestionID { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("<script[\\d\\D]*?>[\\d\\D]*?</script>")]
        public int QuestionTypeID { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("<script[\\d\\D]*?>[\\d\\D]*?</script>")]
        public string QuizQuestion { get; set; }
        [Required(ErrorMessage = "Required")]
        public int DifficultyLevelID { get; set; }
        [Required(ErrorMessage = "Required")]
        public int WeightageID { get; set; }
        [Required(ErrorMessage = "Required")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Required")]
        public bool IsObjective { get; set; }
        [Required(ErrorMessage = "Required")]
        public bool IsMandatory { get; set; }
        [Required(ErrorMessage = "Required")]
        public int SortOrder { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal PointsToEachAnswer { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal Duration { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Parse("1900/01/01");
        [Required(ErrorMessage = "Required")]
        public int AddUpdateQuestionID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string AddUpdateAnswerPoolID { get; set; }
        public string DeleteAnswerPoolID { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("<script[\\d\\D]*?>[\\d\\D]*?</script>")]
        public string QuestionAnswers { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("<script[\\d\\D]*?>[\\d\\D]*?</script>")]
        public string IsAnswerCorrectStatus { get; set; }
        [Required(ErrorMessage = "Required")]
        public int QuestionCategoryID { get; set; }
        public int RowTotal { get; set; }
        public int RowNum { get; set; }
        public string DifficultyLevel { get; set; }
        public string QuestionWeight { get; set; }
        public int NoOfAnswer { get; set; }
        public string StatusName { get; set; }
        public bool IsUpdatable { get; set; }

        #region For Quiz Answer
        public IEnumerable<QuizAnswerEntity> QuizAnswerList { get; set; }
        #endregion

    }
    #region Searching And Pagination
    public class QuizSearchQuestionEntity
    {
        public string SearchQuizQuestion { get; set; }
        public int SearchQuestionTypeID { get; set; }
        public int SearchCategoryID { get; set; }
        public int SearchDifficultyLevelID { get; set; }
        public int SearchWeightageID { get; set; }
        public int SearchStatus { get; set; } = -1;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int SearchQuestionType { get; set; } = -1;
    }
    #endregion

    #region Quiz Question Category
    public class QuizQuestionCategoryEntity
    {
        public string CategoryName { get; set; }
        public int CategoryTreeID { get; set; }
    }
    #endregion

    #region Quiz Question Type
    public class QuizQuestionTypeEntity
    {
        public string TypeDescription { get; set; }
        public int QuizQuestionTypeID { get; set; }
        public bool IsSingleTextBox { get; set; }
        public bool IsTrueFalse { get; set; }
    }
    #endregion

    #region Quiz Question Difficulty
    public class QuizQuestionDifficultyEntity
    {
        public string DifficultyLevel { get; set; }
        public int DifficultyLevelID { get; set; }
    }
    #endregion

    #region Quiz Question Difficulty
    public class QuizQuestionWeightageEntity
    {
        public string QuestionWeight { get; set; }
        public int QuestionWeigthageID { get; set; }
    }
    #endregion
}
