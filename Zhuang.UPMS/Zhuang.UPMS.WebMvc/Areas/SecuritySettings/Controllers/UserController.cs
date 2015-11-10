using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Model.Common;
using Zhuang.Security;
using Zhuang.Security.Services;
using Zhuang.Web.Utility.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Areas.SecuritySettings.Controllers
{
    public class UserController : Controller
    {
        UserService _userService = new UserService();
        DbAccessor _dba = DbAccessor.Get();

        // GET: SecuritySettings/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            SecUser user = new SecUser();
            if (id != null)
            {
                user = _userService.GetUserById(id);
            }
            return View(user);
        }

        public JsonResult Save(SecUser model)
        {
            MyJsonResult mjr = new MyJsonResult();

            using (var dba = DbAccessor.Create())
            {
                try
                {
                    model.ModifiedById = SecurityContext.Current.User.UserId;
                    model.ModifiedDate = DateTime.Now;

                    if (model.UserId == null)
                    {
                        model.UserId = Guid.NewGuid().ToString();
                        model.RecordStatus = RecordStatus.Active;
                        model.CreatedById = SecurityContext.Current.User.UserId;
                        model.CreationDate = DateTime.Now;
                        dba.Insert<SecUser>(model);
                    }
                    else
                    {
                        dba.UpdateFields(model, "LoginName", "Password", "Name",
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

        public ContentResult GetList(int rows, int page, string filter)
        {
            ContentResult cr = new ContentResult();


            string strWhere = "";
            if (filter != null)
            {
                strWhere = string.Format("UserName like '%{0}%' or Name like '%{0}%' or Address like '%{0}%'", filter);
            }

            DataGridUrlReturnDataModel model = new DataGridUrlReturnDataModel();

            int totalRowCount = 0;
            model.rows = _dba.PageQueryDataTable("select * from sec_user", "userid", page, rows, out totalRowCount);
            model.total = totalRowCount;

            cr.Content = Newtonsoft.Json.JsonConvert.SerializeObject(model,new Newtonsoft.Json.Converters.DataTableConverter());

            return cr;

        }
    }
}