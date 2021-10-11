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
    public static class HiddenIndexEx
    {
        /// <summary>
        /// 集合提交时 用来指定元素隐藏索引（Index） 以便获取不连续的元素
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString HiddenIndex(this HtmlHelper html)
        {
            var prefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            var endOfPrefix = prefix.IndexOf('[');
            if (endOfPrefix <= -1) return null; //不是集合
            var index = prefix.Substring(endOfPrefix+1, prefix.LastIndexOf(']') - endOfPrefix - 1);
            var prefixNew = "";
            prefixNew = prefix.Substring(0,endOfPrefix);
            if (prefixNew != String.Empty) prefixNew+=".";
            return MvcHtmlString.Create(String.Format(@"<input type=""hidden"" name=""{0}Index"" value=""{1}"" />", prefixNew, index));

        }

    }
}
