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
using Zhuang.Models;
using Zhuang.Security.Models;
using Zhuang.Security.Services;

namespace Zhuang.UPMS.WebApi.Controllers
{
    public class UserController : ApiController
    {
        UserService _service = new UserService();

        public SecUser GetUserById(string userId)
        {
            return _service.Get(userId);
        }
    }
}
