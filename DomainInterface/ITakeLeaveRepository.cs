using DomainEntities;
using DomainEntities.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ITakeLeaveRepository
    {
        #region Admin
        List<LeaveType> GetLeaveTypeBasedOnOrganisation(string id);
        List<LeaveType> GetAccumulativeLeaveTypeBasedOnOrganisation(string id);
        List<DropDownCommon> GetEmployeeBasedOnOrganisationAndLeaveType(string organisationid, string leavetypeid);

        List<DropDownCommon> GetEmployeeBasedOnOrganisation(string organisationid);
        string CalculateRemainingLeave(TakeLeave takeleave);
        string CalculateRemainingLeave(GeneralViewModel<ClientTakeLeave> takeleave);
        bool AddUpdateTakeLeave(TakeLeave leave);
        bool ApproveLeave(string status,string id);
        List<TakeLeave> GetAllTakeLeave(TakeLeave search);
        TakeLeave EditTakeLeave(int id);

        bool AddUpdateNotificationTakeLeave(Notification notification,string employeeid);

        bool DeleteTakeLeave(int id);

        #endregion

        #region Client
        bool AddUpdateTakeLeave(ClientTakeLeave leave);
        List<TakeLeave> GetAllTakeLeave(ClientTakeLeave search);
        #endregion

        #region Mobile

        LeaveHistoryList LeaveHistoryList(GeneralViewModel<string>model);

        bool AddUpdateTakeLeave(GeneralViewModel<ClientTakeLeave> leave);

        LeaveHistoryStatusList LeaveHistoryStatus(GeneralViewModel<string> model);

        #endregion
    }
}
