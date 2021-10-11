using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using ChinaBM.Common;

namespace Firm.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class D_FriendLink
    {
        [Display(Name = "地址列表")]
        [NotMapped]
        [Dapper.Contrib.Extensions.Write(false)]
        public List<string> All_FriendLink_Address
        {
            get
            {
                try
                {
                    var d = XElement.Load(HttpKit.GetMapPath("/manage/friendlink/FriendLinkByAddress.config"));
                    var n = d.Descendants("address").ToList().Select(a => a.Value).ToList();
                    return n;
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public D_FriendLink()
        {
            F_FriendLink_Type = 1;
            F_FriendLink_IsLock = false;
            F_FriendLink_Create = DateTime.Now;
            F_FriendLink_Update = DateTime.Now;
            F_FriendLink_Order = 10000;
        }

        public class MetaData
        {
            [Display(Name="友情链接ID")]
            public int F_FriendLink_Id { get; set; }

            [Display(Name = "标题")]
            public string F_FriendLink_Title { get; set; }

            [Display(Name = "链接地址")]
            [RegularExpression(@"^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", ErrorMessage = "请输入正确的网址,请带http://")]
            public string F_FriendLink_Url { get; set; }

            [Display(Name = "类  型")]
            [EnumDataType(typeof(FriendLinkType))]
            public byte F_FriendLink_Type { get; set; }

            [Display(Name = "图  片")]
            public string F_FriendLink_Image { get; set; }

            [Display(Name = "锁定")]
            public bool F_FriendLink_IsLock { get; set; }

            [Display(Name = "创建时间")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> F_FriendLink_Create { get; set; }

            [Display(Name = "修改时间")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> F_FriendLink_Update { get; set; }

            [Display(Name = "排序号")]
            [Range(0, 1000000)]
            public int F_FriendLink_Order { get; set; }

            [Display(Name = "展示地址")]
            public string F_FriendLink_ByAddress { get; set; }
        }
    }

    public enum FriendLinkType
    {
        [Description(@"文字连接")]
        文字连接 = 1,
        [Description(@"图片连接")]
        图片连接 = 2
    }
}
