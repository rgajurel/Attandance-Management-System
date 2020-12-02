using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public  interface IDesignationRepository
    {
        #region Admin
        bool AddUpdateDesignation(Designations designation);
        List<Designations> GetAllDesignation();
        bool DeleteDesignaiton(int id);
        Designations EditDesignation(int id);
        #endregion
    }
}
