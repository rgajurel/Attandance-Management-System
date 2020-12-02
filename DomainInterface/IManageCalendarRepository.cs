using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IManageCalendarRepository
    {
        #region Admin
        bool AddUpdateManageCalendar(ManageCalendar department);
        List<ManageCalendar> GetAllManageCalendar();
        bool DeleteManageCalendar(int id);
        ManageCalendar EditManageCalendar(int id);
        #endregion
    }
}
