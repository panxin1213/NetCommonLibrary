using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;

namespace Firm.Core.Admin.SearchModel
{
    public class Admin_Role_Manage_SearchModel : SearchModel<D_Admin_Role>
    {
        public string key { get; set; }

        public string order { get; set; }

        public bool? islock { get; set; }

        public Admin_Role_Manage_SearchModel()
            : base("select * from D_Admin_Role where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Role_Name like '%'+@key+'%') ");

            SqlBuilder.AppendIfHasValue(islock, " and (F_Role_IsLock=@islock) ");
        }

        public override string GetOrderBy()
        {
            return " order by F_Role_Id desc";
        }
    }
}
