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
    public class QuizCategoryReportRepository : IQuizCategoryReport
    {
        public IEnumerable<QuizCategoryReport> GetAllQuizListing(SearchParamQuizCategoryreport objInfo)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters GetAllQuizParam = new DynamicParameters();
                    GetAllQuizParam.Add("@PageSize", objInfo.PageSize);
                    GetAllQuizParam.Add("@PageIndex", objInfo.PageIndex);
                    GetAllQuizParam.Add("@SearchQuizTitle", objInfo.Quiztitle == null?"": objInfo.Quiztitle);
                    GetAllQuizParam.Add("@SearchCategoryID", objInfo.CategoryID);
                    IEnumerable<QuizCategoryReport> QuizCategoryReport = SqlMapper.Query<QuizCategoryReport>(
                    connection, "[Quiz].[usp_QuizCategoryReport_GetAllQuizInfo]", GetAllQuizParam, commandType: CommandType.StoredProcedure);
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
