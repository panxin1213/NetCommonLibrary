using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;

namespace Firm.Core.Common.SearchModel
{
    public class Ad_Manage_SearchModel : SearchModel<D_Ad>
    {
        public string key { get; set; }

        public Ad_Manage_SearchModel()
            : base("select * from D_Ad where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Ad_Title like '%'+@key+'%' or F_Ad_Link like '%'+@key+'%') ");
        }

        public override string GetOrderBy()
        {
            return " order by F_Ad_Id desc ";
        }
    }
}
