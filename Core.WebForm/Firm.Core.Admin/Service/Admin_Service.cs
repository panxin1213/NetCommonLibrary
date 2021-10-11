using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;
using Dapper;
using Dapper.Contrib.Extensions;
using Common.Library.Log;

namespace Firm.Core.Admin.Service
{
    public class Admin_Service : ServiceBase<D_Admin>
    {
        public Admin_Service(IDbConnection db)
            : base(db)
        {

        }


        public override long Insert(D_Admin entity)
        {
            var trans = db.BeginTransaction();

            try
            {
                entity.F_Admin_Password = entity.F_Admin_Password.ToPassword();

                var r = db.Insert(entity, trans);

                if (r > 0)
                {
                    entity.D_Admin_Role = entity.D_Admin_Role.Where(a => a.Checked).ToList();
                    if (entity.D_Admin_Role.Count > 0)
                    {
                        db.InsertWithKeyField(entity.D_Admin_Role.Select(a => new D_Admin_To_Role { F_Admin_Role_AdminId = r.ToInt(), F_Admin_Role_RoleId = a.F_Role_Id }), trans);
                    }
                }
                if (r > 0)
                {
                    trans.Commit();

                    return r;
                }

            }
            catch(Exception e)
            {
                Logger.Error(this, e.Message, e);
            }
            trans.Rollback();
            return 0;
        }

        public D_Admin Get(string name)
        {
            var sql = "select * from D_Admin where F_Admin_Name=@name ";

            return db.Query<D_Admin>(sql, new { name }).SingleOrDefault();
        }

        public D_Admin GetFull(int id)
        {
            var sql = "select a.*,c.* from D_Admin a left join D_Admin_To_Role b on a.F_Admin_Id=b.F_Admin_Role_AdminId left join D_Admin_Role c on b.F_Admin_Role_RoleId=c.F_Role_Id where a.F_Admin_Id=@id ";

            var m = db.QueryMany<D_Admin, D_Admin_Role>(sql, a => a.D_Admin_Role, a => a.F_Admin_Id, new { id }, splitOn: "F_Role_Id").SingleOrDefault();

            if (m.D_Admin_Role != null)
            {
                m.D_Admin_Role = m.D_Admin_Role.Where(a => a != null).ToList();
            }

            return m;
        }

        public override bool Update(D_Admin entity)
        {
            var om = Get(entity.F_Admin_Id);

            var trans = db.BeginTransaction();

            try
            {
                

                if (om.F_Admin_Password != entity.F_Admin_Password)
                {
                    entity.F_Admin_Password = entity.F_Admin_Password.ToPassword();
                }

                var r = db.Update(entity, trans);

                if (r)
                {
                    db.Execute("delete from D_Admin_To_Role where F_Admin_Role_AdminId=@id", new { id = entity.F_Admin_Id }, trans);

                    entity.D_Admin_Role = entity.D_Admin_Role.Where(a => a.Checked).ToList();
                    if (entity.D_Admin_Role.Count > 0)
                    {
                        db.InsertWithKeyField(entity.D_Admin_Role.Select(a => new D_Admin_To_Role { F_Admin_Role_AdminId = entity.F_Admin_Id, F_Admin_Role_RoleId = a.F_Role_Id }), trans);
                    }
                }

                if (r)
                {
                    trans.Commit();
                    return r;
                }
            }
            catch(Exception e)
            {
                Logger.Error(this, e.Message, e);
            }
            trans.Rollback();
            return false;
        }

        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "delete from D_Admin where F_Admin_Id in({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }

        /// <summary>
        /// 更新最后登录时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool UpdateLastJoinTime(int id, IDbTransaction trans = null)
        {
            var sql = "update D_Admin set F_Admin_Time_Last=getdate() where F_Admin_Id=@id";

            return db.Execute(sql, new { id }, trans) > 0;
        }
    }
}
