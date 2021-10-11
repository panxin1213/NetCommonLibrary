using Admin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Admin.SearchModels
{
    public class Admin_SearchModel : SearchModel<T_Admin, T_Admin_Log, T_Admin>
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string key { get; set; }

        public Admin_SearchModel()
            : base("select * from T_Admin a outer apply (select top 1 * from T_Admin_Log where F_A_L_AdminId=a.F_Admin_Id order by F_A_L_LoginTime desc) b where 1=1 ", "F_A_L_AdminId", (a, b) => 
            {
                b = b ?? new T_Admin_Log { F_A_L_AdminId = a.F_Admin_Id, F_A_L_Ip = "", F_A_L_LoginTime = a.F_Admin_Create };
                a.T_Admin_Log = new List<T_Admin_Log> { b };
                return a;
            })
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_Admin_Name like '%'+@key+'%' or F_Admin_RealName like '%'+@key+'%') ");
        }

        public override string GetOrderBy()
        {
            return " order by F_Admin_Id desc ";
        }
    }
}
