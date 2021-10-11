using Firm.Core.Admin.Service;
using Firm.Core.Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firm.Core.Common.BLL
{
    public class CommonService : BaseService
    {
        public CommonService()
            : base("Common")
        {

        }

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

                _adminService = new Admin_Service(db);
                return _adminService;
            }
        }




        private Admin_Role_Service _adminRoleService;

        /// <summary>
        /// 管理员角色Service
        /// </summary>
        public Admin_Role_Service AdminRoleService
        {
            get
            {
                if (_adminRoleService != null) return _adminRoleService;

                _adminRoleService = new Admin_Role_Service(db);
                return _adminRoleService;
            }
        }



        private Admin_Logs_Service _adminLogsService;

        /// <summary>
        /// 管理员登录日志Service
        /// </summary>
        public Admin_Logs_Service AdminLogsService
        {
            get
            {
                if (_adminLogsService != null) return _adminLogsService;

                _adminLogsService = new Admin_Logs_Service(db);
                return _adminLogsService;
            }
        }


        private Admin_Record_Service _adminRecordService;

        /// <summary>
        /// 管理员操作日志Service
        /// </summary>
        public Admin_Record_Service AdminRecordService
        {
            get
            {
                if (_adminRecordService != null) return _adminRecordService;

                _adminRecordService = new Admin_Record_Service(db);
                return _adminRecordService;
            }
        }

        #endregion

        #region 公共部分Service



        private FriendLink_Service _friendLinkService;

        /// <summary>
        /// 友情链接Service
        /// </summary>
        public FriendLink_Service FriendLinkService
        {
            get
            {
                if (_friendLinkService != null) return _friendLinkService;

                _friendLinkService = new FriendLink_Service(db);
                return _friendLinkService;
            }
        }



        private SEO_Service _seoService;

        /// <summary>
        /// SEOService
        /// </summary>
        public SEO_Service SEOService
        {
            get
            {
                if (_seoService != null) return _seoService;

                _seoService = new SEO_Service(db);
                return _seoService;
            }
        }

        #endregion
    }
}
