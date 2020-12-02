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

namespace InfrastructureData
{
    public class QuizQuestionRepository : IQuizQuestionRepository
    {
        #region Quiz Admin Side
        public bool AddUpdateQuizQuestion(QuizQuestionEntity objInfo)
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
                    AddQuizParam.Add("@QuizQuestion", objInfo.QuizQuestion);
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
                    int status = connection.Execute("[Quiz].[usp_AddUpdateQuizQuestion]", AddQuizParam, commandType: CommandType.StoredProcedure);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool BatchUpdateQuizQuestionStatus(string JsonObject)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@JsonObject", JsonObject);
                    connection.Execute("[Quiz].[usp_QuizQuestion_BatchUpdateStatus]", Param, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteQuizQuestion(QuizQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters DeleteQuizParam = new DynamicParameters();
                    DeleteQuizParam.Add("@QuestionID", objInfo.QuestionID);
                    DeleteQuizParam.Add("@OperationStatus", DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("[Quiz].[usp_DeletedQuizQuestion]", DeleteQuizParam, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<QuizQuestionEntity> GetAllQuizQuestion(QuizSearchQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize );
                    GetAllQuizParam.Add("@PageIndex", objInfo.Page);
                    GetAllQuizParam.Add("@SearchQuizQuestion", objInfo.SearchQuizQuestion);
                    GetAllQuizParam.Add("@SearchStatus", objInfo.SearchStatus);
                    GetAllQuizParam.Add("@SearchCategoryID", objInfo.SearchCategoryID);
                    GetAllQuizParam.Add("@SearchDifficultyID", objInfo.SearchDifficultyLevelID);
                    GetAllQuizParam.Add("@SearchWeightID", objInfo.SearchWeightageID);
                    GetAllQuizParam.Add("@SearchQuestionTypeID", objInfo.SearchQuestionTypeID);
                    GetAllQuizParam.Add("@SearchQuestionType", objInfo.SearchQuestionType);
                    connection.Open();
                    IEnumerable<QuizQuestionEntity> QuizQuestionLst = SqlMapper.Query<QuizQuestionEntity>(
                    connection, "[Quiz].[usp_GetAllQuizQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<QuizQuestionDifficultyEntity> GetAllQuizQuestionDifficulty()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<QuizQuestionDifficultyEntity> QuizDifficultyLevel = SqlMapper.Query<QuizQuestionDifficultyEntity>(
                    connection, "[Quiz].[usp_GetAllQuizQuestionDifficultyLevel]", commandType: CommandType.StoredProcedure);
                    return QuizDifficultyLevel;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizQuestionEntity> GetAllQuizQuestionForQuiz(QuizSearchQuestionEntity objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.Page);
                    GetAllQuizParam.Add("@SearchQuizQuestion", objInfo.SearchQuizQuestion);
                    GetAllQuizParam.Add("@SearchStatus", objInfo.SearchStatus);
                    GetAllQuizParam.Add("@SearchCategoryID", objInfo.SearchCategoryID);
                    GetAllQuizParam.Add("@SearchDifficultyID", objInfo.SearchDifficultyLevelID);
                    GetAllQuizParam.Add("@SearchWeightID", objInfo.SearchWeightageID);
                    GetAllQuizParam.Add("@SearchQuestionTypeID", objInfo.SearchQuestionTypeID);
                    GetAllQuizParam.Add("@SearchQuestionType", objInfo.SearchQuestionType);
                    connection.Open();
                    IEnumerable<QuizQuestionEntity> QuizQuestionLst = SqlMapper.Query<QuizQuestionEntity>(
                    connection, "[Quiz].[usp_GetAllQuizQuestionForQuiz]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<QuizQuestionTypeEntity> GetAllQuizQuestionType()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<QuizQuestionTypeEntity> QuizQuestionType = SqlMapper.Query<QuizQuestionTypeEntity>(
                    connection, "[Quiz].[usp_GetAllQuizQuestionType]", commandType: CommandType.StoredProcedure);
                    return QuizQuestionType;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizQuestionWeightageEntity> GetAllQuizQuestionWeight()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    IEnumerable<QuizQuestionWeightageEntity> QuizQuestionType = SqlMapper.Query<QuizQuestionWeightageEntity>(
                    connection, "[Quiz].[usp_GetAllQuizWeightageLevel]", commandType: CommandType.StoredProcedure);
                    return QuizQuestionType;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizQuestionEntity GetQuizQuestionByID(int QuestionID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetQuizByIDParam = new DynamicParameters();
                    GetQuizByIDParam.Add("@QuestionID", QuestionID);
                    QuizQuestionEntity QuizQuestion = SqlMapper.Query<QuizQuestionEntity>(
                    connection, "[Quiz].[usp_GetQuizQuestionByID]", GetQuizByIDParam, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    IEnumerable<QuizAnswerEntity> QuizAnswerLst = SqlMapper.Query<QuizAnswerEntity>(
                    connection, "[Quiz].[usp_GetQuizAnswerByID]", GetQuizByIDParam, commandType: CommandType.StoredProcedure);
                    QuizQuestion.QuizAnswerList = QuizAnswerLst;
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
                                       connection, "[Quiz].[GetStatusForBatchQuestionUpdate]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return MsgObj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
