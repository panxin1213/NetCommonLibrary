using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using Common.Library;

namespace Firm.Core.Admin.Service
{

    public class Admin_Record_Service : ServiceBase<D_Admin_Record>
    {
        public Admin_Record_Service(IDbConnection db)
            : base(db)
        {

        }

        public D_Admin_Record Get(int id, IDbTransaction trans = null)
        {
            var sql = @"select * from D_Admin_Record a inner join D_Admin b on a.F_A_Record_AdminId=b.F_Admin_Id
                    where F_A_Record_Id=@id";

            var m = db.Query<D_Admin_Record, D_Admin, D_Admin_Record>(sql, (a, b) =>
            {
                a.D_Admin = b;
                return a;
            }, new { id = (int)id }, splitOn: "F_Admin_Id", transaction: trans).SingleOrDefault();

            return m;
        }

        /// <summary>
        /// 插入操作记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="m"></param>
        /// <param name="oldm"></param>
        /// <returns></returns>
        public static bool Insert(RecordType type, object m, int adminId, string tablename, object oldm = null)
        {
            using (IDbConnection db = Conn.GetByKey("Common"))
            {
                var rm = new D_Admin_Record();
                rm.F_A_Record_AdminId = adminId;
                rm.F_A_Record_Create = DateTime.Now;
                rm.F_A_Record_NewContent = m != null ? m.ToJson() : "";
                rm.F_A_Record_OldContent = oldm != null ? oldm.ToJson() : "";
                rm.F_A_Record_TableName = tablename;
                rm.F_A_Record_Type = type.ToSafeString();

                return db.Insert(rm) > 0;
            }
        }
    }
}
