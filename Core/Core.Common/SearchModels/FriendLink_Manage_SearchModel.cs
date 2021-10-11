using Admin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.SearchModels
{
    public class FriendLink_Manage_SearchModel : SearchModel<T_FriendLink>
    {
        /// <summary>
        /// 搜索词
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public bool? islock { get; set; }

        public FriendLink_Manage_SearchModel()
            : base(@"select * from T_FriendLink where 1=1 ")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_FriendLink_Title like '%'+@key+'%' or F_FriendLink_Url like '%'+@key+'%' or F_FriendLink_ByAddress like '%'+@key+'%') ");
            SqlBuilder.AppendIfHasValue(islock, " and F_FriendLink_IsLock=@islock ");
        }


        public override string GetOrderBy()
        {
            return " order by F_FriendLink_Update desc,F_FriendLink_Id desc";
        }
    }
}
