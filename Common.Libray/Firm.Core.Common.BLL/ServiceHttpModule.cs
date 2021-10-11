using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Firm.Core.Common.BLL
{
    public class ServiceHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler((sender, e) =>
            {
                var a = sender as HttpApplication;
                //在用Chorme测试中js和css文件也引发此事件。故跳过
                if (a.Request.CurrentExecutionFilePathExtension == ""
                    ||
                    ",.js,.css,.ico,.jpg,.gif,.png,.bmp".IndexOf(a.Request.CurrentExecutionFilePathExtension, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    a.Context.Items.Add("ServiceMain", new ServiceMain());
                }
            });

            context.EndRequest += new EventHandler((sender, e) =>
            {
                var a = sender as HttpApplication;

                if (a.Context.Items.Contains("ServiceMain"))
                {
                    if ((a.Context.Items["ServiceMain"] as ServiceMain) != null)
                    {
                        (a.Context.Items["ServiceMain"] as ServiceMain).Dispose();
                        a.Context.Items["ServiceMain"] = null;
                        a.Context.Items.Remove("ServiceMain");
                    }
                }

            });
        }

        
    }
}
