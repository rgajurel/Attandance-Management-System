using DomainEntities;
using DomainEntities.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IUserRepository
    {
        User GetUserName(string username);
        bool AddUpdateUser(User user);
        List<Parents> GetAllParents(string prefix);
        List<Students> GetAllStudents(string prefix);
        User EditUser(int id);
        User GetUserByUserNameAndPassword(string UserName,string Password,string identifier,DeviceType devicetype);
        List<User> GetAllUsers(User search);
        bool ChekUserAlreadyExist(User user);
        bool DeleteUser(int id);
        bool ChangePassword(ChangePassword password,string mobile=null);

        #region Mobile

        UserProfileInfo GetUserProfileInfo(string employeeid);

        #endregion


    }
}
