using Dapper;
using DomainEntities;
using DomainEntities.Mobile;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
   public class MobileRepository: IMobileRepository
    {
        public UpComingEventsList GetAllOrganisationEvents(GeneralViewModel<string> model)
        {
            try
            {
                var list = new UpComingEventsList();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", model.LoginInfo.OrganisationID);                 
                    var organisationEvents = SqlMapper.Query<UpComingEvents>(connection, "[dbo].[GetAllOrganisationEventsMobile]", param, commandType: CommandType.StoredProcedure).ToList();
                    if (organisationEvents.Count() > 0)
                    {
                        list.Events = organisationEvents.ToList();
                        list.Message = "";
                    }
                    else
                    {
                        list.Events = null;
                        list.Message = "No Up Coming Events Availiable";
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HolidayList GetAllOrganisationHolidaysList(GeneralViewModel<string> model)
        {
            try
            {
                var list = new HolidayList();
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@OrganisationID", model.LoginInfo.OrganisationID);
                    var holidaysList = SqlMapper.Query<Holidays>(connection, "[dbo].[GetAllHolidaysList]", param, commandType: CommandType.StoredProcedure).ToList();
                    if (holidaysList.Count() > 0)
                    {
                        list.Holidays = holidaysList.ToList();
                        list.Message = "";
                    }
                    else
                    {
                        list.Holidays = null;
                        list.Message = "No Records Availiable";
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
