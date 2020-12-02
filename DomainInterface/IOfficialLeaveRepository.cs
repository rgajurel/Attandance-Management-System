using DomainEntities;
using DomainEntities.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IOfficialLeaveRepository
    {
        #region Admin
        List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id);
        List<DropDownCommon> GetEmployeeBasedOnOrganisation(string organisationid);
        bool AddUpdateOfficialLeave(Attandance leave);
        List<Attandance> GetAllOfficialLeave(Attandance search);
        bool ApproveLeave(string status, string id);

        Attandance EditOfficialLeave(int id);
        bool DeleteOfficialLeave(int id);

        #endregion Admin

        #region Client
        List<TakeLeave> GetAllOfficialLeaveClient(Attandance search);
        bool AddUpdateTravellRequest(Attandance leave);
        #endregion Client

        #region Mobile

        bool AddUpdateMobileTravellRequest(Attandance leave);
        TravelRequestList TravelRequestList(GeneralViewModel<string> model);

        #endregion
    }
}
