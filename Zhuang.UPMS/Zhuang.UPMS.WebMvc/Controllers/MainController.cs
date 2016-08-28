using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Models;
using Zhuang.Security.Models;
using Zhuang.Web.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Controllers
{
    [Filters.CheckLogin]
    public class MainController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();

        #region View
        public ActionResult Index()
        {

            //var lsSecMenu = _dba.QueryEntities<SecMenu>(@"SELECT * FROM Sec_Menu
            //WHERE Status=1");

            //List<TreeUrlReturnModel> lsTree = new List<TreeUrlReturnModel>();

            //foreach (var item in lsSecMenu)
            //{
            //    lsTree.Add(new TreeUrlReturnModel()
            //    {
            //        id = item.MenuId,
            //        parentId = item.ParentId,
            //        text = item.Name,
            //        state = item.IsExpand ? TreeUrlReturnModel.State.open.ToString() : TreeUrlReturnModel.State.closed.ToString(),
            //        attributes = new TreeUrlReturnModel.Attributes(){ url = item.Url }
            //    });
            //}


            var lsSecMenu = _dba.QueryEntities<SecPermission>(@"SELECT * FROM Sec_Permission 
            WHERE Status=1 AND PermissionId<>'5CA421DA-0F25-4B3B-83AC-C11F15B3569E'").OrderBy(c=>c.Seq);

            List<TreeModel> lsTree = new List<TreeModel>();

            foreach (var item in lsSecMenu)
            {
                lsTree.Add(new TreeModel()
                {
                    id = item.PermissionId,
                    parentId = item.ParentId,
                    text = item.Name,
                    state =false ? TreeModel.State.open.ToString() : TreeModel.State.closed.ToString(),
                    attributes = new TreeModel.Attributes() { url = item.TypeValue }
                });
            }


            ViewBag.TreeModels = TreeModel.ToTreeUrlReturnModel(lsTree);

            return View();
        }
        #endregion

        #region Action
        public ContentResult GetMenus()
        {
            ContentResult contentResult = new ContentResult();

            var lsSecMenu = _dba.QueryEntities<SecMenu>(@"SELECT * FROM Sec_Menu
            WHERE Status='Active'");

            List<TreeModel> lsTree = new List<TreeModel>();

            foreach (var item in lsSecMenu)
            {
                lsTree.Add(new TreeModel()
                {
                    id = item.MenuId,
                    parentId = item.ParentId,
                    text = item.Name,
                    state = item.IsExpand ? TreeModel.State.open.ToString() : TreeModel.State.closed.ToString(),
                    attributes = new { url = item.Url }
                });
            }

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(TreeModel.ToTreeUrlReturnModel( lsTree));
            return contentResult;
        }
        #endregion
    }
}