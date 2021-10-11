using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeiXin.Core.Model;


namespace WeiXin.Core
{
    public class WeiXin_Service 
    {

        /// <summary>
        /// 写入微信用户openid session
        /// </summary>
        /// <param name="m"></param>
        public static void SetIdentityUser(IdentityUser m)
        {
            System.Web.HttpContext.Current.Session["IdentityUser"] = m;
        }

        /// <summary>
        /// 读取微信用户openid session
        /// </summary>
        /// <returns></returns>
        public static IdentityUser GetIdentityUser()
        {
            return System.Web.HttpContext.Current.Session["IdentityUser"] as IdentityUser;
        }
    }
}
