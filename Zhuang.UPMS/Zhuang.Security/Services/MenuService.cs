using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Models;
using Zhuang.Data;

namespace Zhuang.Security.Services
{
    public class MenuService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecMenu GetMenuById(string menuId)
        {
            return _dba.QueryEntity<SecMenu>("Security.Menu.Get", new { MenuId = menuId });
        }

        public void DeleteRecursive(string menuId)
        {
            DeleteRecursive(menuId, _dba);
        }

        public void DeleteRecursive(string menuId, DbAccessor dba)
        {
            string strSql = "select * from Sec_Menu where ParentId=#ParentId#";
            var childrenMenu = dba.QueryEntities<SecMenu>(strSql, new { ParentId = menuId });
            foreach (var item in childrenMenu)
            {
                DeleteRecursive(item.MenuId, dba);
            }
            dba.ExecuteNonQuery("Security.Menu.Delete", new { MenuId = menuId });
        }
    }
}
