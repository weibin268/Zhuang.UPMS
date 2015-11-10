using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Model.Common;
using Zhuang.Web.Utility.EasyUI;

namespace Zhuang.UPMS.WebMvc.Controllers
{
    public class MainController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();

        #region View
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Action
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
                    attributes = new { url = item.Url }
                });
            }

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(lsTree);
            return contentResult;
        } 
        #endregion
    }
}