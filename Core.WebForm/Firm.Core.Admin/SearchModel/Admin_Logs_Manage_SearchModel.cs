using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Firm.Core.Admin.SearchModel
{
    public class Admin_Logs_Manage_SearchModel : SearchModel<D_Admin_Logs>
    {
        public string key { get; set; }

        public int? id { get; set; }

        public Admin_Logs_Manage_SearchModel()
            : base("select * from D_Admin_Logs where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_A_Log_IP like '%'+@key+'%' or F_A_Log_UserAgent like '%'+@key+'%') ");

            SqlBuilder.AppendIfHasValue(id, " and F_A_Log_Admin_Id=@id ");
        }

        public override string GetOrderBy()
        {
            return " order by F_A_Log_Create desc,F_A_Log_Admin_Id desc ";
        }
    }
}
