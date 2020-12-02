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
    public class QuizClientRepository : IQuizClientRepository
    {
        public IEnumerable<QuizAndSurveyPending> GetAllPendingQuizAndSurvey(string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@UserName", UserName);
                    IEnumerable<QuizAndSurveyPending> QuestionLst = SqlMapper.Query<QuizAndSurveyPending>(
                    connection, "[Quiz].[usp_QuizClient_GetAllPendingQuizAndSurvey]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    return QuestionLst;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<string> GetAllQuizQuestion(int QuizID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@QuizID", QuizID);
                    IEnumerable<string> QuestionLst = SqlMapper.Query<string>(
                    connection, "[Quiz].[usp_QuizClient_GetAllQuizQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    return QuestionLst;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetErrorMessage(int QuizID, int QuestionID, string Username)
        {
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters GetAllQuizParam = new DynamicParameters();
                GetAllQuizParam.Add("@QuizID", QuizID);
                GetAllQuizParam.Add("@QuestionID", QuestionID);
                GetAllQuizParam.Add("@UserName", Username);
                string JsonObject = SqlMapper.Query<string>(
                connection, "[Quiz].[usp_QuizClient_GetErrorMesssage]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return JsonObject;
            }
        }

        public QuizStartInfo GetNextQuestion(QuizStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    DynamicParameters ParamForAnswer = new DynamicParameters();
                    GetAllQuizParam.Add("@QuizID", objInfo.QuizID);
                    GetAllQuizParam.Add("@QuestionID", objInfo.QuestionID);
                    GetAllQuizParam.Add("@UserName", objInfo.UserName);
                    QuizStartInfo QuizLst = SqlMapper.Query<QuizStartInfo>(
                    connection, "[Quiz].[usp_QuizClient_GetNextQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (QuizLst != null)
                    {
                        ParamForAnswer.Add("@QuizID", objInfo.QuizID);
                        ParamForAnswer.Add("@UserName", objInfo.UserName);
                        ParamForAnswer.Add("@QuestionID", QuizLst.QuestionID);
                        IEnumerable<QuizAnswerInfo> AnswerList = SqlMapper.Query<QuizAnswerInfo>(
                        connection, "[Quiz].[usp_QuizClient_GetUserSelectedAnswer]", ParamForAnswer, commandType: CommandType.StoredProcedure).OrderBy(a => Guid.NewGuid());
                        QuizLst.QuizAnswerList = AnswerList.ToList();
                    }
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizStartInfo GetPreviousQuestion(QuizStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    DynamicParameters ParamForAnswer = new DynamicParameters();
                    GetAllQuizParam.Add("@QuizID", objInfo.QuizID);
                    GetAllQuizParam.Add("@QuestionID", objInfo.QuestionID);
                    GetAllQuizParam.Add("@UserName", objInfo.UserName);
                    QuizStartInfo QuizLst = SqlMapper.Query<QuizStartInfo>(
                    connection, "[Quiz].[usp_QuizClient_GetPreviousQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (QuizLst != null)
                    {
                        ParamForAnswer.Add("@QuizID", objInfo.QuizID);
                        ParamForAnswer.Add("@UserName", objInfo.UserName);
                        ParamForAnswer.Add("@QuestionID", QuizLst.QuestionID);
                        IEnumerable<QuizAnswerInfo> AnswerList = SqlMapper.Query<QuizAnswerInfo>(
                        connection, "[Quiz].[usp_QuizClient_GetUserSelectedAnswer]", ParamForAnswer, commandType: CommandType.StoredProcedure).OrderBy(a => Guid.NewGuid());
                        QuizLst.QuizAnswerList = AnswerList.ToList();
                    }
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizClientSide GetQuizDetailsFromSlug(string Slug, string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@QuizSlug", Slug);
                    GetAllQuizParam.Add("@UserName", UserName);
                    QuizClientSide QuizLst = SqlMapper.Query<QuizClientSide>(
                    connection, "[Quiz].[usp_QuizClient_GetQuizDeatilsForQuizSlug]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizClientSide> GetQuizListingForClient(QuizSearchingClientSide objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.PageIndex);
                    GetAllQuizParam.Add("@SearchQuizTitle", objInfo.SearchQuizTitle == null ? "" : objInfo.SearchQuizTitle);
                    GetAllQuizParam.Add("@SortBy", objInfo.SortBy);
                    GetAllQuizParam.Add("@UserName", objInfo.UserName);
                    IEnumerable<QuizClientSide> QuizLst = SqlMapper.Query<QuizClientSide>(
                    connection, "[Quiz].[usp_QuizClient_GetQuizListingForReportTable]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizClientSide> GetQuizListingForMyQuiz(QuizSearchingClientSide objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.PageIndex);
                    GetAllQuizParam.Add("@SearchQuizTitle", objInfo.SearchQuizTitle == null ? "" : objInfo.SearchQuizTitle);
                    GetAllQuizParam.Add("@UserName", objInfo.UserName);
                    IEnumerable<QuizClientSide> QuizLst = SqlMapper.Query<QuizClientSide>(
                    connection, "[Quiz].[usp_QuizClientMyQuiz_GetQuizListingForReportTable]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizClientSide GetQuizProgress(string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@UserName", UserName);
                    QuizClientSide QuizLst = SqlMapper.Query<QuizClientSide>(
                    connection, "[Quiz].[usp_QuizClient_GetQuizProgressForCurrentUser]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizStartInfo GetQuizQuestionAndAnswer(int QuizID, int QuestionID, string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    //using (IDbTransaction tran = connection.BeginTransaction())
                    //{
                    QuizStartInfo QuizLst = new QuizStartInfo();
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    DynamicParameters GetAllQuizParam1 = new DynamicParameters();
                    GetAllQuizParam.Add("@QuizID", QuizID);
                    GetAllQuizParam.Add("@QuestionID", QuestionID);
                    GetAllQuizParam.Add("@UserName", UserName);
                    QuizLst = SqlMapper.Query<QuizStartInfo>(
                    connection, "[Quiz].[usp_QuizClient_GetQuizQuestionByQuizID]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (QuizLst != null)
                    {
                        GetAllQuizParam1.Add("@QuestionID", QuizLst.QuestionID);
                        IEnumerable<QuizAnswerInfo> AnswerList = SqlMapper.Query<QuizAnswerInfo>(
                        connection, "[Quiz].[usp_QuizClientGetAnswerByQuestionID]", GetAllQuizParam1, commandType: CommandType.StoredProcedure).OrderBy(a => Guid.NewGuid());
                        QuizLst.QuizAnswerList = AnswerList.ToList();
                    }
                    return QuizLst;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizCompletionReport> GetQuizReport(int QuizID, string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@UserName", UserName);
                    Param.Add("@QuizID", QuizID);
                    IEnumerable<QuizCompletionReport> QuizLst = SqlMapper.Query<QuizCompletionReport>(
                    connection, "[Quiz].[usp_QuizClientGetQuizReport]", Param, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                    //}
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsAnswerCorrect(QuizStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    if (!objInfo.IsFreeWriting)
                    {
                        Param.Add("@AnswerID", string.Join(",", objInfo.AnswerID));
                    }
                    else
                    {
                        Param.Add("@FreeWritingSkip", objInfo.FreeWritingSkip);
                        Param.Add("@FreeWritingTimeOut", objInfo.FreeWritingTimeOut);
                    }
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@QuizID", int.Parse(objInfo.QuizID));
                    Param.Add("@CompletedOn", null);
                    Param.Add("@FreeWritingAnswer", objInfo.FreeWritingAnswer);
                    Param.Add("@IsFreeWriting", objInfo.IsFreeWriting);
                    Param.Add("@ElapsedTime", objInfo.TimeElapsed);
                    connection.Execute("[Quiz].[usp_QuizClient_CheckIsAnswerCorrect]", Param, commandType: CommandType.StoredProcedure);
                    return true;
                    //}
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPaused(QuizStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@QuizID", int.Parse(objInfo.QuizID));
                    Param.Add("@TimeElasped", objInfo.TimeElapsed);
                    Param.Add("@CurrentValue", DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("[Quiz].[usp_QuizClient_QuizPaused]", Param, commandType: CommandType.StoredProcedure);
                    int OutID = Param.Get<int>("@CurrentValue");
                    return OutID == 0 ? false : true;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetTimeElapsed(QuizStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@QuizID", int.Parse(objInfo.QuizID));
                    Param.Add("@ElaspedTime", objInfo.TimeElapsed);
                    connection.Execute("[Quiz].[usp_QuizClient_UpdateTimeElapsed]", Param, commandType: CommandType.StoredProcedure);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StartQuiz(QuizStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@QuizID", int.Parse(objInfo.QuizID));
                    connection.Execute("[Quiz].[usp_QuizClientFirstQuestionAsked]", Param, commandType: CommandType.StoredProcedure);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
