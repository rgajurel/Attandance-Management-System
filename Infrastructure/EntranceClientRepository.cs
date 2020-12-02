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
    public class EntranceClientRepository : IEntranceClientRepository
    {
        public IEnumerable<string> GetAllEntranceQuestion(int EntranceID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@EntranceID", EntranceID);
                    IEnumerable<string> QuestionLst = SqlMapper.Query<string>(
                    connection, "[Entrance].[usp_EntranceClient_GetAllEntranceQuestion]", Param, commandType: CommandType.StoredProcedure);
                    return QuestionLst;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntranceAndSurveyPending> GetAllPendingEntranceAndSurvey(string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@UserName", UserName);
                    IEnumerable<EntranceAndSurveyPending> QuestionLst = SqlMapper.Query<EntranceAndSurveyPending>(
                    connection, "[Entrance].[usp_EntranceClient_GetAllPendingEntranceAndSurvey]", Param, commandType: CommandType.StoredProcedure);
                    return QuestionLst;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EntranceClientSide GetEntranceDetailsFromSlug(string Slug, string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@EntranceSlug", Slug);
                    Param.Add("@UserName", UserName);
                    EntranceClientSide QuizLst = SqlMapper.Query<EntranceClientSide>(
                    connection, "[Entrance].[usp_EntranceClient_GetEntranceDeatilsForEntranceSlug]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntranceClientSide> GetEntranceListingForClient(EntranceSearchingClientSide objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@PageSize", objInfo.PageSize);
                    Param.Add("@PageIndex", objInfo.PageIndex);
                    Param.Add("@SearchEntranceTitle", objInfo.SearchEntranceTitle == null ? "" : objInfo.SearchEntranceTitle);
                    Param.Add("@SortBy", objInfo.SortBy);
                    Param.Add("@UserName", objInfo.UserName);
                    IEnumerable<EntranceClientSide> QuizLst = SqlMapper.Query<EntranceClientSide>(
                    connection, "[Entrance].[usp_EntranceClient_GetEntranceListingForReportTable]", Param, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntranceClientSide> GetEntranceListingForMyEntrance(EntranceSearchingClientSide objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@PageSize", objInfo.PageSize);
                    Param.Add("@PageIndex", objInfo.PageIndex);
                    Param.Add("@SearchEntranceTitle", objInfo.SearchEntranceTitle == null ? "" : objInfo.SearchEntranceTitle);
                    Param.Add("@UserName", objInfo.UserName);
                    IEnumerable<EntranceClientSide> QuizLst = SqlMapper.Query<EntranceClientSide>(
                    connection, "[Entrance].[usp_EntranceClientMyEntrance_GetEntranceListingForReportTable]", Param, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EntranceClientSide GetEntranceProgress(string UserName)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@UserName", UserName);
                    EntranceClientSide QuizLst = SqlMapper.Query<EntranceClientSide>(
                    connection, "[Entrance].[usp_EntranceClient_GetEntranceProgressForCurrentUser]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EntranceStartInfo GetEntranceQuestionAndAnswer(int EntranceID, int QuestionID, string Username,string Identifier)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    //using (IDbTransaction tran = connection.BeginTransaction())
                    //{
                    EntranceStartInfo QuizLst = new EntranceStartInfo();
                    DynamicParameters GetAllEntranceParam = new DynamicParameters();
                    DynamicParameters GetAllEntranceParam1 = new DynamicParameters();
                    GetAllEntranceParam.Add("@EntranceID", EntranceID);
                    GetAllEntranceParam.Add("@QuestionID", QuestionID);
                    GetAllEntranceParam.Add("@UserName", Username);
                    GetAllEntranceParam.Add("@Identifier", Identifier);
                    QuizLst = SqlMapper.Query<EntranceStartInfo>(
                    connection, "[Entrance].[usp_EntranceClient_GetEntranceQuestionByEntranceID]", GetAllEntranceParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (QuizLst != null)
                    {
                        GetAllEntranceParam1.Add("@QuestionID", QuizLst.QuestionID);
                        IEnumerable<EntranceAnswerInfo> AnswerList = SqlMapper.Query<EntranceAnswerInfo>(
                        connection, "[Entrance].[usp_EntranceClientGetAnswerByQuestionID]", GetAllEntranceParam1, commandType: CommandType.StoredProcedure).OrderBy(a => Guid.NewGuid());
                        QuizLst.EntranceAnswerList = AnswerList.ToList();
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

        public IEnumerable<EntranceCompletionReport> GetEntranceReport(int EntranceID, string UserName,string Identifier)
        {
            try{
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@UserName", UserName);
                    Param.Add("@EntranceID", EntranceID);
                    Param.Add("@Identifier", Identifier);
                    IEnumerable<EntranceCompletionReport> QuizLst = SqlMapper.Query<EntranceCompletionReport>(
                    connection, "[Entrance].[usp_EntranceClientGetEntranceReport]", Param, commandType: CommandType.StoredProcedure);
                    return QuizLst;
                   //}
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetErrorMessage(int EntranceID, int QuestionID, string Username, string Identifier)
        {
            using (IDbConnection connection = DBManager.DbConnect())
            {
                DynamicParameters GetAllQuizParam = new DynamicParameters();
                GetAllQuizParam.Add("@EntranceID", EntranceID);
                GetAllQuizParam.Add("@QuestionID", QuestionID);
                GetAllQuizParam.Add("@UserName", Username);
                GetAllQuizParam.Add("@Identifier", Identifier);
                string JsonObject = SqlMapper.Query<string>(
                connection, "[Entrance].[usp_EntranceClient_GetErrorMesssage]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return JsonObject;
            }
        }

        public EntranceStartInfo GetNextQuestion(EntranceStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    DynamicParameters ParamForAnswer = new DynamicParameters();
                    GetAllQuizParam.Add("@EntranceID", objInfo.EntranceID);
                    GetAllQuizParam.Add("@QuestionID", objInfo.QuestionID);
                    GetAllQuizParam.Add("@UserName", objInfo.UserName);
                    GetAllQuizParam.Add("@Identifier", objInfo.Identifier);
                    EntranceStartInfo QuizLst = SqlMapper.Query<EntranceStartInfo>(
                    connection, "[Entrance].[usp_EntranceClient_GetNextQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (QuizLst != null)
                    {
                        ParamForAnswer.Add("@EntranceID", objInfo.EntranceID);
                        ParamForAnswer.Add("@UserName", objInfo.UserName);
                        ParamForAnswer.Add("@QuestionID", QuizLst.QuestionID);
                        IEnumerable<EntranceAnswerInfo> AnswerList = SqlMapper.Query<EntranceAnswerInfo>(
                        connection, "[Entrance].[usp_EntranceClient_GetUserSelectedAnswer]", ParamForAnswer, commandType: CommandType.StoredProcedure).OrderBy(a => Guid.NewGuid());
                        QuizLst.EntranceAnswerList = AnswerList.ToList();
                    }
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EntranceStartInfo GetPreviousQuestion(EntranceStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    DynamicParameters ParamForAnswer = new DynamicParameters();
                    GetAllQuizParam.Add("@EntranceID", objInfo.EntranceID);
                    GetAllQuizParam.Add("@QuestionID", objInfo.QuestionID);
                    GetAllQuizParam.Add("@UserName", objInfo.UserName);
                    GetAllQuizParam.Add("@Identifier", objInfo.Identifier);
                    EntranceStartInfo QuizLst = SqlMapper.Query<EntranceStartInfo>(
                    connection, "[Entrance].[usp_EntranceClient_GetPreviousQuestion]", GetAllQuizParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (QuizLst != null)
                    {
                        ParamForAnswer.Add("@EntranceID", objInfo.EntranceID);
                        ParamForAnswer.Add("@UserName", objInfo.UserName);
                        ParamForAnswer.Add("@QuestionID", QuizLst.QuestionID);
                        IEnumerable<EntranceAnswerInfo> AnswerList = SqlMapper.Query<EntranceAnswerInfo>(
                        connection, "[Entrance].[usp_EntranceClient_GetUserSelectedAnswer]", ParamForAnswer, commandType: CommandType.StoredProcedure).OrderBy(a => Guid.NewGuid());
                        QuizLst.EntranceAnswerList = AnswerList.ToList();
                    }
                    return QuizLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsAnswerCorrect(EntranceStartInfo objInfo)
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
                    Param.Add("@EntranceID", int.Parse(objInfo.EntranceID));
                    Param.Add("@CompletedOn", null);
                    Param.Add("@FreeWritingAnswer", objInfo.FreeWritingAnswer);
                    Param.Add("@IsFreeWriting", objInfo.IsFreeWriting);
                    Param.Add("@ElapsedTime", objInfo.TimeElapsed);
                    Param.Add("@Identifier", objInfo.Identifier);
                    Param.Add("@Examinee", objInfo.Examinee);
                    connection.Execute("[Entrance].[usp_EntranceClient_CheckIsAnswerCorrect]", Param, commandType: CommandType.StoredProcedure);
                    return true;
                    //}
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPaused(EntranceStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@EntranceID", int.Parse(objInfo.EntranceID));
                    Param.Add("@TimeElasped", objInfo.TimeElapsed);
                    Param.Add("@Identifier", objInfo.Identifier);
                    Param.Add("@CurrentValue", DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("[Entrance].[usp_EntranceClient_EntrancePaused]", Param, commandType: CommandType.StoredProcedure);
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

        public void SetTimeElapsed(EntranceStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@EntranceID", int.Parse(objInfo.EntranceID));
                    Param.Add("@ElaspedTime", objInfo.TimeElapsed);
                    Param.Add("@Identifier", objInfo.Identifier);
                    connection.Execute("[Entrance].[usp_EntranceClient_UpdateTimeElapsed]", Param, commandType: CommandType.StoredProcedure);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StartEntrance(EntranceStartInfo objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuestionID", int.Parse(objInfo.QuestionID));
                    Param.Add("@UserName", objInfo.UserName);
                    Param.Add("@EntranceID", int.Parse(objInfo.EntranceID));
                    Param.Add("@Identifier", objInfo.Identifier);
                    connection.Execute("[Entrance].[usp_EntranceClientFirstQuestionAsked]", Param, commandType: CommandType.StoredProcedure);
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
