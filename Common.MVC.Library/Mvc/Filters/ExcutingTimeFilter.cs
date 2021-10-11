using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace System.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcutingTimeFilter : ActionFilterAttribute
    {
        private Stopwatch timer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.timer = new Stopwatch();
            this.timer.Start();
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            this.timer.Stop();
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "此页运行时间: {0}ms", this.timer.ElapsedMilliseconds));
            if (filterContext.Result is ViewResult)
            {
                ((ViewResult)filterContext.Result).ViewData["ExecutionTime"] = this.timer.Elapsed.TotalMilliseconds.ToString("0.0000");
            }
        }
    }
}
