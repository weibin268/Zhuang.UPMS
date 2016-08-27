using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Security.Models
{
    public enum SecPermissionRefTable
    {
        Sec_Menu,
        Sec_Button,
        Sec_Rule
    }

    public class SecPermission
    {
        public string PermissionId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Seq { get; set; }
        public string Description { get; set; }
        public string RefTable { get; set; }
        public string RefRecordId { get; set; }

        public int Status { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
