﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Models;
using Zhuang.Security;
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

            
            List<TreeModel> lsTree = new List<TreeModel>();

            foreach (var item in SecurityContext.Current.PermissionManager.GetMenuList("left.menu"))
            {
                lsTree.Add(new TreeModel()
                {
                    id = item.Id,
                    parentId = item.ParentId,
                    text = item.Name,
                    state =false ? TreeModel.State.open.ToString() : TreeModel.State.closed.ToString(),
                    attributes = new TreeModel.Attributes() { url = item.Url }
                });
            }

            ViewBag.TreeModels = TreeModel.ToTreeModel(lsTree);

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

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(TreeModel.ToTreeModel( lsTree));
            return contentResult;
        }
        #endregion
    }
}