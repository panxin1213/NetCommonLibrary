using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Common.Library;
using System.ComponentModel;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_FriendLink
    {
        public T_FriendLink()
        {
            F_FriendLink_Create = DateTime.Now;
            F_FriendLink_Order = 10000;
            F_FriendLink_Update = DateTime.Now;
            F_FriendLink_Type = FriendLinkType.Text.ToInt();
        }

        public class MetaData
        {
            [Display(Name = "编号")]
            public int F_FriendLink_Id { get; set; }

            [Display(Name = "标题")]
            public string F_FriendLink_Title { get; set; }

            [Display(Name = "连接地址")]
            [RegularExpression(Constants.REGEX_URL_ADDRESS, ErrorMessage = "连接地址错误，格式:http://www.nyjm168.com/")]
            public string F_FriendLink_Url { get; set; }

            [Display(Name = "友链类型")]
            [EnumDataType(typeof(FriendLinkType))]
            public int F_FriendLink_Type { get; set; }

            [Display(Name = "图片")]
            public string F_FriendLink_Image { get; set; }

            [Display(Name = "显示状态")]
            [DataType("ManageIsLock")]
            public bool F_FriendLink_IsLock { get; set; }

            [Display(Name = "创建时间")]
            public System.DateTime F_FriendLink_Create { get; set; }

            [Display(Name = "更新时间")]
            public System.DateTime F_FriendLink_Update { get; set; }

            [Display(Name = "排序数字")]
            public int F_FriendLink_Order { get; set; }

            [Display(Name = "所属页面")]
            [DataType("FriendLinkAddress")]
            public string F_FriendLink_ByAddress { get; set; }

            [Display(Name = "备注")]
            public string F_FriendLink_Content { get; set; }
        }
    }


    public enum FriendLinkType
    {
        [Description("文字")]
        Text = 1,
        [Description("图片")]
        Image = 2
    }
}
