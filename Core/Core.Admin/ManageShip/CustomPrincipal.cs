using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Data;
using Core.Admin.Service;
using Admin.Model;
using Core.Admin;

namespace System.Web
{
    /// <summary>
    /// 附加到HttpContext.User上 相关用户操作
    /// </summary>
    [Serializable]
    public class CustomPrincipal : IPrincipal
    {

        private CustomIdentity identity;

        private IEnumerable<T_Admin_Role_Right> manageRights;

        public CustomPrincipal(CustomIdentity identity)
        {
            this.identity = identity;
            manageRights = GetAllRoleRights();
        }

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }


        public bool IsInRight(string controller, string action)
        {
            if (identity.IsSupper) return true;
            return manageRights.Any(a => a.F_Admin_Right_Controller.Equals(controller, StringComparison.OrdinalIgnoreCase) && a.F_Admin_Right_Action.Equals(action, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<T_Admin_Role_Right> GetAllRoleRights()
        {
            if (identity.IsSupper) return new T_Admin_Role_Right[] { };
            if (identity == null)
            {
                manageRights = null;
                return null;
            }

            if (!identity.IsAuthenticated)
            {
                manageRights = null;
                return null;
            }

            using (IDbConnection db = AdminConn.Get())
            {
                Admin_Role_Right_Service arrsv = new Admin_Role_Right_Service(db);

                return arrsv.GetListForAdminId(identity.ID);
            }
        }

        /// <summary>
        /// 取得controller下有权限的action,controller为null则取出全部有index的action管理权限菜单
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public List<T_Admin_Role_Right> GetMenu(string controller = null)
        {
            if (controller == null)
            {
                var menulist = manageRights.Where(a => a.F_Admin_Right_Action.ToLower() == "index").ToList();

                return menulist;
            }

            return manageRights.Where(a => a.F_Admin_Right_Controller.ToLower() == controller.ToLower()).ToList();
        }

        public List<T_Admin_Role_Right> GetMenu(List<string> controllers)
        {
            return manageRights.Where(a => controllers.Any(b => a.F_Admin_Right_Controller.ToLower() == b.ToLower())).ToList();
        }

    }
}