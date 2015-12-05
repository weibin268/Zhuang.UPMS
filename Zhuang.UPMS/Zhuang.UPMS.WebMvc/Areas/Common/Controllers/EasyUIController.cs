using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.UPMS.WebMvc.App_Code;
using Zhuang.Web.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.Areas.Common.Controllers
{
    public class EasyUIController : Controller
    {

        DbAccessor _dba = DbAccessor.Get();

        // GET: Common/EasyUI
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult GetPage()
        {
            return EasyUIHelper.GetDataGridPageData();
        }

    }
}