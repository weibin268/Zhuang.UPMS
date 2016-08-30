using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhuang.Security.Models
{
    public class ElementModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public IList<RuleModel> RuleList { get; set; }
    }
}
