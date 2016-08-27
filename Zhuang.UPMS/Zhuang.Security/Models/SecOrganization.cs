using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Security.Models
{
    public class SecOrganization
    {
        public string OrganizationId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public int Level { get; set; }
        public int? Seq { get; set; }

        public int Status { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
