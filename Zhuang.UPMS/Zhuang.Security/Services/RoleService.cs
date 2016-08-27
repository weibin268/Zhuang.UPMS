using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Data;
using Zhuang.Security.Models;

namespace Zhuang.Security.Services
{
    public class RoleService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecUser Get(string id)
        {
            return _dba.QueryEntity<SecUser>("Security.Role.Get", new { Id = id });
        }

    }
}
