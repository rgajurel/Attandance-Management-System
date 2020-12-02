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
    public class OrganisationEventsRepository : IOrganisationEventsRepository
    {
        public bool AddUpdateOrganisationevents(OrganisationEvents organisationEvents)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", organisationEvents.ID==null?0:organisationEvents.ID);
                    parameters.Add("@EventName", organisationEvents.EventName);
                    parameters.Add("@EventDescription", organisationEvents.EventDescription);
                    parameters.Add("@OrganisationID", organisationEvents.OrganisationID);
                    parameters.Add("@NotificationType", organisationEvents.NotificationType);
                    parameters.Add("@NepaliDateFrom", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(organisationEvents.DateFrom));
                    parameters.Add("@DateFrom", organisationEvents.DateFrom);
                    parameters.Add("@NepaliDateTo", DateConversionHelper.GetEnglsihTimeToNepaliDateTime(organisationEvents.DateTo));
                    parameters.Add("@DateTo", organisationEvents.DateTo);
                    parameters.Add("@GroupID", organisationEvents.GroupID);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateOrganisationEvents]", parameters, commandType: CommandType.StoredProcedure);
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

        public bool DeleteOrganisationEvents(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteOrganisationEvents]", parameters, commandType: CommandType.StoredProcedure);
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

        public OrganisationEvents EditOrganisationEvents(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    OrganisationEvents organisationEvents = SqlMapper.Query<OrganisationEvents>(connection, "[dbo].[EditOrganisationEvents]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return organisationEvents;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<OrganisationEvents> GetAllOrganisationEvents(OrganisationEvents organisationEvents)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@EventNameSearch", organisationEvents.EventNameSearch == null ? "" : organisationEvents.EventNameSearch);
                    param.Add("@OrganisationIDSearch", organisationEvents.OrganisationIDSearch);
                    param.Add("@NotificationTypeSearch", organisationEvents.NotificationTypeSearch);
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    param.Add("@offset", organisationEvents.offset);
                    param.Add("@PageSize", organisationEvents.pageSize);
                    List<OrganisationEvents> organisationEventList = SqlMapper.Query<OrganisationEvents>(connection, "[dbo].[GetAllOrganisationEvents]", param, commandType: CommandType.StoredProcedure).ToList();
                      return organisationEventList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<UserGroup> GetUserGroupBasedOnOrganisation(string id)
        {
            throw new NotImplementedException();
        }
    }
}
