using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Firm.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class D_Admin
    {
        private void PartialStructure()
        {
            F_Admin_Time_Create = DateTime.Now;
            F_Admin_Time_Last = DateTime.Now;
        }

        public class MetaData
        {
            [Display(Name="管理员编号")]
            public int F_Admin_Id { get; set; }

            [Display(Name = "用户名")]
            [Remote(AdditionalFields = "F_Admin_Name,F_Admin_Id", HttpMethod = "post", ErrorMessage = "该用户名已存在", FullClassName = "Firm.Web.manage.Admin.UserNameValid", Url = "/Manage/Admin/UserNameValid.ashx")]
            public string F_Admin_Name { get; set; }

            [Display(Name = "姓名")]
            public string F_Admin_Nick { get; set; }

            [Display(Name = "密码")]
            public string F_Admin_Password { get; set; }

            [Display(Name = "最后登录时间")]
            public System.DateTime F_Admin_Time_Last { get; set; }

            [Display(Name = "创建时间")]
            public System.DateTime F_Admin_Time_Create { get; set; }

            [Display(Name = "锁定")]
            public bool F_Admin_IsLock { get; set; }

            [Display(Name = "超管")]
            public bool F_Admin_IsSupper { get; set; }
        }
    }
}
