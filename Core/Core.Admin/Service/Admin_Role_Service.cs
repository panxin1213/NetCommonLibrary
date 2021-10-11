using Common.Library.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Admin.Model;

namespace Core.Admin.Service
{
    public class Admin_Role_Service : ServiceBase<T_Admin_Role>
    {
        private Admin_Role_Right_Service _arrsv;

        public Admin_Role_Service(IDbConnection db)
            : base(db)
        {
            _arrsv = new Admin_Role_Right_Service(db);
        }

        /// <summary>
        /// 插入角色，包含权限
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override long Insert(T_Admin_Role entity)
        {
            var trans = db.BeginTransaction();
            long r = 0;
            try
            {
                r = db.Insert(entity, trans);

                if (r > 0)
                {
                    entity.T_Admin_Role_Right = entity.T_Admin_Role_Right.Where(a => a.Checked).ToList();
                    if (entity.T_Admin_Role_Right.Count > 0)
                    {
                        db.InsertWithKeyField(entity.T_Admin_Role_Right.Select(a => { a.F_Admin_Right_RoleId = r.ToInt(); return a; }), trans);
                    }
                }
                if (r > 0)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
                return r;
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                trans.Rollback();
                return 0;
            }
        }

        /// <summary>
        /// 通过ID返回角色(包含权限集合)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T_Admin_Role Get(dynamic id)
        {
            var sql = "select * from T_Admin_Role a outer apply (select * from T_Admin_Role_Right where F_Admin_Right_RoleId=a.F_Role_Id) b where F_Role_Id=@id";

            return db.QueryMany<T_Admin_Role, T_Admin_Role_Right>(sql, a => a.T_Admin_Role_Right, a => a.F_Role_Id, new { id = (int)id }, splitOn: "F_Admin_Right_RoleId").SingleOrDefault();
        }

        /// <summary>
        /// 更新角色(包含权限)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Update(T_Admin_Role entity)
        {
            var trans = db.BeginTransaction();
            var r = false;

            try
            {
                r = db.Update(entity, trans);

                if (r)
                {
                    _arrsv.DeleteByRoleIds(new[] { entity.F_Role_Id }, trans);
                    entity.T_Admin_Role_Right = entity.T_Admin_Role_Right.Where(a => a.Checked).ToList();
                    if (entity.T_Admin_Role_Right.Count > 0)
                    {
                        db.InsertWithKeyField(entity.T_Admin_Role_Right.Select(a => { a.F_Admin_Right_RoleId = entity.F_Role_Id; return a; }), trans);
                    }
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }

                return r;
                
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                trans.Rollback();
                return false;
            }

        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<int> ids)
        {
            var sql = "delete from T_Admin_Role where F_Role_Id in @ids";

            return db.Execute(sql, new { ids });
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

            return db.Execute("update T_Admin_Role set F_Role_IsLock=-(F_Role_IsLock-1) where F_Role_Id in (@ids)", new { ids });

        }

        /// <summary>
        /// 返回角色组
        /// </summary>
        /// <param name="top"></param>
        /// <param name="islock"></param>
        /// <returns></returns>
        public List<T_Admin_Role> GetList(int top = 0, bool? islock = null)
        {
            var sql = string.Format("select {0} * from T_Admin_Role where 1=1 {1}"
                , top > 0 ? " top " + top : ""
                , islock != null ? " and F_Role_IsLock=@islock " : "");


            return db.Query<T_Admin_Role>(sql, new { islock }).ToList();
        }
    }
}
