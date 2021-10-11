using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 脚本相关扩展
    /// </summary>
    public static class HtmlHelperScriptEx
    {

        ///// <summary>
        ///// 用jquery 的tips显示tempdata里的成功消息
        ///// </summary>
        ///// <param name="html"></param>
        ///// <param name="key"></param>
        ///// <param name="format"></param>
        ///// <returns></returns>
        //public static MvcHtmlString TipsTempDataSuccessMessage(this HtmlHelper html, string key, string format = "{0}")
        //{
        //    var v = html.ViewContext.TempData[key];
        //    if (v != null && !String.IsNullOrEmpty(v.ToString()))
        //    {
        //        return MvcHtmlString.Create(String.Format("<script type=\"text/javascript\">$.dialog.tips('{0}',3,'success.gif')</script>",
        //            String.Format(format, v)));
        //    }
        //    return null;
        //}
    }
}
