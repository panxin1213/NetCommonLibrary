using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_Admin_Role
    {
        [Write(false)]
        public bool Checked { get; set; }

        private void PartialStructure()
        {
            F_Role_Create = DateTime.Now;
        }


        public class MetaData
        {
            [Display(Name = "角色ID")]
            public int F_Role_Id { get; set; }

            [Display(Name = "角色名称")]
            public string F_Role_Name { get; set; }

            [Display(Name = "角色备注")]
            public string F_Role_Description { get; set; }

            [Display(Name = "创建时间")]
            public System.DateTime F_Role_Create { get; set; }

            [Display(Name = "是否锁定")]
            [DataType("ManageIsLock")]
            public bool F_Role_IsLock { get; set; }
        }
    }
}
