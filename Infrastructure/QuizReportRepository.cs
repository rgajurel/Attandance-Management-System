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
    public class QuizReportRepository : IQuizReport
    {
        public IEnumerable<QuizReport> GetAllQuizListing(QuizReportSearch objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    connection.Open();
                    DynamicParameters Param = new DynamicParameters();
                    Param.Add("@SearchQuizTitle", objInfo.SearchQuizTitle);
                    Param.Add("@SearchStatusID", objInfo.SearchStatusID);
                    Param.Add("@SearchStartedFrom", objInfo.SearchStartedFrom);
                    Param.Add("@SearchStartedTo", objInfo.SearchStartedTo);
                    Param.Add("@SearchEndFrom", objInfo.SearchEndFrom);
                    Param.Add("@SearchEndTo", objInfo.SearchEndTo);
                    Param.Add("@UserGroupID", objInfo.UserGroupID==null?"": objInfo.UserGroupID);
                    Param.Add("@QuizCategory", objInfo.QuizCategory==0?-1: objInfo.QuizCategory);
                    Param.Add("@PageIndex", objInfo.PageIndex);
                    Param.Add("@PageSize", objInfo.PageSize);
                    IEnumerable<QuizReport> QuizLst = SqlMapper.Query<QuizReport>(
                   connection, "[Quiz].[usp_QuizReport_GetAllQuizListing]", Param, commandType: CommandType.StoredProcedure);
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
                    connection, "[Quiz].[usp_QuizReport_QuizGetByID]", GetAllQuizByIDParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return Quizobj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
