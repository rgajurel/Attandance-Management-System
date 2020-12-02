using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using Infrastructure;
using System.Text.RegularExpressions;

namespace InfrastructureData
{
    public class QuizRepository : IQuizRepository
    {
        public QuizEntity AddUpdateQuiz(QuizEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters AddQuizParam = new DynamicParameters();
                    AddQuizParam.Add("@QuizID", objInfo.QuizID);
                    AddQuizParam.Add("@CourseID", objInfo.CourseID);
                    AddQuizParam.Add("@QuizTitle", objInfo.QuizTitle);
                    AddQuizParam.Add("@EndDateSelected", objInfo.EndDate);
                    AddQuizParam.Add("@StartDateSelected", objInfo.StartDate);
                    AddQuizParam.Add("@QuizAppearingPoints", objInfo.QuizAppearingPoints);
                    AddQuizParam.Add("@SortOrder", objInfo.SortOrder);
                    AddQuizParam.Add("@StatusValue", objInfo.StatusValue);
                    AddQuizParam.Add("@CategoryID", objInfo.CategoryID);
                    AddQuizParam.Add("@StartPageDescription", objInfo.StartPageDescription);
                    AddQuizParam.Add("@CanShowCorrectAnswer", objInfo.CanShowCorrectAnswer);
                    AddQuizParam.Add("@IsPauseAllowed", objInfo.IsPauseAllowed);
                    AddQuizParam.Add("@NotificationIDs",objInfo.NotificationID.ToString());
                    AddQuizParam.Add("@CanSeePreviousAnswer", objInfo.CanSeePreviousAnswer);
                    AddQuizParam.Add("@EndPageDescription", objInfo.EndPageDescription);
                    AddQuizParam.Add("@IsQuestionManual", objInfo.IsQuestionManual);
                    AddQuizParam.Add("@QuizDescription", objInfo.QuizDescription);
                    AddQuizParam.Add("@QuizImage", objInfo.QuizImage);
                    AddQuizParam.Add("@QuizSlug", ToUrlSlug(objInfo.QuizTitle));
                    AddQuizParam.Add("@MetaTitle", objInfo.MetaTitle);
                    AddQuizParam.Add("@MetaDescription", objInfo.MetaDescription);
                    AddQuizParam.Add("@MetaKeyword", objInfo.MetaKeyword);
                    AddQuizParam.Add("@Tag", objInfo.Tag);
                    AddQuizParam.Add("@Priority", objInfo.Priority);
                    if (objInfo.IsQuestionManual)
                    {
                        AddQuizParam.Add("@CanShowAllQuestions", objInfo.CanShowAllQuestions);
                        AddQuizParam.Add("@TotalQuestion", objInfo.TotalQuestion);
                        AddQuizParam.Add("@SelectedAnswers", objInfo.SelectedAnswers);
                    }
                    else
                    {
                        AddQuizParam.Add("@CanShowAllQuestions", false);
                        AddQuizParam.Add("@TotalQuestion", objInfo.QuestionDynamicList.QuizQuestionMandatoryNo.Sum()+ objInfo.QuestionDynamicList.QuizQuestionOptionalNo.Sum());
                        AddQuizParam.Add("@DynamicQuizQuestionIDs", string.Join(",", objInfo.QuestionDynamicList.ID));
                        AddQuizParam.Add("@TotalMandatoryQuestionLst", string.Join(",", objInfo.QuestionDynamicList.QuizQuestionMandatoryNo));
                        AddQuizParam.Add("@TotalOptionalQuestionLst", string.Join(",", objInfo.QuestionDynamicList.QuizQuestionOptionalNo));
                        AddQuizParam.Add("@QuestionCategory", string.Join(",", objInfo.QuestionDynamicList.QuestionCategory));
                        AddQuizParam.Add("@QuestionDifficulty", string.Join(",", objInfo.QuestionDynamicList.QuestionDifficulty));
                    }
                    AddQuizParam.Add("@UserName", objInfo.AddedBy);
                    QuizEntity quizInfo = SqlMapper.Query<QuizEntity>(
                    connection, "[Quiz].[usp_QuizAddUpdate]", AddQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //int status = connection.Execute("[Quiz].[usp_QuizAddUpdate]", AddQuizParam, commandType: CommandType.StoredProcedure);
                    //return 1;
                    return quizInfo;
                }
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public int DeleteQuizByID(int QuizID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters DeleteParam = new DynamicParameters();
                    DeleteParam.Add("@QuizID", QuizID);
                    DeleteParam.Add("@Status", DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("[Quiz].[QuizDeleteByID]", DeleteParam, commandType: CommandType.StoredProcedure);
                    int OutID = DeleteParam.Get<int>("@Status");
                    return OutID;
                }
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }

        public IEnumerable<QuizCourse> GetAllCourseForQuiz()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<QuizCourse> CourseLst = SqlMapper.Query<QuizCourse>(
                    connection, "[Quiz].[usp_GetAllCourseForQuiz]", commandType: CommandType.StoredProcedure);
                    return CourseLst;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizNotification> GetAllNotification(int Identifier)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllNotificationParam = new DynamicParameters();
                    GetAllNotificationParam.Add("@Identifier", Identifier);
                    IEnumerable<QuizNotification> NotificationLst = SqlMapper.Query<QuizNotification>(
                    connection, "[Quiz].[usp_GetAllNotificationForQuiz]", GetAllNotificationParam, commandType: CommandType.StoredProcedure);
                    return NotificationLst;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IEnumerable<CategoryTree> GetAllQuizCategory(string CategoryType, int Identifier)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = DBManager.DbConnect())
        //        {
        //            DynamicParameters GetAllQuizCategoryParam = new DynamicParameters();
        //            GetAllQuizCategoryParam.Add("@CategoryType", CategoryType);
        //            GetAllQuizCategoryParam.Add("@Identifier", Identifier);
        //            IEnumerable<CategoryTree> QuizCategoryLst = SqlMapper.Query<CategoryTree>(
        //            connection, "[Quiz].[usp_GetAllQuizCategory]", GetAllQuizCategoryParam, commandType: CommandType.StoredProcedure);
        //            return QuizCategoryLst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public IEnumerable<QuizEntity> GetAllQuizListing(SearchQuizParam objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizCategoryParam = new DynamicParameters();
                    GetAllQuizCategoryParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizCategoryParam.Add("@PageIndex", objInfo.PageIndex);
                    GetAllQuizCategoryParam.Add("@SearchStartedFrom", objInfo.SearchStartedFrom==null?"": objInfo.SearchStartedFrom);
                    GetAllQuizCategoryParam.Add("@SearchStartedTo", objInfo.SearchStartedTo==null?"": objInfo.SearchStartedTo);
                    GetAllQuizCategoryParam.Add("@SearchEndFrom", objInfo.SearchEndFrom==null?"": objInfo.SearchEndFrom);
                    GetAllQuizCategoryParam.Add("@SearchEndTo", objInfo.SearchEndTo==null?"": objInfo.SearchEndTo);
                    GetAllQuizCategoryParam.Add("@StatusID", objInfo.SearchStatusID);
                    GetAllQuizCategoryParam.Add("@SearchQuizTitle", objInfo.SearchQuizTitle==null?"": objInfo.SearchQuizTitle);
                    IEnumerable<QuizEntity> QuizLst = SqlMapper.Query<QuizEntity>(
                    connection, "[Quiz].[usp_QuizGetAllQuiz]", GetAllQuizCategoryParam, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizEntity GetQuizByID(int QuizID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizByIDParam = new DynamicParameters();
                    GetAllQuizByIDParam.Add("@QuizID", QuizID);
                    QuizEntity Quizobj = SqlMapper.Query<QuizEntity>(
                    connection, "[Quiz].[usp_QuizGetByID]", GetAllQuizByIDParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return Quizobj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string ToUrlSlug(string value)
        {

            //First to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        //public IEnumerable<CategoryTree> GetAllQuizCategoryWithCount(string CategoryType, int Identifier)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = DBManager.DbConnect())
        //        {
        //            DynamicParameters GetAllQuizCategoryParam = new DynamicParameters();
        //            GetAllQuizCategoryParam.Add("@CategoryType", CategoryType);
        //            GetAllQuizCategoryParam.Add("@Identifier", Identifier);
        //            IEnumerable<CategoryTree> QuizCategoryLst = SqlMapper.Query<CategoryTree>(
        //            connection, "[Quiz].[usp_GetAllQuizCategoryWithCount]", GetAllQuizCategoryParam, commandType: CommandType.StoredProcedure);
        //            return QuizCategoryLst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool BatchUpdateQuiz(string JsonObject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@JsonObject", JsonObject);
                    connection.Execute("[Quiz].[usp_Quiz_BatchUpdateStatus]", Param, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetBatchUploadStatus(string JsonObject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@JsonObject", JsonObject);
                    string MsgObj = SqlMapper.Query<string>(
                    connection, "[Quiz].[usp_Quiz_GetStatusForBatchUpload]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return MsgObj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
