using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using Core.Admin.Service;
using Core.Admin;
using Admin.Model;
using Dapper.Contrib.Extensions;
using Dapper;
using ChinaBM.Common;

namespace Core.Manage.ManageShip
{
    public static class ManageProvider
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName">登录名（手机号码/电子邮件地址/用户名）</param>
        /// <param name="password">密码</param>
        /// <param name="msg">登录失败的信息</param>
        /// <param name="usertyperole"></param>
        /// <param name="contactType">联系人类型</param>
        /// <returns></returns>
        public static bool Login(string loginName, string password, out string msg)
        {
            msg = null;
            var identity = HttpContext.Current.User.Identity as CustomIdentity;
            if (identity != null && identity.IsAuthenticated) return true;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                msg = "请输入用户名和密码";
                return false;
            }
            using (var db = AdminConn.Get())
            {
                var r = db.Query<T_Admin>("SELECT * FROM T_Admin where F_Admin_Name=@name and F_Admin_Password=@pass ", new { name = loginName, pass = password }).FirstOrDefault();
                if (r != null)
                {
                    if (r.F_Admin_IsLock)
                    {
                        msg = "被锁定";
                    }
                    else
                    {
                        HttpContext.Current.SetCookies(r.F_Admin_Id, r.F_Admin_Name, r.F_Admin_RealName, r.F_Admin_Password, r.F_Admin_IsSupper, true, 1);

                        var _alsv = new Admin_Log_Service(db);

                        _alsv.Insert(new T_Admin_Log { F_A_L_AdminId = r.F_Admin_Id, F_A_L_LoginTime = DateTime.Now, F_A_L_Ip = HttpKit.CurrentRequestIP });

                        return true;
                    }
                }
                else
                {
                    msg = "用户名或密码错误";
                }
            }
            return false;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Logout()
        {
            HttpContext.Current.SetCookies(0, null, null, null, false, false, -8);
        }


    }
}
