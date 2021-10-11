using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
namespace System.Web.Mvc
{
    /// <summary>
    /// HtmlHelper Enum扩展类,显示,列表等
    /// </summary>
    public static class DisplayEnumEx
    {
        /// <summary>
        /// 显示一个字段类型为EnumDataTypeAttribute的所有列表
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="optionLabel"></param>
        /// <param name="useEnumValue"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string optionLabel = null, bool useEnumValue = false, object htmlAttributes = null)
        {

            var d = ExpressionEx.GetEnumDataFromLambdaExpression(html, expression, "DropDownListEnumFor");
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var list = new SelectList(d.EnumType.ToList(), useEnumValue ? "EnumValue" : "key", "value", v.Model);

            return html.DropDownListFor(expression, list, optionLabel, htmlAttributes);
        }
        /// <summary>
        /// 显示一个字段类型为EnumDataTypeAttribute的所有列表，排除不显示的枚举值
        /// </summary>
        public static MvcHtmlString DropDownListEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, List<int> exceptList, string optionLabel = null, bool useEnumValue = false, object htmlAttributes = null)
        {

            var d = ExpressionEx.GetEnumDataFromLambdaExpression(html, expression, "DropDownListEnumFor");
            var eList = d.EnumType.ToList();
            if (exceptList != null && exceptList.Count > 0)
            {
                eList = eList.Where(p => !exceptList.Contains(useEnumValue ? p.EnumValue.ToInt() : p.Key.ToInt())).ToList();
            }
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var list = new SelectList(eList, useEnumValue ? "EnumValue" : "key", "value", v.Model);

            return html.DropDownListFor(expression, list, optionLabel, htmlAttributes);
        }
        /// <summary>
        /// 枚举类型的下拉选择
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">下拉名称</param>
        /// <param name="type">枚举类型</param>
        /// <param name="optionLable"></param>
        /// <param name="selected"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListEnum(this HtmlHelper html, string name, Type type, string optionLable = null, object selected = null, object htmlAttributes = null)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("该类型不是枚举类型");
            }
            var list = new SelectList(type.ToList().Select(p => new { value = p.EnumValue, name = p.Value }), "value", "name", selected);
            return html.DropDownList(name, list, optionLable, htmlAttributes);
        }
        public static MvcHtmlString DropDownListEnum(this HtmlHelper html, string name, Type type, List<int> exceptList = null, string optionLable = null, object selected = null, object htmlAttributes = null)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("该类型不是枚举类型");
            }
            var eList = type.ToList();
            if (exceptList != null && exceptList.Count > 0)
            {
                eList = eList.Where(p => !exceptList.Contains(p.EnumValue.ToInt())).ToList();
            }
            var list = new SelectList(eList.Select(p => new { value = p.EnumValue, name = p.Key }), "value", "name", selected);
            return html.DropDownList(name, list, optionLable, htmlAttributes);
        }
        /// <summary>
        /// 显示一个字段类型为EnumDataTypeAttribute的值对应的说明
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString DisplayEnumFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {

            var d = ExpressionEx.GetEnumDataFromLambdaExpression(html, expression, "DisplayEnumFor");
            return MvcHtmlString.Create(d.EnumType.GetDescription(d.Value));

        }

        public static MvcHtmlString RadioButtonForEnum<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool useEnumValue = true, object htmlAttributes = null)
        {
            var d = ExpressionEx.GetEnumDataFromLambdaExpression(html, expression, "RadioButtonForEnum");
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var name = v.PropertyName;
            var l = d.EnumType.ToList().Select(a => (html.RadioButtonFor(expression, useEnumValue ? a.EnumValue.ToInt().ToString() : a.Value, new { id = name + "_" + a.EnumValue.ToInt() }).ToHtmlString() + html.Label(name + "_" + a.EnumValue.ToInt(), a.Value).ToHtmlString()));
            return new MvcHtmlString(string.Join("", l));
        }

        /// <summary>
        /// 根据枚举属性生成radio button组
        /// </summary>
        /// <param name="html">HtmlHelper对象</param>
        /// <param name="expression"></param>
        /// <param name="displayValueType">Radio Button值的类型</param>
        /// <param name="htmlAttributes"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonForEnum<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, DisplayValueType displayValueType = DisplayValueType.Value, object htmlAttributes = null)
        {
            var enumDataType = ExpressionEx.GetEnumDataFromLambdaExpression(html, expression, "RadioButtonForEnum");
            var enumFields = enumDataType.EnumType.ToList();
            var stringBuilder = new StringBuilder();
            // 使用ASP.NET MVC内部生成ID的方法生成唯一的ID
            string tagName = ExpressionHelper.GetExpressionText(expression);
            string tagId =
                TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(tagName));
            foreach (var field in enumFields)
            {
                object value;
                switch (displayValueType)
                {
                    case DisplayValueType.Key:
                        value = field.Key;
                        break;
                    case DisplayValueType.Description:
                        value = field.Value;
                        break;
                    default:
                        value = field.EnumValue;
                        break;
                }

                stringBuilder.Append(
                    html.RadioButtonFor(expression, value, new { id = string.Concat(tagId, "_", field.EnumValue) })
                        .ToHtmlString());
                stringBuilder.Append(
                    html.Label(string.Concat(tagId, "_", field.EnumValue), field.Value)
                        .ToHtmlString());
            }
            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString RadioButtonList<TModel>(this HtmlHelper<TModel> html, string name, IDictionary<string, string> optionlist, object htmlAttributes = null)
        {
            return RadioButtonList<TModel>(html, name, optionlist, null, htmlAttributes);
        }

        public static MvcHtmlString RadioButtonList<TModel>(this HtmlHelper<TModel> html, string name, IDictionary<string, string> optionlist, object checkedValue, object htmlAttributes = null)
        {
            return RadioButtonList(html, name, optionlist, checkedValue, "", htmlAttributes);
        }

        public static MvcHtmlString RadioButtonList<TModel>(this HtmlHelper<TModel> html, string name, IDictionary<string, string> optionlist, object checkedValue, string template, object htmlAttributes = null)
        {
            var d = !String.IsNullOrEmpty(html.ViewData.ModelState.TryGetValue(name, "")) ? html.ViewData.ModelState.TryGetValue(name, "").Split(',').ToList() : new List<string>();

            if (d.Count == 0 && !d.Contains(""))
            {
                d.Add("");
            }

            var l = optionlist.Select(a => {
                var str = (html.RadioButton(name, a.Key, (d != null && d.Contains(a.Key)) || (checkedValue != null && a.Key.Equals(checkedValue.ToString(), StringComparison.InvariantCultureIgnoreCase)), new { id = name + "_" + a.Key }).ToHtmlString() + html.Label(name + "_" + a.Key, a.Value).ToHtmlString());
                if (!String.IsNullOrWhiteSpace(template))
                {
                    return string.Format(template, str);
                }
                else
                {
                    return str;
                }
            });
            return new MvcHtmlString(string.Join("", l));
        }

        public static MvcHtmlString RadioButtonForOptionParam<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var d = ExpressionEx.GetOptionParamDataFromLambdaExpression(html, expression, "RadioButtonForOptionParam");
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var name = v.PropertyName;
            var l = d.Select((a, b) => (html.RadioButtonFor(expression, a.Key, new { id = name + "_" + b.ToString() }).ToHtmlString() + html.Label(name + "_" + b.ToString(), a.Value).ToHtmlString()));
            return new MvcHtmlString(string.Join("", l));
        }

        public static MvcHtmlString CheckBoxButtonForOptionParam<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var d = ExpressionEx.GetOptionParamDataFromLambdaExpression(html, expression, "CheckBoxButtonForOptionParam");
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var name = v.PropertyName;

            List<string> vl = null;

            if (v.Model != null)
            {
                var e = v.Model as IEnumerable;
                if (e != null)
                {
                    vl = (e.Cast<object>()).Select(a => a.ToString()).ToList();
                }

                if (v.Model is string)
                {
                    vl = (v.Model as string).Split(',').ToList();
                }
            }

            var l = d.Select((a, b) => (string.Format("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\" value=\"{2}\" {3}/>", name, name + "_" + b.ToString(), a.Key, vl != null && vl.Contains(a.Key) ? " checked=\"checked\"" : "") + html.Label(name + "_" + b.ToString(), a.Value).ToHtmlString()));
            return new MvcHtmlString(string.Join("", l));
        }


        public static MvcHtmlString CheckBoxForEnum<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool useEnumValue = true, object htmlAttributes = null)
        {
            var d = ExpressionEx.GetEnumDataFromLambdaExpression(html, expression, "CheckBoxForEnum");
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var name = v.PropertyName;

            List<string> vl = null;

            if (v.Model != null)
            {
                var e = v.Model as IEnumerable;
                if (e != null)
                {
                    vl = (e.SafeCast<object>()).Select(a => a.ToString()).ToList();
                }
            }

            if (vl == null && v.Model is string)
            {
                vl = (v.Model as string).Split(',').ToList();
            }

            var l = d.EnumType.ToList().Select(a =>
            {
                object param = null;
                if (vl != null && vl.Contains(useEnumValue ? a.EnumValue.ToInt().ToString() : a.Value))
                {
                    param = new { id = name + "_" + a.EnumValue.ToInt(), @checked = "checked" };
                }
                else
                {
                    param = new { id = name + "_" + a.EnumValue.ToInt() };
                }
                return (html.RadioButtonFor(expression, useEnumValue ? a.EnumValue.ToInt().ToString() : a.Value, param).ToHtmlString().Replace("radio", "checkbox") + html.Label(name + "_" + a.EnumValue.ToInt(), a.Value).ToHtmlString());
            });
            return new MvcHtmlString(string.Join("", l));
        }


        public static MvcHtmlString CheckBoxListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, string> optionlist, object htmlAttributes = null)
        {
            return CheckBoxListFor(html, expression, optionlist, "<input type=\"checkbox\" name=\"{0}\" id=\"{1}\" value=\"{2}\" {3} />" + html.Label("{1}", "{4}").ToHtmlString(), htmlAttributes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="optionlist"></param>
        /// <param name="template">{0}:name,{1}:id,{2}:value,{3}:ischecked,{4}:labeltext</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, string> optionlist, string template, object htmlAttributes = null)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var name = v.PropertyName;

            if (!String.IsNullOrWhiteSpace(html.ViewData.TemplateInfo.HtmlFieldPrefix))
            {
                name = html.ViewData.TemplateInfo.HtmlFieldPrefix + "." + name;
            }

            List<string> vl = null;

            if (v.Model is string)
            {
                vl = (v.Model as string).ToSafeString().Split(',').Select(a => a.ToSafeString().Trim()).ToList();
            }
            else
            {
                if (v.Model != null)
                {
                    var e = v.Model as IEnumerable;
                    if (e != null)
                    {
                        vl = (e.SafeCast<object>()).Select(a => a.ToSafeString().Trim()).ToList();
                    }
                }
            }

            var l = optionlist.Select((a, b) =>
            {
                var value = a.Key.ToSafeString().Trim();
                return string.Format(template, name, name + "_" + b, value, (vl != null && vl.Contains(value) ? " checked=\"checked\" " : ""), a.Value);
            });
            return new MvcHtmlString(string.Join("", l));
        }

        public static MvcHtmlString CheckBoxList<TModel>(this HtmlHelper<TModel> html, string name, IDictionary<string, string> optionlist, object htmlAttributes = null)
        {
            return CheckBoxList<TModel>(html, name, optionlist, null, htmlAttributes);
        }

        public static MvcHtmlString CheckBoxList<TModel>(this HtmlHelper<TModel> html, string name, IDictionary<string, string> optionlist, IList<string> checkedList, object htmlAttributes = null)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                name = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            }
            var d = !String.IsNullOrEmpty(html.ViewData.ModelState.TryGetValue(name, "")) ? html.ViewData.ModelState.TryGetValue(name, "").Split(',').ToList() : new List<string>();

            var ext = html.ViewData["CheckBoxListLabelExt"].ToSafeString();

            var l = optionlist.Select((a, b) =>
            {
                return ("<input type=\"checkbox\" name=\"" + name + "\" id=\"" + name + "_" + ext + "_" + b + "\" value=\"" + a.Key + "\" " + (d != null && d.Contains(a.Key) || (checkedList != null && checkedList.Any(p => a.Key.Equals(p, StringComparison.InvariantCultureIgnoreCase))) ? " checked=\"checked\" " : "") + " />" + html.Label(ext + "_" + b.ToSafeString(), a.Value).ToHtmlString() + "&nbsp;&nbsp;");
            });

            return new MvcHtmlString(string.Join("", l));
        }

        /// <summary>
        /// 表示在前台展示的checkbox或者radiobutton的value值的类型
        /// </summary>
        public enum DisplayValueType
        {
            /// <summary>
            /// 枚举的值
            /// </summary>
            Value,
            /// <summary>
            /// 枚举值的名称
            /// </summary>
            Key,
            /// <summary>
            /// 枚举的描述(DescriptionAttribute)
            /// </summary>
            Description
        }
    }
}
