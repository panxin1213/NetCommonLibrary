using System.Web;
using System;
using System.Diagnostics;
using Common.Library.Log;
namespace Common.Library.httpModules
{
    /// <summary>
    /// 统计页面完整的运行时间 
    /// 挂载到web.config统计
    /// 将以下代码加入 system.web 节内
	/// <httpModules><add name="ExecTime" type="Common.Library.httpModules.PageExecutionTimeAll"/></httpModules>
    /// </summary>
    public class PageExecutionTimeAll : IHttpModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="application"></param>
        public void Init(HttpApplication application) 
        {  

            application.BeginRequest += new EventHandler(application_BeginRequest);
            application.EndRequest += new EventHandler(application_EndRequest);
            //application.Context.ApplicationInstance.

        }
        private Stopwatch _timer = new Stopwatch();
        private void application_BeginRequest(object sender, EventArgs e)
        {
            //_timer = new Stopwatch();
            _timer.Start();
        }
        private void application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                HttpContext context = ((HttpApplication)sender).Context;
                _timer.Stop();
                var contentType = context.Response.ContentType;
                context.Response.AddHeader("t", _timer.Elapsed.TotalMilliseconds.ToString());
                if (contentType == "text/html" && context.Response.StatusCode == 200)
                {
                    var t = _timer.Elapsed.TotalMilliseconds;
                    if (t > 1)
                    {
                        ExceptionLoger.Add("运行时间:" + t);
                    }
                }
                //else if (contentType == "application/json" && context.Response.StatusCode == 200)
                //{
                //var t = _timer.Elapsed.TotalMilliseconds;
                //context.Response.Write("<!--" + t + "-->");
                //context.Response.Write("/*" + t + "*/");
                //}
                _timer.Reset();
            }
            catch
            {

            }
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose() {
            _timer = null;
        }  
    }
}
