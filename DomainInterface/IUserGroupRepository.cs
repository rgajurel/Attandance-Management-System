using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
   public interface IUserGroupRepository
    {

        #region Admin
        bool AddUpdateUserGroup(UserGroup usergroup);
        List<UserGroup> GetAllUserGroup(UserGroup userGroupSearch);
        bool DeleteUserGroup(int ID);
        UserGroup EditUserGroup(int id);

        UserGroup GetUserGroup(string username);

        #endregion
    }
}
