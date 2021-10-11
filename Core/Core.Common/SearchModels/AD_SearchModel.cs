using Admin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.SearchModels
{
    public class AD_SearchModel : SearchModel<T_AD>
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public bool? islock { get; set; }

        /// <summary>
        /// 广告类型
        /// </summary>
        public int? type { get; set; }

        public AD_SearchModel()
            : base("select * from T_AD where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Ad_Title like '%'+@key+'%' or F_Ad_Des like '%'+@key+'%') ");
            SqlBuilder.AppendIfHasValue(islock, " and F_Ad_IsLock=@islock ");
            SqlBuilder.AppendIfHasValue(type, " and F_Ad_Type=@type ");
        }

        public override string GetOrderBy()
        {
            return " order by F_Ad_Id desc";
        }
    }
}
