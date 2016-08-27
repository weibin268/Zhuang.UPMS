using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Security.Models;
using Zhuang.Data;

namespace Zhuang.Security.Services
{
    public class MenuService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecMenu Get(string id)
        {
            return _dba.QueryEntity<SecMenu>("Security.Menu.Get", new { Id = id });
        }

        public void DeleteRecursive(string menuId)
        {
            DeleteRecursive(menuId, _dba);
        }

        public void DeleteRecursive(string id, DbAccessor dba)
        {
            string strSql = "select * from Sec_Menu where ParentId=#ParentId#";
            var childrenMenu = dba.QueryEntities<SecMenu>(strSql, new { ParentId = id });
            foreach (var item in childrenMenu)
            {
                DeleteRecursive(item.MenuId, dba);
            }
            dba.ExecuteNonQuery("Security.Menu.Delete", new { MenuId = id });
        }
    }
}
