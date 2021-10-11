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
    public class Ad_View_Service : ServiceBase<D_Ad_View>
    {
        public Ad_View_Service(IDbConnection db)
            : base(db)
        {

        }

        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "delete from D_Ad_View where F_View_Id in({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }

        public override bool Update(D_Ad_View entity)
        {
            var trans = db.BeginTransaction();

            try
            {
                var r = db.Update(entity, trans);

                var _atvsv = new Ad_To_View_Service(db);
                _atvsv.DeleteByViewId(entity.F_View_Id, trans);

                if (r && entity.D_Ad_To_View.Count > 0)
                {
                    entity.D_Ad_To_View = entity.D_Ad_To_View.Select(a => { a.F_M_To_V_View_ID = entity.F_View_Id; return a; }).ToList();

                    r = _atvsv.InsertList(entity.D_Ad_To_View, trans) > 0;
                }

                if (r)
                {
                    trans.Commit();
                    return true;
                }
            }
            catch
            {

            }
            trans.Rollback();
            return false;
        }

        public override D_Ad_View Get(dynamic id)
        {
            var sql = "select * from D_Ad_View a inner join D_Ad_To_View b on a.F_View_Id=b.F_M_To_V_View_ID where F_View_Id=@id";

            return db.QueryMany<D_Ad_View, D_Ad_To_View>(sql, a => a.D_Ad_To_View, a => a.F_View_Id, new { id = (int)id }, splitOn: "F_M_To_V_View_ID").SingleOrDefault();
        }
    }
}
