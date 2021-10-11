using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Core.Manage.ManageShip
{
    public static class ManageCookieManager
    {
        public static readonly string COOKIES_NAME = "bm_";
        public static readonly string COOKIES_USER_NAME = "n";
        public static readonly string COOKIES_USER_PASS = "p";
        public static readonly string HttpContextKey = "ManageLoginInfo";

        /// <summary>
        /// 当前的Context中取CustomPrincipal
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static CustomIdentity GetFromCookies(this HttpContext context)
        {
            return new HttpContextWrapper(context).GetFromCookies();
        }

        public static void SetCookies(this HttpContext context, int id, string username, string nick_name, string password, bool isSupper, bool isAuthenticated, int rememberDay = 0)
        {
            new HttpContextWrapper(context).SetCookies(id, username, nick_name, password, isSupper, isAuthenticated, rememberDay);
        }
        /// <summary>
        /// 当前Httpcontext 中取CustomIdentity
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static CustomIdentity GetIdentity(this HttpContext context)
        {
            return new HttpContextWrapper(context).GetIdentity();
        }

        /// <summary>
        /// 当前Httpcontext 中取CustomIdentity
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static CustomIdentity GetIdentity(this HttpContextBase context)
        {
            return context.User.Identity as CustomIdentity;
        }
        /// <summary>
        /// 当前的Context中取CustomPrincipal
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static CustomPrincipal GetMember(this HttpContextBase context)
        {
            return context.User as CustomPrincipal;
        }
        /// <summary>
        /// 网站登录cookies同时设置
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="nick"></param>
        /// <param name="isAuthenticated"></param>
        /// <param name="rememberDay"></param>
        public static void SetCookies(this HttpContextBase context, int id, string username, string nick_name, string password, bool isSupper, bool isAuthenticated, int rememberDay = 0)
        {
            context.setCookies(id, username, nick_name, password, isSupper, isAuthenticated, rememberDay);

        }
        /// <summary>
        /// 当前系统登录cookeis设置
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="nick"></param>
        /// <param name="isAuthenticated"></param>
        /// <param name="rememberDay"></param>
        private static void setCookies(this HttpContextBase context, int id, string username, string nick_name, string password, bool isSupper, bool isAuthenticated, int rememberDay = 0)
        {
            var m = new CustomIdentity(id, username, nick_name, isSupper, isAuthenticated);
            context.User = new CustomPrincipal(m);
            if (rememberDay < 0 && m.IsDefaultUser()) //退出登录
            {
                context.Session.Abandon();

            }
            else if (!m.IsDefaultUser()) //默认未登录不记录
            {
                context.Session[COOKIES_NAME + "manage"] = m;
            }

            var c = new HttpCookie(COOKIES_NAME + "a");
            c.Values[COOKIES_NAME + COOKIES_USER_NAME] = username;
            c.Values[COOKIES_NAME + COOKIES_USER_PASS] = password;
            //c.Domain = "."+BMConfig.Current.DomainBase;
            c.HttpOnly = true;
            if (rememberDay != 0)
            {
                c.Expires = DateTime.Now.AddDays(rememberDay);
            }
            context.Response.Cookies.Add(c);
        }
        public static CustomIdentity GetFromCookies(this HttpContextBase context)
        {
            if (context.Session != null)
            {


                var r = context.Session[COOKIES_NAME + "manage"] as CustomIdentity;
                //System.Web.HttpContext.Current.Response.Write(r == null);

                if (r == null || !r.IsAuthenticated)
                {
                    //System.Web.HttpContext.Current.Response.Write("11");
                    CheckCookiesLogin(context);
                    r = context.Session[COOKIES_NAME + "manage"] as CustomIdentity ?? new CustomIdentity();
                }
                return r;


            }
            return new CustomIdentity();
        }
        public static KeyValuePair<string, string> GetCookies(HttpContextBase context, string type = null)
        {
            //新站cookies
            var c = context.Request.Cookies[COOKIES_NAME + "a"];
            string u = String.Empty;
            string p = String.Empty;
            if (c != null)
            {
                u = c.Values[COOKIES_NAME + COOKIES_USER_NAME];
                p = c.Values[COOKIES_NAME + COOKIES_USER_PASS];
            }
            if (String.IsNullOrEmpty(u))
                u = string.Empty;
            if (String.IsNullOrEmpty(p))
                p = string.Empty;
            return new KeyValuePair<string, string>(u, p);
        }
        private static void CheckCookiesLogin(HttpContextBase context)
        {
            //System.Web.HttpContext.Current.Response.Write("a:");
            var r = GetCookies(context);


            if (!string.IsNullOrEmpty(r.Key) && !string.IsNullOrEmpty(r.Value))
            {
                string o;
                if (!ManageProvider.Login(r.Key, r.Value, out o))
                    ManageProvider.Logout();
            }
        }
    }
}
