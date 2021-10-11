using Admin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Admin.SearchModels
{
    public class Admin_Role_SearchModel : SearchModel<T_Admin_Role>
    {
        public string key { get; set; }

        public Admin_Role_SearchModel() :
            base(@"Select * from T_Admin_Role where 1=1 ")
        { }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and F_Role_Name like '%'+@key+'%'");
        }
        public override string GetOrderBy()
        {
            return "order by F_Role_Id desc";
        }

    }
}
