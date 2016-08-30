using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhuang.Web.EasyUI.Models
{

    public class TreeModel
    {
        public class Attributes
        {
            public string url { get; set; }
        }

        public enum State
        {
            open,
            closed,
        }

        public string id { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public bool @checked { get; set; }
        public dynamic attributes { get; set; }
        public List<TreeModel> children { get; set; }


        public static List<TreeModel> ToTreeModel(List<TreeModel> lsRawModel)
        {
            List<TreeModel> lsResult = new List<TreeModel>();

            //得到所有一级节点
            lsResult = lsRawModel.FindAll(c =>
            {
                return !lsRawModel.Exists(cc => cc.id == c.parentId);
            });

            //定放递归函数
            Func<string, List<TreeModel>> funRecursive = null;
            funRecursive = (parentId) =>
            {
                var children = lsRawModel.FindAll(cc => { return cc.parentId == parentId; });

                //如是叶子节点
                if (children.Count == 0)
                {
                    lsRawModel.Find(c => c.id == parentId).state = State.open.ToString();
                }

                children.ForEach(c =>
                {
                    c.children = funRecursive(c.id);
                });

                return children;
            };

            //递归处理所有子孙节点
            lsResult.ForEach(c =>
            {
                c.children = funRecursive(c.id);
            });

            return lsResult;
        }
        
    }
}
