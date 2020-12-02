using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IDropDownRepository
    {

        IEnumerable<Role> RoleGet(int? roleID = default(int?));
        List<UserGroup> GetUserGroup();
        List<NotificationTypes> GetNotificationTypes();
        List<DropDownCommon> GetErrorList();
        List<DropDownCommon> GetSchoolTypeDropDown();
       
        List<DropDownCommon> GetClassTypeDropDown();
        List<DropDownCommon> GetSectionDropDown();

        List<DropDownCommon> GetTermDropDown();
        List<DropDownCommon> GetSessionDropDown();
        List<DropDownCommon> GetActiveSessionDropDown();
        List<DropDownCommon> GetFacultyDropDown();
        List<DropDownCommon> GetDocumentsDropDown();
        List<DropDownCommon> GetStudentsCategoryDropDown();
        List<DropDownCommon> GetReligionDropDown();
        List<DropDownCommon> GetHouseDropDown();
          List<DropDownCommon> GetCasteDropDown();

        List<DropDownCommon> GetClasswDropDown();
        List<DropDownCommon> GetBloodGroupDropDown();

        List<DropDownCommon> GetJobTypeDropDown();

        List<DropDownCommon> GetAllOrganisation();

        List<DropDownCommon> GetAllSalaryHead();

        List<DropDownCommon> GetAllLeaveType();

        List<DropDownCommon> GetMonthDropDown();

        List<DropDownCommon> GetAllMonthDropDown();
        List<DropDownCommon> GetTypeDropDown();
        List<DropDownCommon> GetPersonnelTypeDropDown();
        List<DropDownCommon> GetTakeLeaveDaysMaster();
        List<DropDownCommon> GetLoginEmployeeName();
        List<DropDownCommon> GetLeaveTypeBasedOnEmployee(string employeeid);
        List<DropDownCommon> GetSuperAdminAndAdminNames();
        List<DropDownCommon> GetAllLanguage();
        List<DropDownCommon> GetSuperAdminAndAdminNames(string id);

        List<DropDownCommon> GetLeaveTypeBasedOnOrganisation(string id);

        List<DropDownCommon> GetSalartTypeDropDown();


    }
}
