using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Firm.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class D_Ad
    {
        public class MetaData
        {
            [Display(Name = "广告ID")]
            public int F_Ad_Id { get; set; }

            [Display(Name = "名称")]
            public string F_Ad_Title { get; set; }

            [Display(Name = "类型")]
            [EnumDataType(typeof(ADType))]
            public int F_Ad_Type { get; set; }

            [Display(Name = "地址")]
            [RegularExpression(@"^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", ErrorMessage = "请输入正确的网址,请带http://")]
            public string F_Ad_Link { get; set; }

            [Display(Name = "图片")]
            public string F_Ad_Image { get; set; }

            [Display(Name = "描述")]
            public string F_Ad_Desc { get; set; }

            [Display(Name = "Html代码")]
            public string F_Ad_Html { get; set; }

            [Display(Name = "锁定")]
            public bool F_Ad_IsLock { get; set; }

            [Display(Name = "创建时间")]
            public System.DateTime F_Ad_Create { get; set; }
        }

        public enum ADType
        {
            [Description("文字广告")]
            文字广告 = 1,
            [Description("图片广告")]
            图片广告 = 2,
            [Description("Html广告")]
            Html广告 = 3
        }
    }
}
