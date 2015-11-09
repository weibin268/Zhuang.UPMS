using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Web.Utility.EasyUI;

namespace Zhuang.UPMS.WebMvc.Controllers
{
    public class MainController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();

        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult GetMenus()
        {
            ContentResult contentResult = new ContentResult();

            var lsTree = _dba.QueryEntities<TreeModel>(@"SELECT MenuId AS id,ParentId AS parentId,MenuName AS name 
                                            FROM dbo.Sys_Menu
                                            WHERE ModuleType='sys'");

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(lsTree);
            return contentResult;
        }
    }
}