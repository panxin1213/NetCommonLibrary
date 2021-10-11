using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Firm.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class D_SEO
    {
        public class MetaData
        {
            [Display(Name = "编号")]
            public int F_SEO_Id { get; set; }

            [Display(Name = "SEO名称")]
            public string F_SEO_Name { get; set; }

            [Display(Name = "标题")]
            public string F_SEO_Title { get; set; }

            [Display(Name = "MetaKey")]
            public string F_SEO_MetaKey { get; set; }

            [Display(Name = "MetaContent")]
            public string F_SEO_MetaContent { get; set; }

            [Display(Name = "SEO链接地址")]
            public string F_SEO_Link { get; set; }

            [Display(Name = "显示状态")]
            public bool F_SEO_IsLock { get; set; }
        }
    }
}
