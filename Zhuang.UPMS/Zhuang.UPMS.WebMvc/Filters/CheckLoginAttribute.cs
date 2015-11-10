using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Model.Common;
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

                //filterContext.HttpContext.Response.Write(
                //    string.Format("<script>top.window.location.href='{0}';</script>",
                //    urlLogin));

                filterContext.Result = new RedirectResult(urlLogin);
            }
        }

    }
}