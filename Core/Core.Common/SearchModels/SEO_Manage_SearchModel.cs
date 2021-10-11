using Admin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.SearchModels
{
    public class SEO_Manage_SearchModel : SearchModel<T_SEO>
    {
        public string key { get; set; }
        /// <summary>
        /// 自动添加为Join后的安全字符
        /// </summary>

        public bool? islock { get; set; }

        public SEO_Manage_SearchModel() :
            base(@"Select *
                            From T_SEO  where 1=1")
        { }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Seo_Name like '%'+@key+'%' or F_Seo_Link like '%'+@key+'%' or charindex((F_Seo_Controller+(case when F_Seo_Action='index' then '' else '/'+F_Seo_Action end)),@key)>0)");
            SqlBuilder.AppendIfHasValue(islock, " and F_Seo_IsLock=@islock");
        }

        public override string GetOrderBy()
        {
            return "order by F_Seo_Update desc,F_Seo_Id desc";
        }

    }
}
