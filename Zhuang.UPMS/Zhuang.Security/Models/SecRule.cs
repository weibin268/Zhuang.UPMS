using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Security.Models
{
    public class SecRule
    {
        public string RuleId { get; set; }
        public string MenuId { get; set; }
        public string Name { get; set; }

        public int Status { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
