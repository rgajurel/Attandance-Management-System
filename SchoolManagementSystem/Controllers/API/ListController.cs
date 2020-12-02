using DomainEntities;
using DomainInterface;
using SchoolManagementSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolManagementSystem.Controllers.API
{
    
    [RoutePrefix("api/list")]
    [AllowAnonymous]
    public class ListController : ApiController
    {
        private readonly IDropDownRepository dropDownRepo;

        public ListController(IDropDownRepository dropDownRepo)
        {
            this.dropDownRepo = dropDownRepo;
        }

        [HttpGet]
        [Route("takeleavelist")]
        public IHttpActionResult ListLeaveType()
        {                    
          
            var leaveType = dropDownRepo.GetLeaveTypeBasedOnEmployee("".ToService().LoginInfo.EmployeeID);
            var leaveDays = dropDownRepo.GetTakeLeaveDaysMaster();
            var months = dropDownRepo.GetAllMonthDropDown();
            var year = dropDownRepo.GetActiveSessionDropDown();
            var approvedby = dropDownRepo.GetSuperAdminAndAdminNames("".ToService().LoginInfo.ID);
            var data = new MobileDropDownList()
            {
                Year =year,
                Month=months,
                LeaveType=leaveType,
                LeaveDays=leaveDays,
                ApprovedBy=approvedby
            };
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return null;
            }

        }


        [HttpGet]
        [Route("attandancelisttype")]
        public IHttpActionResult ListOfficialAttandanceType()
        {
            var datas = "".ToService();
            var leaveType = dropDownRepo.GetLeaveTypeBasedOnOrganisation(datas.LoginInfo.OrganisationID);
            var leaveDays = dropDownRepo.GetTakeLeaveDaysMaster();
            var months = dropDownRepo.GetAllMonthDropDown();
            var year = dropDownRepo.GetActiveSessionDropDown();
            var approvedby = dropDownRepo.GetSuperAdminAndAdminNames(datas.LoginInfo.ID);
            var data = new MobileDropDownList()
            {
                Year = year,
                Month = months,
                LeaveType = leaveType,
                LeaveDays = leaveDays,
                ApprovedBy = approvedby
            };
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return null;
            }



        }




    }
}
