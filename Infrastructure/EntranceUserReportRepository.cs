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
    public class EntranceUserReportRepository : IEntranceUserReportRepository
    {
        public bool AssignMarktoUser(int UserScore, int ID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@UserScore", UserScore);
                    Param.Add("@UserEntranceQuestionAnswerID", ID);
                    connection.Open();
                    connection.Execute("[Entrance].[usp_EntranceUserReport_AssignMarkToUser]", Param, commandType: CommandType.StoredProcedure);
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

        public IEnumerable<EntranceUserReport> GetAllEntranceUserListing(SearchParamEntranceUserReport objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@SearchEntranceName", objInfo.SearchEntranceName);
                    Param.Add("@SearchUserGroup", objInfo.SearchUserGroup);
                    Param.Add("@SearchUserID", objInfo.SearchUserID);
                    Param.Add("@SearchCompletionTime", objInfo.SearchCompletionTime);
                    Param.Add("@SearchJoinedFrom", objInfo.SearchJoinedFrom);
                    Param.Add("@SearchJoinedTo", objInfo.SearchJoinedTo);
                    Param.Add("@SearchCompletedFrom", objInfo.SearchCompletedFrom);
                    Param.Add("@SearchCompletedTo", objInfo.SearchCompletedTo);
                    Param.Add("@SearchEntranceStatus", objInfo.SearchEntranceStatus);
                    Param.Add("@PageIndex", objInfo.PageIndex);
                    Param.Add("@PageSize", objInfo.PageSize);
                    connection.Open();
                    IEnumerable<EntranceUserReport> QuizQuestionLst = SqlMapper.Query<EntranceUserReport>(
                    connection, "[Entrance].[usp_EntranceUserReport_GetEntranceUserListing]", Param, commandType: CommandType.StoredProcedure);
                    connection.Close();
                    return QuizQuestionLst;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EntranceAllUser> GetAllUserForEntrance()
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    IEnumerable<EntranceAllUser> QuizQuestionLst = SqlMapper.Query<EntranceAllUser>(
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

        public EntranceQuestionUserReport UserEntranceAnswerByUserID(int EntranceUserID)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@EntranceUserID", EntranceUserID);
                    connection.Open();
                    EntranceQuestionUserReport QuizQuestionLst = SqlMapper.Query<EntranceQuestionUserReport>(
                    connection, "[Entrance].[usp_EntranceUserReport_GetEntranceQuestionByEntranceUserID]", Param, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
