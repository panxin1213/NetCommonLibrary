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
    public class Robot_Service : ServiceBase<T_Robot>
    {
        public Robot_Service(IDbConnection db)
            : base(db)
        {

        }



        #region 基础方法


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

            var r = db.Execute("update T_Robot set F_Robot_IsLock=-(F_Robot_IsLock-1) where F_Robot_Id in (@ids)", new { ids });

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() < 1)
            {
                return 0;
            }

            var r = db.Execute("delete from T_Robot where F_Robot_Id in(" + ids.ToSqlSafeNums() + ")");

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }


        #endregion



        #region 显示操作方法

        /// <summary>
        /// 返回类型关键词与链接字典
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDicByType(string type, bool ispc = true)
        {
            return CacheManager.Get(CacheKey.GetCacheKey<T_Robot>(
                       "getdicbytype",
                       type.ToSafeString(),
                       ispc.ToSafeString()), () =>
                       {
                           var sql = string.Format("select * from T_Robot where F_Robot_IsLock=0 {0} order by len(F_Robot_Name) desc,F_Robot_Id desc"
                               , !String.IsNullOrWhiteSpace(type) ? " and (F_Robot_Type=@type or F_Robot_Type='' or F_Robot_Type is null) " : "");

                           return db.Query<T_Robot>(sql, new { type }).GroupBy(a => a.F_Robot_Name).ToDictionary(a => a.Key, a => 
                               {
                                   var m = a.OrderByDescending(b => b.F_Robot_Id).FirstOrDefault();
                                   if (!ispc && String.IsNullOrWhiteSpace(m.F_Robot_MobileLink))
                                   {
                                       m.F_Robot_MobileLink = m.F_Robot_Link;
                                   }
                                   return ispc ? m.F_Robot_Link : m.F_Robot_MobileLink;
                               });
                       }) ?? new Dictionary<string, string>();

        }


        #endregion
    }
}
