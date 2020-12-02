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
    public class LanguageParameterRepository : ILanguageParameterRepository
    {
        public bool AddUpdateLanguageParameter(LangaugeParameter languageParameter)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", languageParameter.ID);
                    parameters.Add("@LanguageID", languageParameter.LanguageID);
                    parameters.Add("@Key", languageParameter.Key);
                    parameters.Add("@Page", languageParameter.Page);
                    parameters.Add("@OriginalWordInEnglish", languageParameter.OriginalWordInEnglish);
                    parameters.Add("@TranslatedWord", languageParameter.TranslatedWord);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateLanguageParameter]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteLanguageParameter(int id)
        {
            throw new NotImplementedException();
        }

        public LangaugeParameter EditLanguageParameter(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    LangaugeParameter languageParameter = SqlMapper.Query<LangaugeParameter>(connection, "[dbo].[EditLanguageParameter]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return languageParameter;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<LangaugeParameter> GetAllLanguageParameter()
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<LangaugeParameter> languageParameterList = SqlMapper.Query<LangaugeParameter>(connection, "[dbo].[GetAllLanguageParameter]", param, commandType: CommandType.StoredProcedure).ToList();
                    return languageParameterList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
