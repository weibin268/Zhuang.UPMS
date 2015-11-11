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
                    dba.BeginTran();

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

        public JsonResult Delete(string id)
        {
            MyJsonResult mjr = new MyJsonResult();


            try
            {
                var user = new SecUser();
                user.UserId = id;
                user.RecordStatus = RecordStatus.Deleted;
                _dba.UpdateFields(user, "RecordStatus");


                mjr.Success = true;
            }
            catch (Exception ex)
            {

                mjr.Success = false;
                mjr.Message = ex.Message;
            }
      
            return Json(mjr);
        }

        public ContentResult GetList(int rows, int page, string filter)
        {
            ContentResult cr = new ContentResult();

            Dictionary<string, object> dicParam = new Dictionary<string, object>();

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("Filter_"))
                {
                    dicParam.Add(key.Replace("Filter_",""),Request.Form[key]);
                }
            }

            DataGridUrlReturnDataModel model = new DataGridUrlReturnDataModel();

            int totalRowCount = 0;
            model.rows = _dba.PageQueryDataTable("SecuritySettings.User.List", "userid", page, rows, out totalRowCount, dicParam);
            model.total = totalRowCount;

            cr.Content = Newtonsoft.Json.JsonConvert.SerializeObject(model,new Newtonsoft.Json.Converters.DataTableConverter());

            return cr;

        }
    }
}