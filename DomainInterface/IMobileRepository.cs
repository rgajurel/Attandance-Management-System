using DomainEntities;
using DomainEntities.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IMobileRepository
    {
       UpComingEventsList GetAllOrganisationEvents(GeneralViewModel<string> model);

        HolidayList GetAllOrganisationHolidaysList(GeneralViewModel<string> model);
    }
}
