using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;

namespace Firm.Core.Admin.SearchModel
{
    public class Admin_Manage_SearchModel : SearchModel<D_Admin>
    {
        public string key { get; set; }

        public string order { get; set; }

        public bool? islock { get; set; }

        public bool? issupper { get; set; }

        public Admin_Manage_SearchModel()
            : base("select * from D_Admin where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Admin_Name like '%'+@key+'%' or F_Admin_Nick like '%'+@key+'%') ");

            SqlBuilder.AppendIfHasValue(islock, " and F_Admin_IsLock=@islock ");

            SqlBuilder.AppendIfHasValue(issupper, " and F_Admin_IsSupper=@issupper ");
        }

        public override string GetOrderBy()
        {
            return " order by F_Admin_Id desc ";
        }
    }
}
