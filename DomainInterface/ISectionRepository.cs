using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface ISectionRepository
    {
        #region Admin
        bool AddUpdateSection(Section section);
        List<Section> GetAllSection();
        bool DeleteSection(string section);
        Section EditSection(int id);

        int GetSectionCount(string section);

        #endregion
    }
}
