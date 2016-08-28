using System;
using System.Collections.Generic;
using System.Data;
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


        public ContentResult GetTree()
        {
            ContentResult contentResult = new ContentResult();

            string strSql = Request.QueryString["sql"];
            if (strSql.Trim().Contains(" "))
            {
                throw new Exception("参数“sql”格式错误！");
            }

            var dtTree = _dba.QueryDataTable(strSql);

            List<TreeModel> lsTree = new List<TreeModel>();

            foreach (DataRow row in dtTree.Rows)
            {
                lsTree.Add(new TreeModel()
                {
                    id = row["id"].ToString(),
                    parentId = row["parentId"].ToString(),
                    text = row["text"].ToString(),
                    state = TreeModel.State.open.ToString()
                });
            }

            contentResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(TreeModel.ToTreeUrlReturnModel(lsTree));
            return contentResult;
        }


    }
}