using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace Firm.Core.Common.Service
{
    public class SEO_Service : ServiceBase<D_SEO>
    {
        public SEO_Service(IDbConnection db)
            : base(db)
        {

        }



        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "delete from D_SEO where F_SEO_Id in({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }

        public int IsLock(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "update D_SEO set F_SEO_IsLock=-(F_SEO_IsLock-1) where F_SEO_Id in ({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }


        public D_SEO GetModelByUrl(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            var sql = "select top 1 * from D_SEO where F_SEO_IsLock=0 and F_SEO_Link like '%'+@url+'%' order by len(F_SEO_Link) asc ";

            return db.Query<D_SEO>(sql, new { url }).FirstOrDefault();
        }
    }
}
