using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Globalization;
using System.Web.Mvc.Html;
namespace System.Web.Mvc
{
    /// <summary>
    /// 地区
    /// </summary>
    public static class DistrictEx
    {
        /// <summary>
        /// 输出地区选择
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">表单名</param>
        /// <param name="defaultCity">默认值</param>
        /// <param name="optionLabels">下拉选择项个数和字符</param>
        /// <param name="defaultoutelement">外部html元素，默认div</param>
        /// <returns></returns>
        public static MvcHtmlString District(this HtmlHelper html, string name, object defaultCity = null, string optionLabels = "请选省,请选择市", string defaultOutElement = "div", string classname = "")
        {
            
            defaultCity = html.ViewData.ModelState.TryGetValue(name, defaultCity);
            string id = "dst" + name.GetHashCode();
            var hide = html.Hidden(name);
            var r = String.Format(@"<" + defaultOutElement + @" id=""{0}""></" + defaultOutElement + @">
                        <script type=""text/javascript"">
                            $(""#{0}"").District(""{1}"", ""{2}"", ""{3}"", """", ""{4}"");
                        </script>
            ", id, defaultCity, name, optionLabels, classname);

            return MvcHtmlString.Create(hide.ToHtmlString() + r);
        }


        /// <summary>
        /// 输出地区选择
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression">表达式</param>
        /// <param name="defaultCity">默认值</param>
        /// <param name="optionLabels">下拉选择项个数和字符</param>
        /// <returns></returns>
        public static MvcHtmlString DistrictFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string defaultCity = null, string optionLabels = "请选省,请选择市", string defaultOutElement = "div", string classname = "")
        {

           
           ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

           string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
           string id = "dst" + fullName.GetHashCode();
           var v = Convert.ToString(metadata.Model, CultureInfo.CurrentCulture);
           defaultCity = String.IsNullOrEmpty(v) ? defaultCity : v;
           return html.District(fullName, html.ViewData.Eval(fullName) ?? defaultCity, optionLabels, defaultOutElement, classname);
            //var d = GetEnumDataFromLambdaExpression(html, expression, "DisplayEnumFor");
        }
    }
}
