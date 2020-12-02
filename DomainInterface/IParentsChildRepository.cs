using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IParentsChildRepository
    {
        #region Client
        List<ParentsChild> GetAllStudents(string email);
        #endregion
    }
}
