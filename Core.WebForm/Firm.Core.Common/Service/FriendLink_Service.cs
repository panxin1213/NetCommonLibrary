using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Firm.Core.Common.Service
{
    public class FriendLink_Service : ServiceBase<D_FriendLink>
    {
        public FriendLink_Service(IDbConnection db)
            : base(db)
        {

        }


        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "delete from D_FriendLink where F_FriendLink_Id in({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }

        public List<D_FriendLink> GetList(int top = 0, bool? islock = null, string order = " order by F_FriendLink_Order asc,F_FriendLink_Update desc,F_FriendLink_Id desc ")
        {
            var sql = "select {0} * from D_FriendLink where 1=1 {1} {2}";

            sql = string.Format(sql, top > 0 ? " top " + top : "", islock != null ? " and F_FriendLink_IsLock=@islock  " : "", order);

            return db.Query<D_FriendLink>(sql, new { islock }).ToList();
        }

        public int IsLock(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "update D_FriendLink set F_FriendLink_IsLock=-(F_FriendLink_IsLock-1) where F_FriendLink_Id in ({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }
    }
}
