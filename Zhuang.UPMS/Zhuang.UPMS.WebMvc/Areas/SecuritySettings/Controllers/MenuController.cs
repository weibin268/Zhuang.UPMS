using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Model.Common;
using Zhuang.UPMS.WebMvc.App_Code;
using Zhuang.Web.Utility.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Areas.SecuritySettings.Controllers
{
    public class MenuController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();

        // GET: SecuritySettings/Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            ViewBag.ParentId = Request.QueryString["ParentId"];

            return View();
        }

        public ContentResult GetMenus()
        {
            ContentResult contentResult = new ContentResult();

            var lsSecMenu = _dba.QueryEntities<SecMenu>(@"SELECT * FROM dbo.Sec_Menu
            WHERE RecordStatus='Active'");

            List<TreeModel> lsTree = new List<TreeModel>();

            foreach (var item in lsSecMenu)
            {
                lsTree.Add(new TreeModel()
                {
                    id = item.MenuId,
                    parentId = item.ParentId,
                    text = item.Name,
                    state = item.IsExpand ? TreeStateType.open.ToString() : TreeStateType.closed.ToString(),
                    attributes = new { url = item.Url }
                });
            }

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(TreeModel.ToTreeModel(lsTree));
            return contentResult;
        }

        public ContentResult GetList(int page, int rows)
        {
            return EasyUIHelper.GetDataGridPageData("SecuritySettings.Menu.List", "menuid", page, rows);
        }
    }
}