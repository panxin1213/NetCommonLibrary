using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using Firm.Model;
using Common.Library;
using System.Data;
using Firm.Core.Admin.Service;

namespace Firm.Core.Admin.ManagePermissions
{
    public class Principal : IPrincipal
    {
        private Identity identity;

        private IEnumerable<D_Admin_Role_Right> manageRights;

        public Principal(Identity identity)
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

        public bool IsInRight(string namespaces, string classname)
        {
            if (identity.IsSupper) return true;
            return manageRights.Any(a => a.NameSpace.Equals(namespaces, StringComparison.OrdinalIgnoreCase) && a.ClassName.Equals(classname, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<D_Admin_Role_Right> GetAllRoleRights()
        {
            if (identity == null)
            {
                manageRights = null;
                return null;
            }
            if (identity.IsSupper) return new D_Admin_Role_Right[] { };


            if (!identity.IsAuthenticated)
            {
                manageRights = null;
                return null;
            }

            using (IDbConnection db = Conn.Get())
            {
                var _arrsv = new Admin_Role_Right_Service(db);

                return _arrsv.GetListForAdminId(identity.Id);
            }
        }

        /// <summary>
        /// 取得controller下有权限的action,controller为null则取出全部有index的action管理权限菜单
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public List<D_Admin_Role_Right> GetMenu(string namespaces = null)
        {
            if (namespaces == null)
            {
                var menulist = manageRights.Where(a => a.ClassName.ToLower() == "select").ToList();

                return menulist;
            }

            return manageRights.Where(a => a.NameSpace.ToLower() == namespaces.ToLower()).ToList();
        }

        public List<D_Admin_Role_Right> GetMenu(List<string> namespaces)
        {
            return manageRights.Where(a => namespaces.Any(b => a.NameSpace.ToLower() == b.ToLower())).ToList();
        }
    }
}
