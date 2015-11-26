using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhuang.Web.ZTree.Models
{
    public class TreeNodeModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<TreeNodeModel> children { get; set; }
        public bool @checked { get; set; }
        public bool open { get; set; }
        public string url { get; set; }

    }
}
