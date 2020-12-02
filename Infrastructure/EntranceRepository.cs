using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using System.Text.RegularExpressions;

namespace Infrastructure
{
    public class EntranceRepository : IEntranceRepository
    {
        public EntranceEntity AddUpdateEntrance(EntranceEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@EntranceID", objInfo.EntranceID);
                    Param.Add("@CourseID", objInfo.CourseID);
                    Param.Add("@EntranceTitle", objInfo.EntranceTitle);
                    Param.Add("@EndDateSelected", objInfo.EndDate);
                    Param.Add("@StartDateSelected", objInfo.StartDate);
                    Param.Add("@EntranceAppearingPoints", objInfo.EntranceAppearingPoints);
                    Param.Add("@SortOrder", objInfo.SortOrder);
                    Param.Add("@StatusValue", objInfo.StatusValue);
                    Param.Add("@CategoryID", objInfo.CategoryID);
                    Param.Add("@StartPageDescription", objInfo.StartPageDescription);
                    Param.Add("@CanShowCorrectAnswer", objInfo.CanShowCorrectAnswer);
                    Param.Add("@IsPauseAllowed", objInfo.IsPauseAllowed);
                    Param.Add("@NotificationIDs", objInfo.NotificationID.ToString());
                    Param.Add("@CanSeePreviousAnswer", objInfo.CanSeePreviousAnswer);
                    Param.Add("@EndPageDescription", objInfo.EndPageDescription);
                    Param.Add("@IsQuestionManual", objInfo.IsQuestionManual);
                    Param.Add("@EntranceDescription", objInfo.EntranceDescription);
                    Param.Add("@EntranceImage", objInfo.EntranceImage);
                    Param.Add("@EntranceSlug", ToUrlSlug(objInfo.EntranceTitle));
                    Param.Add("@MetaTitle", objInfo.MetaTitle);
                    Param.Add("@MetaDescription", objInfo.MetaDescription);
                    Param.Add("@MetaKeyword", objInfo.MetaKeyword);
                    Param.Add("@Tag", objInfo.Tag);
                    Param.Add("@Priority", objInfo.Priority);
                    if (objInfo.IsQuestionManual)
                    {
                        Param.Add("@CanShowAllQuestions", objInfo.CanShowAllQuestions);
                        Param.Add("@TotalQuestion", objInfo.TotalQuestion);
                        Param.Add("@SelectedAnswers", objInfo.SelectedAnswers);
                    }
                    //else
                    //{
                    //    Param.Add("@CanShowAllQuestions", false);
                    //    Param.Add("@TotalQuestion", objInfo.QuestionDynamicList.QuizQuestionMandatoryNo.Sum() + objInfo.QuestionDynamicList.QuizQuestionOptionalNo.Sum());
                    //    Param.Add("@DynamicQuizQuestionIDs", string.Join(",", objInfo.QuestionDynamicList.ID));
                    //    Param.Add("@TotalMandatoryQuestionLst", string.Join(",", objInfo.QuestionDynamicList.QuizQuestionMandatoryNo));
                    //    Param.Add("@TotalOptionalQuestionLst", string.Join(",", objInfo.QuestionDynamicList.QuizQuestionOptionalNo));
                    //    Param.Add("@QuestionCategory", string.Join(",", objInfo.QuestionDynamicList.QuestionCategory));
                    //    Param.Add("@QuestionDifficulty", string.Join(",", objInfo.QuestionDynamicList.QuestionDifficulty));
                    //}
                    Param.Add("@UserName", objInfo.AddedBy);
                    EntranceEntity EntranceInfo = SqlMapper.Query<EntranceEntity>(
                    connection, "[Entrance].[usp_EntranceAddUpdate]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //int status = connection.Execute("[Quiz].[usp_QuizAddUpdate]", Param, commandType: CommandType.StoredProcedure);
                    //return 1;
                    return EntranceInfo;
                }
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public bool BatchUpdateEntrance(string JsonObject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@JsonObject", JsonObject);
                    connection.Execute("[Entrance].[usp_Entrance_BatchUpdateStatus]", Param, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEntranceByID(int EntranceID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters DeleteParam = new DynamicParameters();
                    DeleteParam.Add("@EntranceID", EntranceID);
                    DeleteParam.Add("@Status", DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("[Entrance].[EntranceDeleteByID]", DeleteParam, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<EntranceEntity> GetAllEntranceListing(SearchEntranceParam objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@PageSize", objInfo.PageSize);
                    Param.Add("@PageIndex", objInfo.PageIndex);
                    Param.Add("@SearchStartedFrom", objInfo.SearchStartedFrom == null ? "" : objInfo.SearchStartedFrom);
                    Param.Add("@SearchStartedTo", objInfo.SearchStartedTo == null ? "" : objInfo.SearchStartedTo);
                    Param.Add("@SearchEndFrom", objInfo.SearchEndFrom == null ? "" : objInfo.SearchEndFrom);
                    Param.Add("@SearchEndTo", objInfo.SearchEndTo == null ? "" : objInfo.SearchEndTo);
                    Param.Add("@StatusID", objInfo.SearchStatusID);
                    Param.Add("@SearchEntranceTitle", objInfo.SearchEntranceTitle == null ? "" : objInfo.SearchEntranceTitle);
                    IEnumerable<EntranceEntity> QuizLst = SqlMapper.Query<EntranceEntity>(
                    connection, "[Entrance].[usp_EntranceGetAllEntrance]", Param, commandType: CommandType.StoredProcedure);
                    return QuizLst;
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
                    connection, "[Entrance].[usp_Entrance_GetStatusForBatchUpload]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return MsgObj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EntranceEntity GetEntranceByID(int EntranceID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizByIDParam = new DynamicParameters();
                    GetAllQuizByIDParam.Add("@EntranceID", EntranceID);
                    EntranceEntity EntranceObj = SqlMapper.Query<EntranceEntity>(
                    connection, "[Entrance].[usp_EntranceGetByID]", GetAllQuizByIDParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return EntranceObj;
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
    }
}
