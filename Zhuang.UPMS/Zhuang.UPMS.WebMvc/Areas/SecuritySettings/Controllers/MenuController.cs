using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Model.Common;
using Zhuang.Security;
using Zhuang.Security.Services;
using Zhuang.UPMS.WebMvc.App_Code;
using Zhuang.Web.Utility.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Areas.SecuritySettings.Controllers
{
    [Filters.CheckLogin]
    public class MenuController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();
        MenuService _menuService = new MenuService();

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

        public ActionResult Edit(string id, string ParentId)
        {
            ViewBag.ParentName = _dba.ExecuteScalar<string>("SELECT Name FROM dbo.Sec_Menu WHERE MenuId=#MenuId#",
                new { MenuId = ParentId });

            SecMenu model = new SecMenu();
            model.ParentId = ParentId;
            if (id != null)
            {
                model = _menuService.GetMenuById(id);
            }
            return View(model);
        }

        public JsonResult Save(SecMenu model)
        {
            MyJsonResult mjr = new MyJsonResult();

            using (var dba = DbAccessor.Create())
            {
                try
                {
                    dba.BeginTran();

                    model.ModifiedById = SecurityContext.Current.User.UserId;
                    model.ModifiedDate = DateTime.Now;

                    if (model.MenuId == null)
                    {
                        model.MenuId = Guid.NewGuid().ToString();
                        model.RecordStatus = RecordStatus.Active;
                        model.CreatedById = SecurityContext.Current.User.UserId;
                        model.CreationDate = DateTime.Now;
                        dba.ExecuteNonQuery("Security.Menu.Insert", model);
                    }
                    else
                    {
                        dba.UpdateFields(model, "Name", "Url", "IsExpand",
                            "MobilePhone",
                            "ModifiedById", "ModifiedDate");
                    }

                    dba.CommitTran();
                    mjr.Success = true;
                    mjr.Message = "保存成功！";
                }
                catch (Exception ex)
                {
                    dba.RollbackTran();
                    mjr.Success = false;
                    mjr.Message = ex.Message;
                }
            }

            return Json(mjr);
        }

        public JsonResult Delete(string id)
        {
            MyJsonResult mjr = new MyJsonResult();
            using (DbAccessor dba = DbAccessor.Create())
            {
                try
                {
                    dba.BeginTran();

                    _menuService.DeleteRecursive(id, dba);

                    dba.CommitTran();

                    mjr.Success = true;
                }
                catch (Exception ex)
                {
                    dba.RollbackTran();

                    mjr.Success = false;
                    mjr.Message = ex.Message;
                }
            }

            return Json(mjr);
        }

        public ContentResult GetMenus()
        {
            ContentResult contentResult = new ContentResult();

            var lsSecMenu = _dba.QueryEntities<SecMenu>("SecuritySettings.Menu.GetTree");

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