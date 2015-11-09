using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zhuang.UPMS.WebMvc.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult GetMenus()
        {
            ContentResult contentResult = new ContentResult();
            //contentResult.Content = sys_MenuBLL.GetJson4EasyUI("View_Cms_Menu");
            return contentResult;
        }
    }
}