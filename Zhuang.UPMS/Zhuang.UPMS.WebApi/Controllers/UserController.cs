using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zhuang.Data;
using Zhuang.Model.Common;

namespace Zhuang.UPMS.WebApi.Controllers
{
    public class UserController : ApiController
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecUser GetById(string userId)
        {
            return _dba.QueryEntity<SecUser>("select * from sec_user where UserId=#UserId#", new { UserId = userId });
        }
    }
}
