using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_Admin
    {
        private void PartialStructure()
        {
            F_Admin_Create = DateTime.Now;
            F_Admin_IsLock = false;
            F_Admin_IsSupper = false;
        }

        public class MetaData
        {
            [Display(Name = "管理员ID")]
            public int F_Admin_Id { get; set; }

            [Display(Name = "管理员用户名")]
            [Remote("CheckName", "Admin", AdditionalFields = "F_Admin_Id", ErrorMessage = "用户名重复")]
            public string F_Admin_Name { get; set; }

            [Display(Name = "管理员姓名")]
            public string F_Admin_RealName { get; set; }

            [Display(Name = "管理员密码")]
            public string F_Admin_Password { get; set; }

            [Display(Name = "创建时间")]
            [Write(false)]
            public System.DateTime F_Admin_Create { get; set; }

            [Display(Name = "是否超管")]
            public bool F_Admin_IsSupper { get; set; }

            [Display(Name = "是否锁定")]
            [DataType("ManageIsLock")]
            public bool F_Admin_IsLock { get; set; }
        }
    }
}
