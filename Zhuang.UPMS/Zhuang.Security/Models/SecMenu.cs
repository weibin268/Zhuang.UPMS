using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Security.Models
{
    [Serializable]
    [Zhuang.Data.Annotations.Table("Sec_Menu")]
    public class SecMenu
    {
        [Data.Annotations.Key]
        public string MenuId { get; set; }
        public string ParentId { get; set; }
        public string PermissionId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? Seq { get; set; }
        public bool IsExpand { get; set; }

        public int Status { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
