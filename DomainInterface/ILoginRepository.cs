using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface ILoginRepository
    {
        string GetUserImage(string employeeid);
        string GetUserImage(double phoneno);
    }
}
