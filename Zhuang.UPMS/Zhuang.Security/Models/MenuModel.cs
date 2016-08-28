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
    }
}
