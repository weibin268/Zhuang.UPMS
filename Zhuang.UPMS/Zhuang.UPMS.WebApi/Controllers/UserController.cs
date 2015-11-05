using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Zhuang.Data;
using Zhuang.Data.Common;
using Zhuang.Data.SqlCommands.Store;
using Zhuang.Model.Common;

namespace Zhuang.UPMS.WebApi.Controllers
{
    public class UserController : ApiController
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecUser GetById(string userId)
        {

            return _dba.QueryEntity<SecUser>("Security.SecUser.GetById", new { UserId = userId });

        }
    }
}
