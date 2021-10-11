using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Admin.Model;

namespace Core.Common.Service
{
    public class SEO_Service : ServiceBase<T_SEO>
    {
        public SEO_Service(IDbConnection db)
            : base(db)
        {

        }

        #region 基础方法

        /// <summary>
        /// 返回SEO信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public T_SEO Get(string action, string controller, string area, string link)
        {
            var sql = string.Format("select top 1 * from T_SEO where (F_Seo_Action=@action and F_Seo_Controller=@controller {0}) or (F_Seo_Link=@link) order by (case when F_Seo_Area=@area and F_Seo_Action=@action and F_Seo_Controller=@controller then 2 when F_Seo_Link<>'' and F_Seo_Link is not null then 1 else 0 end) desc,len(F_Seo_Link) desc,F_Seo_Id desc "
                , String.IsNullOrEmpty(area) || area.Equals("www", StringComparison.OrdinalIgnoreCase) ? " and (F_Seo_Area is null or F_Seo_Area='' or F_Seo_Area='www')" : " and F_Seo_Area=@area ");

            return db.Query<T_SEO>(sql, new { action, controller, area, link }).SingleOrDefault();
        }


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

            var r = db.Execute("update T_SEO set F_Seo_IsLock=-(F_Seo_IsLock-1) where F_Seo_Id in (@ids)", new { ids });

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 删除SEO
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() < 1)
            {
                return 0;
            }

            var r = db.Execute("delete from T_SEO where F_Seo_Id in(" + ids.ToSqlSafeNums() + ")");

            if (r > 0)
            {
                base.RemoveCacheByThisType();
            }

            return r;
        }


        #endregion
    }
}
