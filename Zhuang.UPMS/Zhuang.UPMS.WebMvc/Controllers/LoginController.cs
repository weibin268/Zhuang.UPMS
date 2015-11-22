using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Models;
using Zhuang.Security;
using Zhuang.Security.Services;
using Zhuang.Web.Utility.Images;

namespace Zhuang.UPMS.WebMvc.Controllers
{
    public class LoginController : Controller
    {
        #region View
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Action
        public JsonResult Login(SecUser model, string ValidateCode)
        {
            MyJsonResult mjr = new MyJsonResult();

            var obj = Session["ValidateCode"];
            if (obj == null || obj.ToString() != ValidateCode)
            {
                mjr.Success = false;
                mjr.Message = "验证码不正确！";
                mjr.Data = 1;
                if (obj == null)
                {
                    mjr.Data = 11;
                }
                return Json(mjr);
            }

            UserService userService = new UserService();

            var user = userService.GetUserByLoginName(model.LoginName);

            if (user == null)
            {
                mjr.Success = false;
                mjr.Message = "用户名不正确！";
                mjr.Data = 2;
            }
            else
            {
                if (user.Password != model.Password)
                {
                    mjr.Success = false;
                    mjr.Message = "密码不正确！";
                    mjr.Data = 3;
                }
                else
                {
                    SecurityContext.Current = new SecurityContext() { User = user };
                    //Session[SSessionIndex.IsAuthorizedForCKEditor] = true;
                    mjr.Success = true;
                }
            }


            return Json(mjr);
        }

        public ActionResult Logout()
        {
            SecurityContext.Current = null;
            return View("Index");
        }

        public FileContentResult GetValidateCode()
        {
            ValidateCodeHelper vc = new ValidateCodeHelper();
            string strValidateCode = vc.CreateValidateCode(4);
            Session["ValidateCode"] = strValidateCode;
            return File(vc.CreateValidateGraphic(strValidateCode), "image/jpeg");

        } 
        #endregion
    }
}