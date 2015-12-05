using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zhuang.Data;
using Zhuang.Web.EasyUI.Models;

namespace Zhuang.UPMS.WebMvc.App_Code
{
    public static class EasyUIHelper
    {
        public static ContentResult GetDataGridPageData()
        {
            string strSql = HttpContext.Current.Request.Form["sql"];
            if (strSql.Trim().Contains(" "))
            {
                throw new Exception("参数“sql”格式错误！");
            }
            string strOrderBy = HttpContext.Current.Request.Form["orderby"];
            int page = Convert.ToInt32(HttpContext.Current.Request.Form["page"]);
            int rows = Convert.ToInt32(HttpContext.Current.Request.Form["rows"]);

            DbAccessor dba = DbAccessor.Get();

            ContentResult cr = new ContentResult();

            Dictionary<string, object> dicParam = new Dictionary<string, object>();

            foreach (string key in HttpContext.Current.Request.Form.Keys)
            {
                if (key.StartsWith("Filter_"))
                {
                    dicParam.Add(key.Replace("Filter_", ""), HttpContext.Current.Request.Form[key]);
                }
            }

            DataGridUrlReturnModel model = new DataGridUrlReturnModel();

            int totalRowCount = 0;
            model.rows = dba.PageQueryDataTable(strSql, strOrderBy, page, rows, out totalRowCount, dicParam);
            model.total = totalRowCount;

            cr.Content = Newtonsoft.Json.JsonConvert.SerializeObject(model, new Newtonsoft.Json.Converters.DataTableConverter());

            return cr;

        }
    }
}