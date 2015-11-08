using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Data;
using Zhuang.Model.Common;

namespace Zhuang.Security.Service
{
    public class UserService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecUser GetUserById(string userId)
        {
            return _dba.QueryEntity<SecUser>("Security.User.GetUserById", new { UserId = userId });
        }
    }
}
