using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
  public interface IUserRole
    {
        IEnumerable<Role> RoleGet(int? roleID = null);
        IEnumerable<Menu> MenuGet(bool IsAdmin);
       // ReturnType RoleSave(Role oUser, string userName);
        IEnumerable<MenuRole> RoleMenuGet(int? roleID = null);
        ReturnType RoleMenuSave(Role oRole, string xml, string userName);
        IEnumerable<Menu> MenuGet(int roleID);
        IEnumerable<Menu> MenuGetBasedOnLoggedInUserRole(string userName);
    }
}
