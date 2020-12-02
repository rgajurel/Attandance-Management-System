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
    public class LanguageRepository : ILanguageRepository
    {
        public bool AddUpdateLanguage(Language language)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", language.ID);
                    parameters.Add("@Name", language.Name);
                    parameters.Add("@Code", language.Code);
                    parameters.Add("@Status", language.Status);
                    parameters.Add("@Image", language.Image);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateLanguage]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteLanguage(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteLanguage]", parameters, commandType: CommandType.StoredProcedure);
                    var savechanges = parameters.Get<Boolean>("@DeleteSuccess");
                    if (savechanges)
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
                throw ex;
            }
        }

        public Language EditLanguage(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Language language = SqlMapper.Query<Language>(connection, "[dbo].[EditLanguage]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return language;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Language> GetAllLanguage()
        {

            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    List<Language> languageList = SqlMapper.Query<Language>(connection, "[dbo].[GetAllLanguage]", param, commandType: CommandType.StoredProcedure).ToList();
                    return languageList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
