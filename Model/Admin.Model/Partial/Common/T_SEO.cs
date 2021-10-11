using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_SEO
    {
        public T_SEO()
        {
            F_Seo_Create = DateTime.Now;
            F_Seo_Update = DateTime.Now;
        }

        public class MetaData
        {
            [Display(Name = "编号")]
            public int F_Seo_Id { get; set; }

            [Display(Name = "SEO名称")]
            public string F_Seo_Name { get; set; }

            [Display(Name = "MetaKey")]
            public string F_Seo_MetaKey { get; set; }

            [Display(Name = "MetaContent")]
            public string F_Seo_MetaContent { get; set; }

            [Display(Name = "创建时间")]
            public DateTime F_Seo_Create { get; set; }

            [Display(Name = "更新时间")]
            public DateTime F_Seo_Update { get; set; }

            [Display(Name = "锁定")]
            [DataType("ManageIsLock")]
            public bool F_Seo_IsLock { get; set; }

            [Display(Name = "SEO标题")]
            public string F_Seo_Title { get; set; }

            [Display(Name = "页面Action")]
            public string F_Seo_Action { get; set; }

            [Display(Name = "页面Controller")]
            public string F_Seo_Controller { get; set; }

            [Display(Name = "SEO类型")]
            [EnumDataType(typeof(SEOType))]
            public int F_Seo_Type { get; set; }

            [Display(Name = "搜索类型SEO标题")]
            public string F_Seo_Search_Title { get; set; }

            [Display(Name = "搜索类型MetaKey")]
            public string F_Seo_Search_MetaKey { get; set; }

            [Display(Name = "搜索类型MetaContent")]
            public string F_Seo_Search_MetaContent { get; set; }

            [Display(Name = "页面区域")]
            public string F_Seo_Area { get; set; }

            [Display(Name = "页面地址")]
            public string F_Seo_Link { get; set; }
        }
    }


    public enum SEOType
    {
        [Description("单页")]
        单页 = 1,
        [Description("搜索页")]
        搜索页 = 2
    }
}
