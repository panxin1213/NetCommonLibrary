using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Collections;
using System.Threading;
using Firm.Core.Admin.ManagePermissions;
using Firm.Core.Admin.Authority;

namespace System.Web
{
    public static class Manager
    {
        public static readonly string COOKIES_NAME = "bm_";
        public static readonly string COOKIES_USER_NAME = "n";
        public static readonly string COOKIES_User_PASS = "p";

        public static Identity GetManage(this HttpContext context)
        {
            return new HttpContextWrapper(context).GetManage();
        }

        static Manager()
        {

        }

        #region GetManage 获取用户登录对象
        /// <summary>
        ///  用户登录对象
        /// </summary>
        public static Identity GetManage(this HttpContextBase context)
        {
            Identity m = null;

            if (context.Session != null)
            {

                m = context.Session[COOKIES_NAME + "Manage"] as Identity;

                if (m == null || !m.IsAuthenticated)
                {
                    CheckCookiesLogin(context);

                    m = context.Session[COOKIES_NAME + "Manage"] as Identity;
                }

            }

            return m == null ? new Identity() : m;

        }
        #endregion

        #region SetManageCookies

        public static void SetManageCookies(this HttpContext context, int id, string username, string password, string nick, bool isSupper, bool isAuthenticated)
        {
            new HttpContextWrapper(context).SetManageCookies(id, username, password, nick, isSupper, isAuthenticated);
        }

        public static void SetManageCookies(this HttpContextBase context, int id, string username, string password, string nick, bool isSupper, bool isAuthenticated)
        {
            int rememberDay = 1;
            if (isAuthenticated)
            {
                var identity = new Identity(id, username, nick, isSupper, true);
                context.User = new Principal(identity);
                context.Session[COOKIES_NAME + "Manage"] = identity;
            }
            else
            {
                rememberDay = -8;
                context.Session.Remove(COOKIES_NAME + "Manage");
            }

            var c = new HttpCookie(COOKIES_NAME + "a");
            c.HttpOnly = true;
            c.Values[COOKIES_NAME + COOKIES_USER_NAME] = username;
            c.Values[COOKIES_NAME + COOKIES_User_PASS] = password;
            c.Expires = DateTime.Now.AddDays(rememberDay);
            context.Response.Cookies.Add(c);

        }

        #endregion

        private static void CheckCookiesLogin(HttpContextBase context)
        {
            var c = context.Request.Cookies.Get(COOKIES_NAME + "a");
            var u = "";
            var p = "";

            if (c == null)
            {
                return;
            }

            u = c.Values[COOKIES_NAME + COOKIES_USER_NAME];
            p = c.Values[COOKIES_NAME + COOKIES_User_PASS];


            if (!string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(p))
            {
                string o;
                if (!ManageProvider.Login(u, p, out o))
                {
                    ManageProvider.Logout();
                }
            }
        }


        public static bool LoginValid()
        {
            var u = HttpContext.Current.GetManage();

            if (u == null || !u.IsAuthenticated)
            {
                return false;
            }

            HttpContext.Current.User = new Principal(u);

            return true;
        }

        public static bool RoleRightValid(string namespaces, string classname)
        {
            var pl = ManagePermissions.GetAllPermissions();
            var p = HttpContext.Current.User as Principal;
            var r = p.Identity as Identity;

            if (r.IsSupper)
            {
                return true;
            }
            

            if (pl.Any(a => a.Value.Any(b => b.NameSpace.Equals(namespaces, StringComparison.OrdinalIgnoreCase) && b.ClassName.Equals(classname, StringComparison.OrdinalIgnoreCase))))
            {
                if (!p.IsInRight(namespaces, classname))
                {

                    return false;
                }
            }
            return true;
        }

    }
}
