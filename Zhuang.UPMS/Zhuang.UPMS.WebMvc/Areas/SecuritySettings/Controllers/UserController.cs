using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Models;
using Zhuang.Security;
using Zhuang.Security.Services;
using Zhuang.UPMS.WebMvc.App_Code;
using Zhuang.Web.Utility.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Areas.SecuritySettings.Controllers
{
    [Filters.CheckLogin]
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

                        #region 校验数据
                        int count = _dba.ExecuteScalar<int>("SecuritySettings.User.CountByLoginName",
                                           new { LoginName = model.LoginName, RecordStatus = RecordStatus.Active });

                        if (count > 0)
                        {
                            mjr.Success = false;
                            mjr.Message = "登录名已存在！";
                            goto End;
                        } 
                        #endregion

                        model.UserId = Guid.NewGuid().ToString();
                        model.RecordStatus = RecordStatus.Active;
                        model.CreatedById = SecurityContext.Current.User.UserId;
                        model.CreationDate = DateTime.Now;
                        dba.Insert<SecUser>(model);
                    }
                    else
                    {
                        dba.UpdateFields(model, "LoginName", "Password", "Name", "Sex", "Birthday",
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

            End:

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

        public ContentResult GetList(int page, int rows)
        {
            return EasyUIHelper.GetDataGridPageData("SecuritySettings.User.List", "userid", page, rows);
        }
    }
}