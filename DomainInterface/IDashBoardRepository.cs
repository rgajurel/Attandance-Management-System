using DomainEntities;
using DomainEntities.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IDashBoardRepository
    {
        IQueryable<Holidays> GetAllOrganisationHolidaysList();
        IQueryable<OrganisationEvents> GetAllOrganisationEvents();
        IQueryable<BirthDayEvents> GetAllUpcomingBirthdays();
        IQueryable<DailyCount> GetAllDailyAttandanceCount();
        IQueryable<StudentTotalByClass> GetAllTotalStudentbyClass();
    }
}
