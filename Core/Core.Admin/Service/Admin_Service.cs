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
using Dapper;
using Dapper.Contrib.Extensions;

namespace Core.Admin.Service
{
    public class Admin_Service : ServiceBase<T_Admin>
    {
        public Admin_Service(IDbConnection db)
            : base(db)
        {

        }

        /// <summary>
        /// 插入管理员(包含角色组)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override long Insert(T_Admin entity)
        {
            var trans = db.BeginTransaction();
            long r = 0;

            try
            {
                r = db.Insert(entity, trans);

                if (r > 0)
                {
                    entity.T_Admin_Role = entity.T_Admin_Role.Where(a => a.Checked).ToList();
                    if (entity.T_Admin_Role.Count > 0)
                    {
                        db.InsertWithKeyField(entity.T_Admin_Role.Select(a => new T_Admin_To_Role { F_Admin_Role_AdminId = r.ToInt(), F_Admin_Role_RoleId = a.F_Role_Id }), trans);
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
                return 0;
            }
        }

        /// <summary>
        /// 返回管理员对象(包含角色组)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T_Admin Get(dynamic id)
        {
            var sql = "select * from T_Admin a outer apply (select c.* from T_Admin_To_Role b inner join T_Admin_Role c on b.F_Admin_Role_RoleId=c.F_Role_Id where b.F_Admin_Role_AdminId=a.F_Admin_Id ) t where a.F_Admin_Id=@id";

            return db.QueryMany<T_Admin, T_Admin_Role>(sql, a => a.T_Admin_Role, a => a.F_Admin_Id, new { id = (int)id }, splitOn: "F_Role_Id").SingleOrDefault();
        }

        /// <summary>
        /// 更新管理员(包含角色)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Update(T_Admin entity)
        {
            var trans = db.BeginTransaction();
            var r = false;

            try
            {
                r = db.Update(entity, trans);

                if (r)
                {
                    db.Execute("delete from T_Admin_To_Role where F_Admin_Role_AdminId=@id", new { id = entity.F_Admin_Id }, trans);
                    entity.T_Admin_Role = entity.T_Admin_Role.Where(a => a.Checked).ToList();
                    if (entity.T_Admin_Role.Count > 0)
                    {
                        db.InsertWithKeyField(entity.T_Admin_Role.Select(a => new T_Admin_To_Role { F_Admin_Role_AdminId = entity.F_Admin_Id, F_Admin_Role_RoleId = a.F_Role_Id }), trans);
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
            var sql = "delete from T_Admin where F_Admin_Id in @ids";

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

            return db.Execute("update T_Admin set F_Admin_IsLock=-(F_Admin_IsLock-1) where F_Admin_Id in (@ids)", new { ids });

        }

        /// <summary>
        /// 验证用户名是否唯一
        /// </summary>
        /// <param name="admin_name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckName(string admin_name, int id)
        {
            int count = db.Query<int>("select count(0) from T_Admin where F_Admin_Id<>@id and F_Admin_Name=@name", new { @name = admin_name, @id = id }).First();

            return count == 0;
        }
    }
}
