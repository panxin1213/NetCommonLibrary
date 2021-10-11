using Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web
{
    public static class ServiceHttpContext
    {
        /// <summary>
        /// 当前的Context中取ServiceMain
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ServiceMain GetServiceMain(this HttpContext context)
        {
            return new HttpContextWrapper(context).GetServiceMain();
        }

        public static ServiceMain GetServiceMain(this HttpContextBase context)
        {
            return context.Items["ServiceMain"] as ServiceMain;
        }
    }
}
