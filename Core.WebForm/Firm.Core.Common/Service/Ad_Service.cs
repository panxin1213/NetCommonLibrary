using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;
using Dapper;
using Dapper.Contrib.Extensions;
using Common.Library;

namespace Firm.Core.Common.Service
{
    public class Ad_Service : ServiceBase<D_Ad>
    {
        public Ad_Service(IDbConnection db)
            : base(db)
        {

        }

        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "delete from D_Ad where F_Ad_Id in({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }


        public List<D_Ad> GetList(int top = 0, bool? islock = null, IEnumerable<int> ids = null, IEnumerable<int> exids = null, string order = " order by F_Ad_Id desc")
        {
            var sql = string.Format("select {0} * from D_Ad where 1=1 {1} {2} {4} {3}", top > 0 ? " top " + top : "", islock != null ? " and F_Ad_IsLock=@islock " : "", ids != null && ids.Count() > 0 ? " and F_Ad_Id in(" + ids.ToSqlSafeNums() + ") " : "", order, exids != null && exids.Count() > 0 ? " and F_Ad_Id not in(" + exids.ToSqlSafeNums() + ") " : "");

            return db.Query<D_Ad>(sql, new { islock }).ToList();
        }

        public static string GetAdString(int id)
        {
            using (IDbConnection db = Conn.Get())
            {
                var asv = new Ad_Service(db);
                var m = asv.Get(id);
                db.Close();
                db.Dispose();
                if (m == null)
                {
                    return "没有该广告";
                }

                if (m.F_Ad_Type == Firm.Model.D_Ad.ADType.文字广告.ToInt())
                {
                    return string.Format("<a href=\"{0}\" title=\"{1}\" target=\"_blank\">{1}</a>", m.F_Ad_Link, m.F_Ad_Desc);
                }
                else if (m.F_Ad_Type == Firm.Model.D_Ad.ADType.图片广告.ToInt())
                {
                    return string.Format("<a href=\"{0}\" title=\"{1}\" target=\"_blank\"><img src=\"{2}\" alt=\"{1}\"/></a>", m.F_Ad_Link, m.F_Ad_Desc, m.F_Ad_Image);
                }
                else
                {
                    return m.F_Ad_Html;
                }
            }
        }
    }
}
