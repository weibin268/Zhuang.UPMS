using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhuang.Web.Utility.EasyUI
{
    public enum TreeStateType
    {
        open,
        closed,
    }

    public class TreeModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public bool @checked { get; set; }
        public object attributes { get; set; }
    }
}
