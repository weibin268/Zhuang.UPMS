using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Web.Utility.Images;

namespace Zhuang.UPMS.WebMvc.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult GetValidateCode()
        {

            ValidateCodeHelper vc = new ValidateCodeHelper();
            string strValidateCode = vc.CreateValidateCode(4);
            Session["ValidateCode"] = strValidateCode;
            return File(vc.CreateValidateGraphic(strValidateCode), "image/jpeg");

        }
    }
}