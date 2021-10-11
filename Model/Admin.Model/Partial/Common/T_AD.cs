using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_AD
    {
        public T_AD()
        {
            F_Ad_Create = DateTime.Now;
        }

        public class MetaData
        {
            [Display(Name = "广告id")]
            public int F_Ad_Id { get; set; }

            [Display(Name = "标题")]
            public string F_Ad_Title { get; set; }

            [Display(Name = "类型")]
            [EnumDataType(typeof(AdType))]
            public int F_Ad_Type { get; set; }

            [Display(Name = "链接地址")]
            public string F_Ad_Link { get; set; }

            [Display(Name = "图片")]
            public string F_Ad_Image { get; set; }

            [Display(Name = "描述")]
            public string F_Ad_Des { get; set; }

            [Display(Name = "html代码")]
            public string F_Ad_Html { get; set; }

            [Display(Name = "显示状态")]
            [DataType("ManageIsLock")]
            public bool F_Ad_IsLock { get; set; }

            [Display(Name = "创建时间")]
            public System.DateTime F_Ad_Create { get; set; }
        }
    }

    public enum AdType
    {
        [Description("文字广告")]
        文字广告 = 1,
        [Description("图片广告")]
        图片广告 = 2,
        [Description("Html代码广告")]
        Html代码广告 = 3
    }
}
