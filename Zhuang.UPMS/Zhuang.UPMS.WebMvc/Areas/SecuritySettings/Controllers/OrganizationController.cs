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
    public class OrganizationController : Controller
    {
        DbAccessor _dba = DbAccessor.Get();
        OrganizationService _organizationService = new OrganizationService();

        // GET: SecuritySettings/Organization
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
            ViewBag.ParentName = _dba.ExecuteScalar<string>("SELECT Name FROM dbo.Sec_Organization WHERE OrganizationId=#OrganizationId#",
                new { OrganizationId = ParentId });

            SecOrganization model = new SecOrganization();
            model.ParentId = ParentId;
            if (id != null)
            {
                model = _organizationService.GetOrganizationById(id);
            }
            return View(model);
        }

        public ActionResult Select()
        {
            return View();
        }

        public JsonResult Save(SecOrganization model)
        {
            MyJsonResult mjr = new MyJsonResult();

            using (var dba = DbAccessor.Create())
            {
                try
                {
                    dba.BeginTran();

                    model.ModifiedById = SecurityContext.Current.User.UserId;
                    model.ModifiedDate = DateTime.Now;

                    if (model.OrganizationId == null)
                    {
                        model.OrganizationId = Guid.NewGuid().ToString();
                        model.RecordStatus = RecordStatus.Active;
                        model.CreatedById = SecurityContext.Current.User.UserId;
                        model.CreationDate = DateTime.Now;
                        dba.ExecuteNonQuery("Security.Organization.Insert", model);
                    }
                    else
                    {
                        dba.UpdateFields(model, "Name", "Code",
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

            try
            {
                _organizationService.DeleteRecursive(id);

                mjr.Success = true;
            }
            catch (Exception ex)
            {

                mjr.Success = false;
                mjr.Message = ex.Message;
            }

            return Json(mjr);
        }

        public ContentResult GetTree()
        {
            ContentResult contentResult = new ContentResult();

            var lsSecOrganization = _dba.QueryEntities<SecOrganization>("SecuritySettings.Organization.GetTree");

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

        public ContentResult GetList(int page, int rows)
        {
            return EasyUIHelper.GetDataGridPageData("SecuritySettings.Organization.List", "OrganizationId", page, rows);
        }

    }
}