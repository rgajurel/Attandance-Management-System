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
    public class QuizUserReportRepository : IQuizUserReportRepository
    {
        public bool AssignMarktoUser(int UserScore, int ID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@UserScore", UserScore);
                    Param.Add("@UserQuizQuestionAnswerID", ID);
                    connection.Open();
                    connection.Execute("[Quiz].[usp_QuizUserReport_AssignMarkToUser]", Param, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public IEnumerable<QuizUserReport> GetAllQuizUserListing(SearchParamQuizUserReport objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@SearchQuizName", objInfo.SearchQuizName);
                    Param.Add("@SearchUserGroup", objInfo.SearchUserGroup);
                    Param.Add("@SearchUserID", objInfo.SearchUserID);
                    Param.Add("@SearchCompletionTime", objInfo.SearchCompletionTime);
                    Param.Add("@SearchJoinedFrom", objInfo.SearchJoinedFrom);
                    Param.Add("@SearchJoinedTo", objInfo.SearchJoinedTo);
                    Param.Add("@SearchCompletedFrom", objInfo.SearchCompletedFrom);
                    Param.Add("@SearchCompletedTo", objInfo.SearchCompletedTo);
                    Param.Add("@SearchQuizStatus", objInfo.SearchQuizStatus);
                    Param.Add("@PageIndex", objInfo.PageIndex);
                    Param.Add("@PageSize", objInfo.PageSize);
                    connection.Open();
                    IEnumerable<QuizUserReport> QuizQuestionLst = SqlMapper.Query<QuizUserReport>(
                    connection, "[Quiz].[usp_QuizUserReport_GetQuizUserListing]", Param, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<QuizAllUser> GetAllUserForQuiz()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    IEnumerable<QuizAllUser> QuizQuestionLst = SqlMapper.Query<QuizAllUser>(
                    connection, "[Quiz].[usp_QuizUserReport_GetAllUser]", commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuizQuestionUserReport UserQuizAnswerByUserID(int QuizUserID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@QuizUserID", QuizUserID);
                    connection.Open();
                    QuizQuestionUserReport QuizQuestionLst = SqlMapper.Query<QuizQuestionUserReport>(
                    connection, "[Quiz].[usp_QuizUserReport_GetQuizQuestionByQuizUserID]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
