using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Dynamic;
using System.Reflection;
using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// 重写视图，方便扩展 视图功能
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class SmanViewPage<TModel> : WebViewPage<TModel>
    {

        /// <summary>
        /// 控制器名
        /// </summary>
        public string ControllerName { get; private set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string ActionName { get;private set; }
        /// <summary>
        /// 区域名
        /// </summary>
        public string AreaName { get; private set; }

        /// <summary>
        /// 在设置ViewData时
        /// </summary>
        /// <param name="viewData"></param>
        protected override void SetViewData(ViewDataDictionary viewData)
        {
            ControllerName = this.ViewContext.RouteData.Values.TryGetValue("Controller", "").ToString();
            ActionName = this.ViewContext.RouteData.Values.TryGetValue("Action", "").ToString();
            AreaName = this.ViewContext.RouteData.Values.TryGetValue("Area", "").ToString();

            base.SetViewData(viewData);
        }
        
    }

}
