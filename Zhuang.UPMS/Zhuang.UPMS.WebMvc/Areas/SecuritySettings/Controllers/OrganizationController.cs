using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Model.Common;
using Zhuang.Web.Utility.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Areas.SecuritySettings.Controllers
{
    public class OrganizationController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();

        // GET: SecuritySettings/Organization
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult GetTree()
        {
            ContentResult contentResult = new ContentResult();

            var lsSecOrganization = _dba.QueryEntities<SecOrganization>("SecuritySettings.Menu.GetTree");

            List<TreeModel> lsTree = new List<TreeModel>();

            foreach (var item in lsSecOrganization)
            {
                lsTree.Add(new TreeModel()
                {
                    id = item.OrganizationId,
                    parentId = item.ParentId,
                    text = item.Name,
                    state = TreeStateType.open.ToString()
                });
            }

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(TreeModel.ToTreeModel(lsTree));
            return contentResult;
        }
    }
}