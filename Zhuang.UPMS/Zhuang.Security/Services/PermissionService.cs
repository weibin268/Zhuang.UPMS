using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Data;
using Zhuang.Security.Models;

namespace Zhuang.Security.Services
{
    [Serializable]
    public class PermissionService
    {
        
        public IList<SecPermission> GetListByUserId(string userId)
        {
            DbAccessor dba = DbAccessor.Get();

            string strSql = @"SELECT  *
            FROM    Sec_Permission a
            WHERE a.Status=1 and  EXISTS ( SELECT NULL
                             FROM   Sec_RolePermission rp
                             WHERE  a.PermissionId = rp.PermissionId
                                    AND rp.RoleId IN ( SELECT   RoleId
                                                       FROM     Sec_UserRole ur
                                                       WHERE    ur.UserId = #UserId# ) )
            ";

            return dba.QueryEntities<SecPermission>(strSql, new { UserId = userId });
        }
    }
}
