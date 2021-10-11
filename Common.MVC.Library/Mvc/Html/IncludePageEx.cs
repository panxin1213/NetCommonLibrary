using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace System.Web.Mvc
{
    /// <summary>
    /// 包含文件
    /// </summary>
    public static class IncludePageEx
    {
        /// <summary>
        /// 包含外部的文件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MvcHtmlString WebPage(this HtmlHelper htmlHelper, string url)
        {
            return MvcHtmlString.Create(new WebClient().DownloadString((url)));
        }
        /// <summary>
        /// 当前服务器上的页面
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MvcHtmlString ServerPage(this HtmlHelper htmlHelper, string url)
        {
            return MvcHtmlString.Create(new WebClient().DownloadString(htmlHelper.ViewContext.HttpContext.Server.MapPath(url)));
        } 
    }
}
