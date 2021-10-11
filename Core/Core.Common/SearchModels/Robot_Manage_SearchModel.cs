using Admin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.SearchModels
{
    public class Robot_Manage_SearchModel : SearchModel<T_Robot>
    {
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string key { get; set; }

        public string type { get; set; }

        public Robot_Manage_SearchModel()
            : base("select * from T_Robot where 1=1 ")
        {

        }


        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Robot_Name like '%'+@key+'%' or F_Robot_Link like '%'+@key+'%' ) ");

            SqlBuilder.AppendIfHasValue(type, " and F_Robot_Type=@type ");
        }

        public override string GetOrderBy()
        {
            return " order by F_Robot_Id desc ";
        }
    }
}
