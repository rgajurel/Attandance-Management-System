using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IChangePasswordRepository
    {
        #region Client

        bool checkUser(string email, string oldPassword);
        bool changePassword(string email,string OldPassword, string newPassword);
        #endregion
    }
}
