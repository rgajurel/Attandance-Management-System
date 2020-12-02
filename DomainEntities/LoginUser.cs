using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DomainEntities
{
    public class LoginUser
    {
            

        public string UserName { get { return CurrentUser(); } }
        public bool IsAdmin { get { return CurrentUserIsAdmin(); } }
        public bool IsClient { get { return CurrentUserIsClient(); } }

        public bool IsSuperAdmin { get { return CurrentUserIsSuperAdmin(); } }
        public string UserImage { get { return CurrentUserImage(); } }

        public string LoggedInuserID { get { return CurrentUserID(); } }

        public int LoggedInEmployeeID { get { return CurrentEmployeeID(); } }


        private string CurrentUserID()
        {
            HttpContext context = HttpContext.Current;
            string key;
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim nameClaim = currentClaimsPrincipal.FindFirst("ID");
            key = nameClaim.Value;
            return key;
        }
        private int CurrentEmployeeID()
        {
            HttpContext context = HttpContext.Current;
            int key;
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim nameClaim = currentClaimsPrincipal.FindFirst("employeeID");
            key = Convert.ToInt16(nameClaim.Value);
            return key;
        }
        private string CurrentUser()
        {
            HttpContext context = HttpContext.Current;

            string key;
            //key = context.Request.Cookies[Constant.UserNameCookie]?.Value ?? string.Empty;
            //return Crypto.Decrypt(key);
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim nameClaim = currentClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (nameClaim != null)
            {
                key = nameClaim.Value;
            }
            else
            {
                key = string.Empty;
            }
            return key;
        }


        private bool CurrentUserIsAdmin()
        {
            HttpContext context = HttpContext.Current;
            bool key;           
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim nameClaim = currentClaimsPrincipal.FindFirst("IsAdmin");
            key = Convert.ToBoolean(nameClaim.Value);
            return key;
        }

        private bool CurrentUserIsClient()
        {
            HttpContext context = HttpContext.Current;
            bool key;
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim nameClaim = currentClaimsPrincipal.FindFirst("isClient");
            key = Convert.ToBoolean(nameClaim.Value);
            return key;
        }

        private bool CurrentUserIsSuperAdmin()
        {
            HttpContext context = HttpContext.Current;
            bool key;
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim nameClaim = currentClaimsPrincipal.FindFirst("isSuperAdmin");
            key = Convert.ToBoolean(nameClaim.Value);
            return key;
        }

        private string CurrentUserImage()
        {
            HttpContext context = HttpContext.Current;

            string key;
            
            ClaimsPrincipal currentClaimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

            Claim userImageClaim = currentClaimsPrincipal.FindFirst("Image");
            if (userImageClaim != null)
            {
                key = userImageClaim.Value.ToString();
                string fullPath = HttpContext.Current.Request.MapPath("~" + key);
                if (!System.IO.File.Exists(fullPath))
                {
                    key = "/Content/Images/Students/Students.png";
                }
            }
            else
            {
                key = "/Content/Images/Students/Students.png";
            }

            return key;
        }
    }
}
