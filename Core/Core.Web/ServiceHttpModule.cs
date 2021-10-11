using Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Web
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
                a.Context.Items.Add("ServiceMain", GetWebServiceMain());
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

        private ServiceMain GetWebServiceMain()
        {
            if (BaseConfig.Current.ServiceMainParam != null && !String.IsNullOrWhiteSpace(BaseConfig.Current.ServiceMainParam.Assembly) && !String.IsNullOrWhiteSpace(BaseConfig.Current.ServiceMainParam.Type))
            {
                var r = Assembly.Load(BaseConfig.Current.ServiceMainParam.Assembly).CreateInstance(BaseConfig.Current.ServiceMainParam.Type) as ServiceMain;
                if (r == null)
                    return new ServiceMain();
                return r;
            }
            return new ServiceMain();
        }
    }
}
