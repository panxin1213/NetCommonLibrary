using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Firm.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class D_Admin_Role
    {
        [Dapper.Contrib.Extensions.Write(false)]
        [NotMapped]
        public bool Checked { get; set; }

        private void PartialStructure()
        {
            F_Role_Time_Create = DateTime.Now;
        }

        public class MetaData
        {
            [Display(Name = "角色编号")]
            public int F_Role_Id { get; set; }

            [Display(Name = "角色名称")]
            public string F_Role_Name { get; set; }

            [Display(Name = "备注")]
            public string F_Role_Description { get; set; }

            [Display(Name = "创建时间")]
            public System.DateTime F_Role_Time_Create { get; set; }

            [Display(Name = "锁定")]
            public bool F_Role_IsLock { get; set; }
        }
    }
}
