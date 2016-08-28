using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Security.Models;
using Zhuang.Security.Services;

namespace Zhuang.Security
{
    public class PermissionManager
    {
        PermissionService _service;
        IList<SecPermission> _permissionList;
        string _userId;

        public PermissionManager(string userId)
        {
            _userId = userId;
            _service = new PermissionService();
            _permissionList = _service.GetListByUserId(_userId);
        }

        public IList<MenuModel> GetMenuList(string userId)
        {
            IList<MenuModel> result = new List<MenuModel>();

            var pMenu = _service.GetListByUserId(userId)
                .Where(c => { return (c.Type == PermissionType.Module.ToString() || c.Type == PermissionType.Page.ToString()); })
                .OrderBy(c => c.Seq);

            foreach (var p in pMenu)
            {
                result.Add(new MenuModel()
                {
                    Id = p.PermissionId,
                    ParentId = p.ParentId,
                    Name = p.Name,
                    Url = p.TypeValue,
                    Seq = p.Seq,
                });
            }

            return result;
        }
    }
}
