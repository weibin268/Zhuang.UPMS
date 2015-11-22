using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Data;
using Zhuang.Models;

namespace Zhuang.Security.Services
{
    public class UserService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecUser GetUserById(string userId)
        {
            return _dba.QueryEntity<SecUser>("Security.User.GetUserById", new { UserId = userId });
        }

        public SecUser GetUserByLoginName(string loginName)
        {
            return _dba.QueryEntity<SecUser>("Security.User.GetUserBy",
                new { LoginName = loginName, RecordStatus = RecordStatus.Active });
        }
    }
}
