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
    public class Admin_Role_Right_Service : ServiceBase<D_Admin_Role_Right>
    {
        public Admin_Role_Right_Service(IDbConnection db)
            : base(db)
        {

        }

        public int Insert(IEnumerable<D_Admin_Role_Right> l, IDbTransaction trans)
        {
            return db.InsertWithKeyField(l, trans);
        }

        public int Delete(int roleid, IDbTransaction trans)
        {
            var sql = "delete from D_Admin_Role_Right where F_Admin_Right_RoleId=@roleid";

            return db.Execute(sql, new { roleid }, trans);
        }

        /// <summary>
        /// 根据管理员ID取权限列表
        /// </summary>
        /// <param name="adminid"></param>
        /// <returns></returns>
        public List<D_Admin_Role_Right> GetListForAdminId(int adminid)
        {
            var sql = "select * from D_Admin_Role_Right where F_Admin_Right_RoleId in( select F_Role_Id from D_Admin_Role where F_Role_IsLock=0 and F_Role_Id in( select F_Admin_Role_RoleId from D_Admin_To_Role where F_Admin_Role_AdminId=@adminid )  )";

            return db.Query<D_Admin_Role_Right>(sql, new { adminid }).ToList();
        }
    }
}
