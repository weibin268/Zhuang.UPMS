using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Zhuang.Security.Models;

namespace Zhuang.Security
{
    [Serializable]
    public class SecurityContext
    {
        private const string Key = "SecurityContext";

        private SecUser _user;

        public SecUser User
        {
            get
            {
                return _user;
            }
        }

        public SecurityContext(SecUser user)
        {
            _user = user;
        }

        public PermissionManager PermissionManager
        {
            get
            {
                return new PermissionManager(User.UserId);
            }
        }

        public static SecurityContext Current
        {
            get
            {
                var result = HttpContext.Current.Session[Key];
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return (SecurityContext)result;
                }

            }
            set
            {
                HttpContext.Current.Session[Key] = value;
            }
        }
    }
}
