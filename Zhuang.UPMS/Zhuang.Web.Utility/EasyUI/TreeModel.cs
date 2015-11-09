using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhuang.Web.Utility.EasyUI
{
    public class TreeModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public object attributes { get; set; }
    }
}
