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
    public class QuizQuestionReportRepository : IQuizQuestionReportRepository
    {
        public IEnumerable<QuizQuestionReport> GetAllQuestionListing(SearchParamQuizQuestionreport objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.PageIndex);
                    GetAllQuizParam.Add("@SearchQuestion", objInfo.Question==null?"": objInfo.Question);
                    IEnumerable<QuizQuestionReport> QuizCategoryReport = SqlMapper.Query<QuizQuestionReport>(
                    connection, "[Quiz].[usp_QuizQuestionReport_GetAllQuestionList]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
                    return QuizCategoryReport;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
