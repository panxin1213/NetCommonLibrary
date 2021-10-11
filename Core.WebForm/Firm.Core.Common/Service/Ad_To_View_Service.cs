using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firm.Model;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Firm.Core.Common.Service
{
    public class Ad_To_View_Service : ServiceBase<D_Ad_To_View>
    {
        public Ad_To_View_Service(IDbConnection db)
            : base(db)
        {

        }

        public int DeleteByViewId(int viewid, IDbTransaction trans = null)
        {
            var sql = "delete from D_Ad_To_View where F_M_To_V_View_ID=@viewid ";

            return db.Execute(sql, new { viewid }, trans);
        }


        public int InsertList(IEnumerable<D_Ad_To_View> l, IDbTransaction trans = null)
        {
            return db.InsertWithKeyField(l, trans);
        }
    }
}
