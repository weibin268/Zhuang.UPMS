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

        public SecUser Get(string id)
        {
            return _dba.QueryEntity<SecUser>("Security.User.Get", new { UserId = id });
        }

        public SecUser GetUserByLoginName(string loginName)
        {
            return _dba.QueryEntity<SecUser>("Security.User.GetBy",
                new { LoginName = loginName, RecordStatus = RecordStatus.Active });
        }
    }
}
