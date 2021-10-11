using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Dapper;
using Firm.Core.Admin.ManagePermissions;
using Common.Library;
using Firm.Model;
using Firm.Core.Admin.Service;
using System.Data;
using Common.Library.Log;
using ChinaBM.Common;

namespace Firm.Core.Admin.ManagePermissions
{
    public static class ManageProvider
    {

        #region Login

        public static bool Login(string username, string password, out string msg)
        {
            return Login(username, password, out msg, null);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码，加密后</param>
        /// <param name="msg">错误返回信息</param>
        /// <param name="orgpassword">密码，加密前</param>
        /// <returns></returns>
        public static bool Login(string username, string password, out string msg,string orgpassword)
        {
            msg = null;
            var ident = HttpContext.Current.User.Identity as Identity;
            if (ident != null && ident.IsAuthenticated) return true;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                msg = "请输入用户名和密码";
                return false;
            }
            try
            {
                using (var db = Conn.Get())
                {
                    var alesv = new Admin_Login_Errors_Service(db);
                    var em = new D_Admin_Login_Errors { F_A_E_IP = HttpKit.CurrentRequestIP, F_A_E_Name = username, F_A_E_Pass = orgpassword, F_A_E_UserAgent = HttpKit.CurrentRequestUserAgent, F_A_E_Create = DateTime.Now };
                    var r = db.Query<D_Admin>("Select * From D_Admin where F_Admin_Name = @name and F_Admin_Password=@pass", new { name = username, pass = password }).SingleOrDefault();
                    if (r != null)
                    {
                        if (r.F_Admin_IsLock)
                        {
                            msg = "被锁定";
                            em.F_A_E_Message = msg;
                            alesv.Insert(em);
                        }
                        else
                        {
                            var trans = db.BeginTransaction();
                            try
                            {
                                var alsv = new Admin_Logs_Service(db);
                                var asv = new Admin_Service(db);

                                var result = asv.UpdateLastJoinTime(r.F_Admin_Id, trans);
                                if (result)
                                {
                                    alsv.InsertWithKeyField(new D_Admin_Logs { F_A_Log_Admin_Id = r.F_Admin_Id, F_A_Log_Create = DateTime.Now, F_A_Log_IP = HttpKit.CurrentRequestIP, F_A_Log_UserAgent = HttpKit.CurrentRequestUserAgent }, trans);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Error("ManageProvider.Login_Logs", e.Message, e);
                                msg = "登录失败，请稍后再试";
                                trans.Rollback();
                            }
                            

                            HttpContext.Current.SetManageCookies(r.F_Admin_Id, r.F_Admin_Name, r.F_Admin_Password, r.F_Admin_Nick, r.F_Admin_IsSupper, true);
                            return true;
                        }
                    }
                    else
                    {
                        msg = "用户名或密码错误";
                        em.F_A_E_Message = msg;
                        alesv.Insert(em);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("ManageProvider.Login", e.Message, e);
                msg = "登录失败，请稍后再试";
            }
            return false;

        }

        #endregion

        #region Loginout
        public static void Logout()
        {
            HttpContext.Current.SetManageCookies(0, null, null, null, false, false);
        }

        #endregion

        #region GetAllRoles
        /// <summary>
        /// 取得当前用户角色
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static IEnumerable<D_Admin_Role> GetAllRoles()
        {
            var r = System.Web.HttpContext.Current.GetManage();

            if (r == null)
            {
                return null;
            }
            using (IDbConnection db = Conn.Get())
            {
                var arsv = new Admin_Role_Service(db);

                var list = arsv.GetRolesForAdminId(r.Id);

                return list;
            }
        }

        #endregion
    }
}
