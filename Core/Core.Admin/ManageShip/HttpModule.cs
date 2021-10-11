using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Web.SessionState;
using Core.Base;

namespace Core.Manage.ManageShip
{
    /// <summary>
    /// 用户登录状态维护
    /// </summary>
    public class HttpModule : IHttpModule, IRequiresSessionState
    {
        static readonly string _rootBaseDomain = "." + BaseConfig.Current.DomainBase;
        Type stateServerSessionProvider = typeof(HttpSessionState).Assembly.GetType("System.Web.SessionState.OutOfProcSessionStateStore");
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {

            CrossDoaminSession(context);
            context.AcquireRequestState += new EventHandler(application_AcquireRequestState);


        }

        private void CrossDoaminSession(HttpApplication context)
        {
            FieldInfo uriField = stateServerSessionProvider.GetField("s_uribase", BindingFlags.Static | BindingFlags.NonPublic);
            if (uriField == null)
                throw new ArgumentException("HttpModule 'CrossDoaminSession' uriField error");

            uriField.SetValue(null, _rootBaseDomain);

            context.EndRequest += new System.EventHandler(CrossDoaminCookies);
        }

        private void application_AcquireRequestState(object sender, EventArgs e)
        {
            var a = sender as HttpApplication;
            //在用Chorme测试中js和css文件也引发此事件。故跳过
            if (a.Context.Session != null && (a.Request.CurrentExecutionFilePathExtension == ""
                ||
                ",.js,.css,.ico,.jpg,.gif,.png,.woff,.svg".IndexOf(a.Request.CurrentExecutionFilePathExtension, StringComparison.OrdinalIgnoreCase) == -1))
            {
                a.Context.Items.Add(ManageCookieManager.HttpContextKey, new CustomPrincipal(a.Context.GetFromCookies()));
            }
        }

        private void CrossDoaminCookies(object sender, System.EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            for (int i = 0; i < app.Context.Response.Cookies.Count; i++)
            {
                app.Context.Response.Cookies[i].Domain = _rootBaseDomain;
            }

        }
    }
}
