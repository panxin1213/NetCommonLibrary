using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Admin.Model;

namespace Core.Admin.Service
{
    public class Admin_Role_Right_Service : ServiceBase<T_Admin_Role_Right>
    {
        public Admin_Role_Right_Service(IDbConnection db)
            : base(db)
        {

        }


        /// <summary>
        /// 通过角色ID取权限列表
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public List<T_Admin_Role_Right> GetListForRoleId(int roleid)
        {
            var sql = "select * from T_Admin_Role_Right where F_Admin_Right_RoleId=@roleid ";

            return db.Query<T_Admin_Role_Right>(sql, new { roleid }).ToList();
        }

        /// <summary>
        /// 根据管理员ID取权限列表
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public List<T_Admin_Role_Right> GetListForAdminId(int adminid)
        {
            var list = new List<T_Admin_Role_Right>();

            const string sql = @"select * from T_Admin_Role_Right where F_Admin_Right_RoleId in 
                ( select F_Role_Id from T_Admin_Role where F_Role_IsLock=0 and F_Role_Id in( select F_Admin_Role_RoleId from T_Admin_To_Role where F_Admin_Role_AdminId=@adminid )  )";

            list = db.Query<T_Admin_Role_Right>(sql, new { @adminid = adminid }).ToList();

            return list;
        }

        /// <summary>
        /// 删除权限根据角色ID集合
        /// </summary>
        /// <param name="roleids"></param>
        /// <returns></returns>
        public int DeleteByRoleIds(IEnumerable<int> roleids, IDbTransaction trans = null)
        {
            var sql = "delete from T_Admin_Role_Right where F_Admin_Right_RoleId in @roleids";

            return db.Execute(sql, new { roleids }, trans);
        }
    }
}
