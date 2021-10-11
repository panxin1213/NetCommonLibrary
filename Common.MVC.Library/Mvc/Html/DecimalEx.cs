using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 
    /// </summary>
    public static class DecimalEx
    {
        /// <summary>
        /// 格式化输出decimal类型
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public static MvcHtmlString DecimalFor(this HtmlHelper helper,decimal money)
        {
            string str = money.ToString("#,###.00");
            return new MvcHtmlString(str);
        }
    }
}
