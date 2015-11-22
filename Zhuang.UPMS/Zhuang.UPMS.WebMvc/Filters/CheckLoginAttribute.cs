using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Models;
using Zhuang.Security;

namespace Zhuang.UPMS.WebMvc.Filters
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SecurityContext.Current == null)
            {
                string urlLogin = UrlHelper.GenerateContentUrl("~/Login/Index", filterContext.HttpContext);
                string strScript = string.Format("<script 'text/javascript'>top.window.location.href='{0}';</script>", urlLogin);

                //filterContext.HttpContext.Response.Write(
                //    string.Format("<script>top.window.location.href='{0}';</script>",
                //    urlLogin));

                //filterContext.Result = new RedirectResult(urlLogin);

                //filterContext.Result = new JavaScriptResult() { Script = strScript };

                filterContext.Result = new ContentResult() { Content = strScript };

            }
        }

    }
}