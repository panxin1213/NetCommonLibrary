using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Firm.Core.Admin.Service
{
    public class Admin_Role_Service : ServiceBase<D_Admin_Role>
    {
        private Admin_Role_Right_Service _arrsv;

        public Admin_Role_Service(IDbConnection db)
            : base(db)
        {
            _arrsv = new Admin_Role_Right_Service(db);
        }

        public override long Insert(D_Admin_Role entity)
        {
            var trans = db.BeginTransaction();

            try
            {
                var r = db.Insert(entity, trans);

                if (r > 0)
                {
                    entity.D_Admin_Role_Right = entity.D_Admin_Role_Right.Where(a => a.Checked).Select(a => { a.F_Admin_Right_RoleId = r.ToInt(); return a; }).ToList();
                    _arrsv.Insert(entity.D_Admin_Role_Right, trans);
                    trans.Commit();
                    return r;
                }
                
            }
            catch
            {

            }

            trans.Rollback();
            return 0;
        }

        public override bool Update(D_Admin_Role entity)
        {
            var trans = db.BeginTransaction();
            try
            {
                var r = db.Update(entity, trans);

                if (r)
                {
                    _arrsv.Delete(entity.F_Role_Id, trans);
                    entity.D_Admin_Role_Right = entity.D_Admin_Role_Right.Where(a => a.Checked).Select(a => { a.F_Admin_Right_RoleId = entity.F_Role_Id; return a; }).ToList();
                    _arrsv.Insert(entity.D_Admin_Role_Right, trans);
                    trans.Commit();
                    return r;
                }

            }
            catch
            {

            }
            trans.Rollback();
            return false;

        }

        public D_Admin_Role GetFull(int id)
        {
            var sql = "select * from D_Admin_Role a left join D_Admin_Role_Right b on a.F_Role_Id=b.F_Admin_Right_RoleId where a.F_Role_Id=@id";

            var m = db.QueryMany<D_Admin_Role, D_Admin_Role_Right>(sql, a => a.D_Admin_Role_Right, a => a.F_Role_Id, new { id }, splitOn: "F_Admin_Right_RoleId").SingleOrDefault();

            if (m != null && m.D_Admin_Role_Right != null)
            {
                m.D_Admin_Role_Right = m.D_Admin_Role_Right.Where(a => a != null).Select(a => { a.Checked = true; return a; }).ToList();
            }

            return m;
        }

        public int Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0)
            {
                return 0;
            }

            var sql = "delete from D_Admin_Role where F_Role_Id in({0})";

            return db.Execute(string.Format(sql, ids.ToSqlSafeNums()));
        }

        public List<D_Admin_Role> GetList(int top = 10, bool? islock = null)
        {
            var sql = "select {0} * from D_Admin_Role where 1=1 {1}";

            return db.Query<D_Admin_Role>(string.Format(sql, top > 0 ? " top " + top : "", islock != null ? " and F_Role_IsLock=@islock " : ""), new { islock }).ToList();
        }

        /// <summary>
        /// 根据管理员ID取得管理员角色列表
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public List<D_Admin_Role> GetRolesForAdminId(int adminid)
        {
            var sql = "select * from D_Admin_Role where  F_Role_IsLock=0 and F_Role_Id in(select F_Admin_Role_RoleId from D_Admin_To_Role where F_Admin_Role_AdminId=@adminid)  ";

            return db.Query<D_Admin_Role>(sql, new { adminid }).ToList();

        }
    }
}
