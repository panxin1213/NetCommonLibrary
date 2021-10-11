using Common.Library;
using Core.Admin;
using Core.Admin.Service;
using Core.Common.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Web
{
    public class ServiceMain : IDisposable
    {
        ~ServiceMain()
        {
            Dispose();
        }

        private IDbConnection db = null;

        #region 管理员部分Service

        private Admin_Service _adminService;

        /// <summary>
        /// 管理员Service
        /// </summary>
        public Admin_Service AdminService
        {
            get
            {
                if (_adminService != null) return _adminService;

                if (db == null) db = AdminConn.Get();
                _adminService= new Admin_Service(db);
                return _adminService;
            }
        }


        private Admin_Log_Service _adminlogService;

        /// <summary>
        /// 管理后台登录日志service
        /// </summary>
        public Admin_Log_Service AdminLogService
        {
            get
            {
                if (_adminlogService != null) return _adminlogService;

                if (db == null) db = AdminConn.Get();
                _adminlogService = new Admin_Log_Service(db);
                return _adminlogService;
            }
        }


        private Admin_Role_Service _adminroleService;

        /// <summary>
        /// 管理员角色service
        /// </summary>
        public Admin_Role_Service AdminRoleService
        {
            get
            {
                if (_adminroleService != null) return _adminroleService;

                if (db == null) db = AdminConn.Get();
                _adminroleService = new Admin_Role_Service(db);
                return _adminroleService;
            }
        }
        
        #endregion

        #region 公共部分Service

        private AD_Service _ad_Service;


        /// <summary>
        /// 广告service
        /// </summary>
        public AD_Service ADService
        {
            get
            {
                if (_ad_Service != null) return _ad_Service;

                if (db == null) db = AdminConn.Get();
                _ad_Service = new AD_Service(db);
                return _ad_Service;
            }
        }


        private SEO_Service _seo_Service;


        /// <summary>
        /// SEO service
        /// </summary>
        public SEO_Service SEOService
        {
            get
            {
                if (_seo_Service != null) return _seo_Service;

                if (db == null) db = AdminConn.Get();
                _seo_Service = new SEO_Service(db);
                return _seo_Service;
            }
        }


        private FriendLink_Service _friendLinkService;


        /// <summary>
        /// 友链 service
        /// </summary>
        public FriendLink_Service FriendLinkService
        {
            get
            {
                if (_friendLinkService != null) return _friendLinkService;

                if (db == null) db = AdminConn.Get();
                _friendLinkService = new FriendLink_Service(db);
                return _friendLinkService;
            }
        }




        private Robot_Service _robotService;


        /// <summary>
        /// 关键词 service
        /// </summary>
        public Robot_Service RobotService
        {
            get
            {
                if (_robotService != null) return _robotService;

                if (db == null) db = AdminConn.Get();
                _robotService = new Robot_Service(db);
                return _robotService;
            }
        }


        #endregion

        public virtual void Dispose()
        {
            if (db != null)
            {
                db.Close();
                db.Dispose();
                db = null;
            }
        }
    }
}
