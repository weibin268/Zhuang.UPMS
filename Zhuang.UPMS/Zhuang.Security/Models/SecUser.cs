using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Security.Models
{
    [Serializable]
    [Data.Annotations.Table("Sec_User")]
    public class SecUser
    {
        [Data.Annotations.Key]
        public string UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string MobilePhone { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public string OrganizationId { get; set; }

        public int Status { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
