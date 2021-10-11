using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Text.RegularExpressions;
namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 根据字段上的Required属性 在字段名前显示 * 号
    /// </summary>
    public static class RequiredFor
    {
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat("", htmlHelper.LabelFor(expression, (object)null)));
            }
            return htmlHelper.LabelFor(expression, (object)null);
        }
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat("", htmlHelper.LabelFor(expression, (object)null)));
            }
            return htmlHelper.LabelFor(expression, (object)null);
        }
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, decimal>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat("", htmlHelper.LabelFor(expression, (object)null)));
            }
            return htmlHelper.LabelFor(expression, (object)null);
        }
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat("", htmlHelper.LabelFor(expression, (object)null)));
            }
            return htmlHelper.LabelFor(expression, (object)null);
        }



        private static Regex labelRegex = new Regex(@"<label(((?!>).)*)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, object htmlAttributes)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(labelRegex.Replace(htmlHelper.LabelFor(expression, null, htmlAttributes).ToSafeString(), "$0<b class=\"Required\">*</b>"));
            }
            return htmlHelper.LabelFor(expression, null, htmlAttributes);
        }
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, object htmlAttributes)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(labelRegex.Replace(htmlHelper.LabelFor(expression, null, htmlAttributes).ToSafeString(), "$0<b class=\"Required\">*</b>"));
            }
            return htmlHelper.LabelFor(expression, null, htmlAttributes);
        }
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, decimal>> expression, object htmlAttributes)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(labelRegex.Replace(htmlHelper.LabelFor(expression, null, htmlAttributes).ToSafeString(), "$0<b class=\"Required\">*</b>"));
            }
            return htmlHelper.LabelFor(expression, null, htmlAttributes);
        }
        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime>> expression, object htmlAttributes)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(labelRegex.Replace(htmlHelper.LabelFor(expression, null, htmlAttributes).ToSafeString(), "$0<b class=\"Required\">*</b>"));
            }
            return htmlHelper.LabelFor(expression, null, htmlAttributes);
        }

        /// <summary>
        /// 根据字段上的Required属性 在字段名前显示 * 号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int?>> expression, object htmlAttributes)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(labelRegex.Replace(htmlHelper.LabelFor(expression, null, htmlAttributes).ToSafeString(), "$0<b class=\"Required\">*</b>"));
            }
            return htmlHelper.LabelFor(expression, null, htmlAttributes);
        }


        /*
        public static MvcHtmlString TextBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, object htmlAttributes = null) 
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.TextBoxFor<TModel, string>(expression, new RouteValueDictionary(htmlAttributes)).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.TextBoxFor<TModel, string>(expression, new RouteValueDictionary(htmlAttributes));
        }
        public static MvcHtmlString TextBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, object htmlAttributes = null)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.TextBoxFor<TModel,int>(expression, new RouteValueDictionary(htmlAttributes)).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.TextBoxFor<TModel, int>(expression, new RouteValueDictionary(htmlAttributes));
        }


        public static MvcHtmlString DropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression,IEnumerable<SelectListItem> selectList=null,object htmlAttributes=null)
        {

            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.DropDownListFor<TModel,string>(expression,selectList,htmlAttributes), " <b style=\"color:red\">*</b>"));
            }
            return new MvcHtmlString(String.Concat(htmlHelper.DropDownListFor<TModel, string>(expression, selectList, htmlAttributes), " <b style=\"color:red\">*</b>"));
        }
        public static MvcHtmlString DropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, IEnumerable<SelectListItem> selectList = null,string label=null    , object htmlAttributes = null)
        {

            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.DropDownListFor<TModel, string>(expression, selectList, label, htmlAttributes), " <b style=\"color:red\">*</b>"));
            }
            return new MvcHtmlString(String.Concat(htmlHelper.DropDownListFor<TModel, string>(expression, selectList, htmlAttributes), " <b style=\"color:red\">*</b>"));
        }
        public static MvcHtmlString DropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, IEnumerable<SelectListItem> selectList = null, string label = null)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.DropDownListFor<TModel, int>(expression, selectList, label), " <b style=\"color:red\">*</b>"));
            }
            return new MvcHtmlString(String.Concat(htmlHelper.DropDownListFor<TModel, int>(expression, selectList, label), " <b style=\"color:red\">*</b>"));
        }


        public static MvcHtmlString DropDownListEnumFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, string optionLabel = null, object htmlAttributes = null)
        {

            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.DropDownListEnumFor<TModel, string>(expression, optionLabel, htmlAttributes), " <b style=\"color:red\">*</b>"));
            }
            return new MvcHtmlString(String.Concat(htmlHelper.DropDownListEnumFor<TModel, string>(expression, optionLabel, htmlAttributes), " <b style=\"color:red\">*</b>"));
        }

        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, object htmlAttributes = null)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.TextAreaFor<TModel, string>(expression, new RouteValueDictionary(htmlAttributes)).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.TextAreaFor<TModel, string>(expression, new RouteValueDictionary(htmlAttributes));
        }
        public static MvcHtmlString TextAreaFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, object htmlAttributes = null)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.TextAreaFor<TModel, int>(expression, new RouteValueDictionary(htmlAttributes)).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.TextAreaFor<TModel, int>(expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString EditorFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.EditorFor<TModel, string>(expression,null).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.EditorFor<TModel, string>(expression,null);
        }

        public static MvcHtmlString EditorFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.EditorFor<TModel, int>(expression,null).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.EditorFor<TModel, int>(expression,null);
        }

        public static MvcHtmlString EditorFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, decimal>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.EditorFor<TModel, decimal>(expression,null).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.EditorFor<TModel, decimal>(expression, null);
        }
        public static MvcHtmlString EditorFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime>> expression)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (v.IsRequired)
            {
                return new MvcHtmlString(String.Concat(htmlHelper.EditorFor<TModel, DateTime>(expression, null).ToHtmlString(), " <b style=\"color:red\">*</b>"));
            }
            return htmlHelper.EditorFor<TModel, DateTime>(expression, null);
        }*/
    }
}
