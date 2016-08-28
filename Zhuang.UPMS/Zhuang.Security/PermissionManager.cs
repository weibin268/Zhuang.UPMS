using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhuang.Security.Models;
using Zhuang.Security.Services;

namespace Zhuang.Security
{
    [Serializable]
    public class PermissionManager
    {
        PermissionService _service;
        IList<SecPermission> _permissionList;
        string _userId;

        private static PermissionManager _instance;
        private static object _objLock = new object();


        public static PermissionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PermissionManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private PermissionManager()
        {

        }

        public void Init(string userId)
        {
            _userId = userId;
            _service = new PermissionService();
            _permissionList = _service.GetListByUserId(_userId);
        }

        public IList<MenuModel> GetMenuList()
        {
            return GetMenuList(null);
        }

        public IList<MenuModel> GetMenuList(string moduleCode)
        {
            IList<MenuModel> result = new List<MenuModel>();

            var allPermissionList = _permissionList
                .Where(c => { return (c.Type == PermissionType.Module.ToString() || c.Type == PermissionType.Page.ToString()); })
                .OrderBy(c => c.Seq).ToList();

            List<SecPermission> tempPermissionList = null;
            if (!string.IsNullOrEmpty(moduleCode))
            {
                tempPermissionList = new List<SecPermission>();
                var momdule = allPermissionList.Where(c => c.Code == moduleCode).FirstOrDefault();

                Action<SecPermission> fun =null;

                fun = (c) =>
                {
                    var subPermissionList = allPermissionList.Where(p => p.ParentId == c.PermissionId).ToList();

                    if (subPermissionList.Count == 0)
                        return;

                    tempPermissionList.AddRange(subPermissionList);
                    foreach (var p in subPermissionList)
                    {
                        fun(p);
                    }
                };

                fun(momdule);
            }

            tempPermissionList = tempPermissionList ?? allPermissionList;

            foreach (var p in tempPermissionList)
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
