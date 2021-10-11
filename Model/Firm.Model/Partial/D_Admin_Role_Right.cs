using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Firm.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class D_Admin_Role_Right
    {
        [Dapper.Contrib.Extensions.Write(false)]
        [NotMapped]
        public bool Checked { get; set; }

        public class MetaData
        {
            [Display(Name="角色ID")]
            public int F_Admin_Right_RoleId { get; set; }

            [Display(Name="命名空间")]
            public string NameSpace { get; set; }

            [Display(Name = "类名")]
            public string ClassName { get; set; }
        }
    }
}
