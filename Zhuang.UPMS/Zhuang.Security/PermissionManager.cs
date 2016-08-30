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

        public PermissionManager(string userId)
        {
            _userId = userId;
            _service = new PermissionService();
            Init();
        }

        public void Init()
        {
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

                Action<SecPermission> funRecursive =null;

                funRecursive = (c) =>
                {
                    var subPermissionList = allPermissionList.Where(p => p.ParentId == c.PermissionId).ToList();

                    if (subPermissionList.Count == 0)
                        return;

                    tempPermissionList.AddRange(subPermissionList);
                    foreach (var p in subPermissionList)
                    {
                        funRecursive(p);
                    }
                };

                if (momdule != null)
                {
                    funRecursive(momdule);
                }
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

        public bool HasElementPermission(string elementCode)
        {
            bool result = false;

            var element = GetElement(elementCode);

            if (element != null)
            {
                result = true;
            }

            return result;
        }

        public string GetElementRule(string elementCode)
        {
            var element = GetElement(elementCode);
            if (element == null) return null;

            var rule = element.RuleList.OrderBy(c => c.Priority).FirstOrDefault();
            if (rule == null)
                return null;
            else
                return rule.Value;
        }

        private ElementModel GetElement(string elementCode)
        {

            var pElement = _permissionList.Where(c => c.Type == PermissionType.Element.ToString() && c.Code == elementCode).FirstOrDefault();

            if (pElement == null) return null;

            ElementModel result = new ElementModel() { Id = pElement.PermissionId, Code = pElement.Code, Name = pElement.Name };

            result.RuleList = new List<RuleModel>();

            var pRuleList = _permissionList.Where(c => c.Type == PermissionType.Rule.ToString() && c.ParentId == result.Id).ToList();

            pRuleList.ForEach(c =>
            {
                result.RuleList.Add(new RuleModel() { Priority = c.Seq, Value = c.TypeValue });
            });

            return result;
        }
    }
}
