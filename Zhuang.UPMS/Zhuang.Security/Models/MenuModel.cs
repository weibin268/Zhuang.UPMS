using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhuang.Security.Models
{
    public class MenuModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? Seq { get; set; }
        public IList<MenuModel> Children { get; set; }

        public static List<MenuModel> ToTreeModel(List<MenuModel> lsRawModel)
        {
            List<MenuModel> lsResult = new List<MenuModel>();

            //得到所有一级节点
            lsResult = lsRawModel.FindAll(c =>
            {
                return !lsRawModel.Exists(cc => cc.Id == c.ParentId);
            });

            //定放递归函数
            Func<string, List<MenuModel>> funRecursive = null;
            funRecursive = (ParentId) =>
            {
                var Children = lsRawModel.FindAll(cc => { return cc.ParentId == ParentId; });

                //如是叶子节点
                if (Children.Count == 0)
                {

                }

                Children.ForEach(c =>
                {
                    c.Children = funRecursive(c.Id);
                });

                return Children;
            };

            //递归处理所有子孙节点
            lsResult.ForEach(c =>
            {
                c.Children = funRecursive(c.Id);
            });

            return lsResult;
        }
    }
}
