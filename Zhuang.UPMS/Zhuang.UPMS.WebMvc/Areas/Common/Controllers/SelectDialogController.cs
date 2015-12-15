using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zhuang.UPMS.WebMvc.Areas.Common.Controllers
{
    public class SelectDialogController : Controller
    {
        // GET: Common/SelectDialog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree()
        {
            return View();
        }
    }
}