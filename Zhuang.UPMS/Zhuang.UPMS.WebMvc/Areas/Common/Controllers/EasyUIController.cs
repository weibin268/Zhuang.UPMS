using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
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

        public ContentResult GetDataGridPageData(int rows, int page)
        {
            ContentResult cr = new ContentResult();

            Dictionary<string, object> dicParam = new Dictionary<string, object>();

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("Filter_"))
                {
                    dicParam.Add(key.Replace("Filter_", ""), Request.Form[key]);
                }
            }

            DataGridUrlReturnModel model = new DataGridUrlReturnModel();

            int totalRowCount = 0;
            model.rows = _dba.PageQueryDataTable("SecuritySettings.User.List", "userid", page, rows, out totalRowCount, dicParam);
            model.total = totalRowCount;

            cr.Content = Newtonsoft.Json.JsonConvert.SerializeObject(model, new Newtonsoft.Json.Converters.DataTableConverter());

            return cr;

        }

    }
}