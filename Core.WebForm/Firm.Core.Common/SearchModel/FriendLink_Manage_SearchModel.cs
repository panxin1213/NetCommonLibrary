using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;

namespace Firm.Core.Common.SearchModel
{
    public class FriendLink_Manage_SearchModel : SearchModel<D_FriendLink>
    {
        public string key { get; set; }

        public string address { get; set; }

        public bool? islock { get; set; }

        public FriendLink_Manage_SearchModel()
            : base("select * from D_FriendLink where 1=1")
        {

        }

        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(key, " and (F_FriendLink_Title like '%'+@key+'%' or F_FriendLink_Url like '%'+@key+'%') ");
            SqlBuilder.AppendIfHasValue(address, " and (F_FriendLink_ByAddress=@address) ");
            SqlBuilder.AppendIfHasValue(islock, " and (F_FriendLink_IsLock=@islock) ");
        }

        public override string GetOrderBy()
        {
            return " order by F_FriendLink_Update desc,F_FriendLink_Id desc ";
        }
    }
}
