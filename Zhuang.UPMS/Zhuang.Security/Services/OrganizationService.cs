using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Data;
using Zhuang.Model.Common;

namespace Zhuang.Security.Services
{
    public class OrganizationService
    {
        DbAccessor _dba = DbAccessor.Get();

        public SecOrganization GetOrganizationById(string menuId)
        {
            return _dba.QueryEntity<SecOrganization>("Security.SecOrganization.GetSecOrganizationById", new { MenuId = menuId });
        }
    }
}
