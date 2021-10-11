using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlHelperRawTextEx
    {
        /// <summary>
        /// 输出textarea的格式 html 有回车换行
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MvcHtmlString RawText(this HtmlHelper htmlHelper, object b) 
        {
            return RawText(b);
        }
        /// <summary>
        /// 输出textarea的格式 html 有回车换行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static MvcHtmlString RawText<T>(this HtmlHelper<T> htmlHelper, object b) 
        {
            return RawText(b);
        }
        /// <summary>
        /// 输出textarea的格式 html 有回车换行
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString RawTextFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return RawText(metadata.Model);
        }
        private static MvcHtmlString RawText(object b)
        {
            return MvcHtmlString.Create(String.Format("{0}",b).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" ", "&nbsp;").Replace("\r\n", "<br />"));
        }
    }
}
