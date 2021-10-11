using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Firm.Core.Common.SearchModel
{
    public class SEO_SearchModel : SearchModel<D_SEO>
    {
        public string key { get; set; }

        public bool? islock { get; set; }

        public SEO_SearchModel()
            : base("select * from D_SEO where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_SEO_Name like '%'+@key+'%' or F_SEO_Title like '%'+@key+'%' or F_SEO_Link like '%'+@key+'%') ");


            SqlBuilder.AppendIfHasValue(islock, " and F_SEO_IsLock=@islock ");
        }


        public override string GetOrderBy()
        {
            return " order by F_SEO_Id desc ";
        }
    }
}
