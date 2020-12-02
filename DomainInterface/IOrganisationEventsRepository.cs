using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface IOrganisationEventsRepository
    {
        #region Admin
        bool AddUpdateOrganisationevents(OrganisationEvents organisationEvents);
        List<OrganisationEvents> GetAllOrganisationEvents(OrganisationEvents organisationEvents);
        bool DeleteOrganisationEvents(int id);
        OrganisationEvents EditOrganisationEvents(int id);
        List<UserGroup> GetUserGroupBasedOnOrganisation(string id);
        #endregion
    }
}
