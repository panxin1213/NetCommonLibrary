using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;

namespace Firm.Core.Common.SearchModel
{
    public class Ad_View_Manage_SearchModel : SearchModel<D_Ad_View>
    {
        public string key { get; set; }

        public Ad_View_Manage_SearchModel()
            : base("select * from D_Ad_View where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_View_Title like '%'+@key+'%' or F_View_Address like '%'+@key+'%') ");
        }

        public override string GetOrderBy()
        {
            return " order by F_View_Id desc ";
        }
    }
}
