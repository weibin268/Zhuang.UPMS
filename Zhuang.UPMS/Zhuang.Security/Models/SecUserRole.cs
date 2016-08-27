using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Security.Models
{
    public class SecUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public int Status { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
