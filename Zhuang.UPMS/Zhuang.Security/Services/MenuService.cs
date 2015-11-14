using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Model.Common;
using Zhuang.Data;

namespace Zhuang.Security.Services
{
    public class MenuService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecMenu GetMenuById(string menuId)
        {
            return _dba.QueryEntity<SecMenu>("Security.Menu.GetMenuById", new { MenuId = menuId });
        }
    }
}
