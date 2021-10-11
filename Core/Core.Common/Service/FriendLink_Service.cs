using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Common.Library.Caching;
using Admin.Model;

namespace Core.Common.Service
{
    public class FriendLink_Service : ServiceBase<T_FriendLink>
    {
        public FriendLink_Service(IDbConnection db)
            : base(db)
        {

        }

        #region 基础操作

        /// <summary>
        /// 改变信息显示状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int ChangeLock(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var r = db.Execute("update T_FriendLink set F_FriendLink_IsLock=-(F_FriendLink_IsLock-1) where F_FriendLink_Id in (@ids)", new { ids });

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() < 1)
            {
                return 0;
            }

            var r = db.Execute("delete from T_FriendLink where F_FriendLink_Id in(" + ids.ToSqlSafeNums() + ")");

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }

        #endregion


        #region 显示操作方法



        private readonly static string CommonWhere = " and F_FriendLink_IsLock=0 ";



        /// <summary>
        /// 返回对象集合
        /// </summary>
        /// <param name="top">条数</param>
        /// <param name="byaddress">显示地址</param>
        /// <param name="order">排序字符串</param>
        /// <returns></returns>
        public List<T_FriendLink> GetList(int top = 0, string byaddress = null, string order = " order by F_FriendLink_Order asc,F_FriendLink_Update desc,F_FriendLink_Id desc ")
        {
            return CacheManager.Get(CacheKey.GetCacheKey<T_FriendLink>(
                       "getlist",
                       top.ToString(),
                       byaddress.ToSafeString(),
                       order
                       ), () =>
                       {
                           var sql = string.Format("select {0} * from T_FriendLink a where 1=1 {1} {2} {3} "
                               , top > 0 ? " top " + top : ""
                               , CommonWhere
                               , String.IsNullOrWhiteSpace(byaddress) ? " and (F_FriendLink_ByAddress='' or F_FriendLink_ByAddress is null) " : " and (F_FriendLink_ByAddress=@byaddress or F_FriendLink_ByAddress='' or F_FriendLink_ByAddress is null) "
                               , order);


                           return db.Query<T_FriendLink>(sql, new { byaddress }).ToList();
                       }) ?? new List<T_FriendLink>();
        }



        #endregion
    }
}
