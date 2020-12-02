using Dapper;
using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SettingsRepository : ISettingsRepository
    {
        public string GetSettingByIDandGroup(string SettingsID, string OfGroups)
        {
            IDbConnection con = Infrastructure.DBManager.DbConnect();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@SettingsID", SettingsID);
                parameters.Add("@OfGroups", OfGroups);

                Settings setting = SqlMapper.Query<Settings>(con, "GetSettingsByIdandGroup", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                string SettingValue = "";
                if (!String.IsNullOrEmpty(setting.SettingsValue))
                {
                    SettingValue = setting.SettingsValue;
                }
                else if (!String.IsNullOrEmpty(setting.DefaultValue))
                {
                    SettingValue = setting.DefaultValue;
                }
               
                return SettingValue;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
