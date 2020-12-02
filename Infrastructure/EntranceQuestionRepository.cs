using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;

namespace Infrastructure
{
    public class EntranceQuestionRepository : IEntranceQuestionRepository
    {
        public bool AddUpdateEntranceQuestion(EntranceQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters AddQuizParam = new DynamicParameters();
                    AddQuizParam.Add("@AddUpdateQuestionID", objInfo.AddUpdateQuestionID);
                    AddQuizParam.Add("@AddUpdateAnswerPoolID", objInfo.AddUpdateAnswerPoolID);
                    AddQuizParam.Add("@DeleteAnswerPoolID", objInfo.DeleteAnswerPoolID);
                    AddQuizParam.Add("@QuestionTypeID", objInfo.QuestionTypeID);
                    AddQuizParam.Add("@EntranceQuestion", objInfo.EntranceQuestion);
                    AddQuizParam.Add("@DifficultyLevelID", objInfo.DifficultyLevelID);
                    AddQuizParam.Add("@WeightageID", objInfo.WeightageID);
                    AddQuizParam.Add("@IsActive", objInfo.IsActive);
                    AddQuizParam.Add("@IsObjective", objInfo.IsObjective);
                    AddQuizParam.Add("@IsMandatory", objInfo.IsMandatory);
                    AddQuizParam.Add("@SortOrder", objInfo.SortOrder);
                    AddQuizParam.Add("@PointsToEachAnswer", objInfo.PointsToEachAnswer);
                    AddQuizParam.Add("@Duration", objInfo.Duration);
                    AddQuizParam.Add("@AddedBy", "s");
                    AddQuizParam.Add("@UpdatedBy", "s");
                    AddQuizParam.Add("@QuestionAnswers", objInfo.QuestionAnswers);
                    AddQuizParam.Add("@IsAnswerCorrectStatus", objInfo.IsAnswerCorrectStatus);
                    AddQuizParam.Add("@QuestionCategoryID", objInfo.QuestionCategoryID);
                    int status = connection.Execute("[Entrance].[usp_AddUpdateEntranceQuestion]", AddQuizParam, commandType: CommandType.StoredProcedure);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool BatchUpdateEntranceQuestionStatus(string JsonObject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@JsonObject", JsonObject);
                    connection.Execute("[Entrance].[usp_EntranceQuestion_BatchUpdateStatus]", Param, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEntranceQuestion(EntranceQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters DeleteQuizParam = new DynamicParameters();
                    DeleteQuizParam.Add("@QuestionID", objInfo.QuestionID);
                    DeleteQuizParam.Add("@OperationStatus", DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("[Entrance].[usp_DeletedEntranceQuestion]", DeleteQuizParam, commandType: CommandType.StoredProcedure);
                    int OutID = DeleteQuizParam.Get<int>("@OperationStatus");
                    return OutID;
                }
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }

        public IEnumerable<EntranceQuestionEntity> GetAllEntranceQuestion(EntranceSearchQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.Page);
                    GetAllQuizParam.Add("@SearchEntranceQuestion", objInfo.SearchEntranceQuestion);
                    GetAllQuizParam.Add("@SearchStatus", objInfo.SearchStatus);
                    GetAllQuizParam.Add("@SearchCategoryID", objInfo.SearchCategoryID);
                    GetAllQuizParam.Add("@SearchDifficultyID", objInfo.SearchDifficultyLevelID);
                    GetAllQuizParam.Add("@SearchWeightID", objInfo.SearchWeightageID);
                    GetAllQuizParam.Add("@SearchQuestionTypeID", objInfo.SearchQuestionTypeID);
                    GetAllQuizParam.Add("@SearchQuestionType", objInfo.SearchQuestionType);
                    connection.Open();
                    IEnumerable<EntranceQuestionEntity> EntranceQuestionLst = SqlMapper.Query<EntranceQuestionEntity>(
                    connection, "[Entrance].[usp_GetAllEntranceQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return EntranceQuestionLst;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EntranceQuestionDifficultyEntity> GetAllEntranceQuestionDifficulty()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<EntranceQuestionDifficultyEntity> QuizDifficultyLevel = SqlMapper.Query<EntranceQuestionDifficultyEntity>(
                    connection, "[Entrance].[usp_GetAllEntranceQuestionDifficultyLevel]", commandType: CommandType.StoredProcedure);
                    return QuizDifficultyLevel;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntranceQuestionEntity> GetAllEntranceQuestionForEntrance(EntranceSearchQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.Page);
                    GetAllQuizParam.Add("@SearchEntranceQuestion", objInfo.SearchEntranceQuestion);
                    GetAllQuizParam.Add("@SearchStatus", objInfo.SearchStatus);
                    GetAllQuizParam.Add("@SearchCategoryID", objInfo.SearchCategoryID);
                    GetAllQuizParam.Add("@SearchDifficultyID", objInfo.SearchDifficultyLevelID);
                    GetAllQuizParam.Add("@SearchWeightID", objInfo.SearchWeightageID);
                    GetAllQuizParam.Add("@SearchQuestionTypeID", objInfo.SearchQuestionTypeID);
                    GetAllQuizParam.Add("@SearchQuestionType", objInfo.SearchQuestionType);
                    connection.Open();
                    IEnumerable<EntranceQuestionEntity> QuizQuestionLst = SqlMapper.Query<EntranceQuestionEntity>(
                    connection, "[Entrance].[usp_GetAllEntranceQuestionForEntrance]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EntranceQuestionTypeEntity> GetAllEntranceQuestionType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<EntranceQuestionTypeEntity> QuizQuestionType = SqlMapper.Query<EntranceQuestionTypeEntity>(
                    connection, "[Entrance].[usp_GetAllEntranceQuestionType]", commandType: CommandType.StoredProcedure);
                    return QuizQuestionType;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntranceQuestionWeightageEntity> GetAllEntranceQuestionWeight()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<EntranceQuestionWeightageEntity> QuizQuestionType = SqlMapper.Query<EntranceQuestionWeightageEntity>(
                    connection, "[Entrance].[usp_GetAllEntranceWeightageLevel]", commandType: CommandType.StoredProcedure);
                    return QuizQuestionType;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EntranceQuestionEntity GetEntranceQuestionByID(int QuestionID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetQuizByIDParam = new DynamicParameters();
                    GetQuizByIDParam.Add("@QuestionID", QuestionID);
                    EntranceQuestionEntity QuizQuestion = SqlMapper.Query<EntranceQuestionEntity>(
                    connection, "[Entrance].[usp_GetEntranceQuestionByID]", GetQuizByIDParam, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    IEnumerable<EntranceAnswerEntity> QuizAnswerLst = SqlMapper.Query<EntranceAnswerEntity>(
                    connection, "[Entrance].[usp_GetEntranceAnswerByID]", GetQuizByIDParam, commandType: CommandType.StoredProcedure);
                    QuizQuestion.EntranceAnswerList = QuizAnswerLst;
                    return QuizQuestion;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetStatusForBatchUpdateQuestionUpdate(string JsonObject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@JsonObject", JsonObject);
                    string MsgObj = SqlMapper.Query<string>(
                                       connection, "[Entrance].[GetStatusForBatchQuestionUpdate]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return MsgObj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<CategoryTree> GetAllEntraceQuestionCategory(string CategoryType)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizCategoryParam = new DynamicParameters();
                    GetAllQuizCategoryParam.Add("@CategoryType", CategoryType);
                    IEnumerable<CategoryTree> QuizCategoryLst = SqlMapper.Query<CategoryTree>(
                    connection, "[Quiz].[usp_GetAllCategory]", GetAllQuizCategoryParam, commandType: CommandType.StoredProcedure);
                    return QuizCategoryLst;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
