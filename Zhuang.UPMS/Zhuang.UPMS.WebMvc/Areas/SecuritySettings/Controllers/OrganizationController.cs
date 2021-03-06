﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Models;
using Zhuang.Security;
using Zhuang.Security.Commons;
using Zhuang.Security.Models;
using Zhuang.Security.Services;
using Zhuang.UPMS.WebMvc.App_Code;
using Zhuang.Web.EasyUI.Models;

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
                model = _organizationService.Get(id);
            }
            return View(model);
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
                        model.Status = (int)StatusType.Enabled;
                        model.CreatedById = SecurityContext.Current.User.UserId;
                        model.CreatedDate = DateTime.Now;
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

    }
}