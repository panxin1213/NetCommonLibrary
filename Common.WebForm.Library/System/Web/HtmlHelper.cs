using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Pager;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Drawing;
using System.Web.UI;

namespace System.Web
{
    public class Html
    {
        public ModelState MState = null;

        public string ValidErrorString { get; set; }

    }

    public static class HtmlTool
    {
        /// <summary>
        /// 给html元素添加ID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static IDictionary<string, object> AddId(string name, IDictionary<string, object> htmlAttributes = null)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }
            if (htmlAttributes.Any(a => a.Key.Equals("id", StringComparison.OrdinalIgnoreCase)))
            {
                return htmlAttributes;
            }
            htmlAttributes.Add(new KeyValuePair<string, object>("id", name));

            return htmlAttributes;
        }

        /// <summary>
        /// 铜鼓html元素属性字典生成属性字符串
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static string GetAttributeString(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                return string.Empty;
            }

            var attr = "";

            if (htmlAttributes != null)
            {
                foreach (var item in htmlAttributes)
                {
                    attr += " " + item.Key + "=\"" + (item.Value != null ? item.Value.ToString() : "") + "\" ";
                }
            }

            return attr;
        }

        /// <summary>
        /// input type='text'元素字符串
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">值</param>
        /// <param name="htmlAttributes">属性字典</param>
        /// <returns></returns>
        public static string TextBox(string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            var v = value == null ? HttpContext.Current.Request.QueryString[name] : value.ToString();
            htmlAttributes = AddId(name, htmlAttributes);

            return string.Format("<input type=\"text\" name=\"{0}\" value=\"{2}\" {1} />", name, GetAttributeString(htmlAttributes), v);
        }

        /// <summary>
        /// input type='password'元素字符串
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">值</param>
        /// <param name="htmlAttributes">属性字典</param>
        /// <returns></returns>
        public static string Password(string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            var v = value == null ? HttpContext.Current.Request.QueryString[name] : value.ToString();
            htmlAttributes = AddId(name, htmlAttributes);

            return string.Format("<input type=\"password\" name=\"{0}\" value=\"{2}\" {1} />", name, GetAttributeString(htmlAttributes), v);
        }

        /// <summary>
        /// input type='hidden'元素字符串
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">值</param>
        /// <param name="htmlAttributes">属性字典</param>
        /// <returns></returns>
        public static string Hidden(string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            htmlAttributes = AddId(name, htmlAttributes);

            return string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" {2}/>", name, value, GetAttributeString(htmlAttributes));
        }

        /// <summary>
        /// input type='radio'元素字符串
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">值</param>
        /// <param name="isChecked">是否选中</param>
        /// <param name="htmlAttributes">属性字典</param>
        /// <returns></returns>
        public static string RadioButton(string name, object value = null, bool? isChecked = null, IDictionary<string, object> htmlAttributes = null)
        {
            if (isChecked == null && HttpContext.Current.Request.QueryString[name] == value)
            {
                isChecked = true;
            }

            return string.Format("<input type=\"radio\" name=\"{0}\" value=\"{1}\" {2} {3} />", name, value, GetAttributeString(htmlAttributes), isChecked != null && (bool)isChecked ? " checked=\"checked\" " : "");
        }

        /// <summary>
        /// 错误信息元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string ValidationMessage(string name, string value = "", IDictionary<string, object> htmlAttributes = null)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }
            if (htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes["class"] = htmlAttributes["class"] + " field-validation-valid help-block";
            }
            else
            {
                htmlAttributes.Add("class", "field-validation-valid");
            }
            htmlAttributes.Add("data-valmsg-for", name);
            htmlAttributes.Add("data-valmsg-replace", "true");

            return "<span " + GetAttributeString(htmlAttributes) + ">" + value + "</span>";
        }

        /// <summary>
        /// input type='checkbox'元素字符串
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">值</param>
        /// <param name="ischeck">是否选中</param>
        /// <param name="haslabel">是否默认label元素</param>
        /// <param name="text">显示值</param>
        /// <param name="htmlAttributes">属性字典</param>
        /// <returns></returns>
        public static string CheckBox(string name, bool? ischeck = null, object value = null, IDictionary<string, object> htmlAttributes = null, string text = null, bool haslabel = true)
        {
            string v = value == null ? null : value.ToString();

            if (v == null)
            {
                v = "true";
            }

            if (ischeck == null)
            {
                if (HttpContext.Current.Request.QueryString[name] != null && HttpContext.Current.Request.QueryString[name].StartsWith(v, StringComparison.OrdinalIgnoreCase))
                {
                    ischeck = true;
                }
            }
            if (String.IsNullOrWhiteSpace(text))
            {
                text = v;
            }

            if (text.Equals("true", StringComparison.OrdinalIgnoreCase) || text.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                text = "";
            }

            var idvalue = name.Replace(".", "_");

            if (htmlAttributes != null && htmlAttributes.Any(a => a.Key.Equals("id", StringComparison.OrdinalIgnoreCase)))
            {
                var idobj = htmlAttributes.FirstOrDefault(a => a.Key.Equals("id", StringComparison.OrdinalIgnoreCase));
                idvalue = idobj.Value.ToString();
                htmlAttributes.Remove(idobj);
            }

            if (haslabel)
            {
                return string.Format("<input type=\"checkbox\" name=\"{0}\" id=\"{5}\" {3} value=\"{2}\" {1} /><label for=\"{5}\">{6}</label>{4}", name, GetAttributeString(htmlAttributes), v.ToLower() == "true" || v.ToLower() == "false" ? "true" : v, ischeck != null && (bool)ischeck ? " checked=\"checked\" " : "", v.ToLower() == "true" || v.ToLower() == "false" ? string.Format("<input type=\"hidden\" name=\"{0}\" value=\"false\"/>", name) : "", idvalue, text);
            }
            else
            {
                return string.Format("<input type=\"checkbox\" name=\"{0}\" id=\"{4}\" {3} value=\"{2}\" {1} />", name, GetAttributeString(htmlAttributes), v.ToLower() == "true" || v.ToLower() == "false" ? "true" : v, ischeck != null && (bool)ischeck ? " checked=\"checked\" " : "", idvalue);
            }
        }

        /// <summary>
        /// textarea元素字符串
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">值</param>
        /// <param name="htmlAttributes">属性字典</param>
        /// <returns></returns>
        public static string TextArea(string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            var v = value == null ? HttpContext.Current.Request[name] : value.ToString();

            htmlAttributes = AddId(name, htmlAttributes);

            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object> { { "cols", "20" }, { "rows", "2" } };
            }
            else
            {
                if (!htmlAttributes.Any(a => a.Key.Equals("cols", StringComparison.OrdinalIgnoreCase)))
                {
                    htmlAttributes.Add("cols", "20");
                }
                if (!htmlAttributes.Any(a => a.Key.Equals("rows", StringComparison.OrdinalIgnoreCase)))
                {
                    htmlAttributes.Add("rows", "2");
                }
            }

            return string.Format("<textarea name=\"{0}\" {1}>{2}</textarea>", name, GetAttributeString(htmlAttributes), v);
        }

        /// <summary>
        /// select元素
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="options">option集合</param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultoption">默认option</param>
        /// <returns></returns>
        public static string DropDownList(string name, IEnumerable<OptionModel> options = null, IDictionary<string, object> htmlAttributes = null, string defaultoption = "-请选择-")
        {
            var v = HttpContext.Current.Request.QueryString[name];
            if (v == null)
            {
                v = string.Empty;
            }
            htmlAttributes = AddId(name, htmlAttributes);
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("<select name=\"{0}\" {1}>" + (!String.IsNullOrEmpty(defaultoption) ? "<option value=\"\">" + defaultoption + "</option>" : ""), name, GetAttributeString(htmlAttributes)));

            foreach (var item in options)
            {
                var st = item.Selected ? "selected" : "";
                var da = item.Disabled ? "disabled" : "";

                sb.AppendLine(string.Format("<option value=\"{0}\" {2}>{1}</option>", item.Value, item.Text, st + " " + da));
            }
            sb.AppendLine("</select>");

            return sb.ToString();
        }

        /// <summary>
        /// 输出地区选择
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">表单名</param>
        /// <param name="defaultCity">默认值</param>
        /// <param name="optionLabels">下拉选择项个数和字符</param>
        /// <param name="defaultoutelement">外部html元素，默认div</param>
        /// <returns></returns>
        public static string District(string name, object defaultCity = null, string optionLabels = "请选省,请选择市", string defaultOutElement = "div", IDictionary<string, object> htmlAttributes = null)
        {
            defaultCity = defaultCity == null ? HttpContext.Current.Request.QueryString[name] : defaultCity.ToString();
            string id = "dst" + name.GetHashCode();
            var hide = Hidden(name, htmlAttributes: htmlAttributes);
            var r = String.Format(@"<" + defaultOutElement + @" id=""{0}"" />
                        <script type=""text/javascript"">
                            $(""#{0}"").District(""{1}"", ""{2}"", ""{3}"");
                        </script>
            ", id, defaultCity, name, optionLabels);

            return hide + r;
        }

        /// <summary>
        /// img 缩略图元素
        /// </summary>
        /// <param name="filepath">文件地址</param>
        /// <param name="nopicfile">无图显示图</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="mode">等比处理模式</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string ImageThumb(string filepath, string nopicfile = "/css/images/icons/noimg.jpg", int width = 0, int height = 0, ThumbMode mode = Drawing.ThumbMode.Cut, IDictionary<string, object> htmlAttributes = null)
        {
            if (String.IsNullOrEmpty(filepath))
            {
                filepath = nopicfile;
            }
            else
            {
                var d = new Dictionary<ThumbMode, string>() { { ThumbMode.Cut, "c" }, { ThumbMode.Height, "h" }, { ThumbMode.Width, "w" }, { ThumbMode.WidthAndHeight, "a" }, { ThumbMode.WidthOrHeight, "o" } };
                filepath = "/u/" + width + "-" + height + "-" + d[mode] + filepath;
            }
            htmlAttributes = (htmlAttributes == null ? new Dictionary<string, object>() : htmlAttributes);
            htmlAttributes.Add(new KeyValuePair<string, object>("onerror", String.Format("this.src='{0}';this.onerror=null", nopicfile)));
            if (width != 0 && !htmlAttributes.ContainsKey("width"))
            {
                htmlAttributes.Add("width", width);
            }
            if (height != 0 && !htmlAttributes.ContainsKey("height"))
            {
                htmlAttributes.Add("height", height);
            }
            return string.Format("<img src=\"{0}\" {1}/>", filepath, GetAttributeString(htmlAttributes));
        }

        public static readonly string Hidden_Name = "_sm_back_url_";
        
        /// <summary>
        /// 生成修改添加时的返回按钮
        /// 会生成两个表单对象 :
        /// 一个hidden 用于维护 “修改/添加”前的地址 get 后取 UrlReferrer,POST后取hidden提交的值
        /// 一个button用于直接跳转到之前的地址
        /// </summary>
        /// <param name="h"></param>
        /// <param name="urldefalt"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        public static string FormBackButton(string urldefalt = null, string text = "返回", IDictionary<string, object> htmlAttributes = null, string backUrl = null)
        {
            var referrer = System.Web.HttpContext.Current.Request.UrlReferrer;
            backUrl = backUrl ?? (referrer != null ? referrer.ToString() : "");

            var sb = new StringBuilder();
            if ("POST".Equals(HttpContext.Current.Request.HttpMethod))
            {
                var re = System.Web.HttpContext.Current.Request[Hidden_Name];
                if (re != null)
                    backUrl = re.ToString();
            }
            backUrl = String.IsNullOrEmpty(backUrl) ? urldefalt : backUrl;

            //hidden
            sb.Append("<input type=\"hidden\" id=\"" + Hidden_Name + "\" name=\"" + Hidden_Name + "\" value=\"" + backUrl + "\"/>");

            var attr = "";

            if (htmlAttributes != null)
            {
                foreach (var item in htmlAttributes)
                {
                    attr += " " + item.Key + "=\"" + (item.Value != null ? item.Value.ToString() : "") + "\" ";
                }
            }
            //input
            sb.AppendFormat("<input type=\"button\" value=\"{0}\" {1} onclick=\"location=document.getElementById('{2}').value\"/>", text, attr, Hidden_Name);

            return sb.ToString();
        }

        public static string ShowIsLock(bool islock, string param, string changurl = "changelock.ashx")
        {
            var url = changurl + param;

            return string.Format("<a href='{0}' title='{1}'>", url, !islock ? "锁定信息" : "显示信息") + (!islock ? "<font color='green'>显示</font>" : "<font color='red'>不显示</font>") + "</a>";
        }

        public static string Label(string name, bool autorequired = true, IDictionary<string, object> htmlAttributes = null, string des = null)
        {
            return string.Format("{2}<label for=\"{0}\" {3}>{1}</label>", name, des ?? name, autorequired ? "<b class=\"Required\">*</b>" : "", GetAttributeString(htmlAttributes));
        }

        public static string ValidationSummary()
        {
            return @"<div class=""validation-summary-valid"" data-valmsg-summary=""true""><ul><li style=""display:none""></li></ul></div>";
        }
    }

    public static class HtmlEx
    {
        public static string TextBox(this Html html, string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.TextBox(name, value, htmlAttributes);
        }

        public static string Password(this Html html, string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.Password(name, value, htmlAttributes);
        }

        public static string Hidden(this Html html, string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.Hidden(name, value, htmlAttributes);
        }

        public static string RadioButton(this Html html, string name, object value = null, bool? isChecked = null, IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.RadioButton(name, value, isChecked, htmlAttributes);
        }

        public static string ValidationMessage(this Html html, string name, string value = "", IDictionary<string, object> htmlAttributes = null)
        {
            if (html.MState != null && html.MState.Errors.ContainsKey(name))
            {
                value = html.MState.Errors[name];
            }

            return HtmlTool.ValidationMessage(name, value, htmlAttributes);
        }

        public static string CheckBox(this Html html, string name, bool? ischeck = null, object value = null, IDictionary<string, object> htmlAttributes = null, string text = null, bool haslabel = true)
        {
            return HtmlTool.CheckBox(name, ischeck, value, htmlAttributes, text, haslabel);
        }

        public static string TextArea(this Html html, string name, object value = null, IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.TextArea(name, value, htmlAttributes);
        }

        public static string DropDownList(this Html html, string name, IDictionary<string, string> optionlist, object value = null, IDictionary<string, object> htmlAttributes = null, IEnumerable<object> selectedlist = null, string defaultoption = "-请选择-")
        {
            var v = value == null ? HttpContext.Current.Request.QueryString[name] : value.ToString();
            var options = optionlist.Select(a => new OptionModel(a.Key, a.Value)).ToList();
            options.ForEach(a =>
            {
                if ((v != null && a.Value.Equals(v, StringComparison.OrdinalIgnoreCase)) || (selectedlist != null && selectedlist.Any(b => b != null && b.ToString().Equals(a.Value, StringComparison.OrdinalIgnoreCase))))
                {
                    a.Selected = true;
                }
            });


            return HtmlTool.DropDownList(name, options, htmlAttributes, defaultoption);
        }

        /// <summary>
        /// 输出地区选择
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">表单名</param>
        /// <param name="defaultCity">默认值</param>
        /// <param name="optionLabels">下拉选择项个数和字符</param>
        /// <param name="defaultoutelement">外部html元素，默认div</param>
        /// <returns></returns>
        public static string District(this Html html, string name, object defaultCity = null, string optionLabels = "请选省,请选择市", string defaultOutElement = "div", IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.District(name, defaultCity, optionLabels, defaultOutElement, htmlAttributes);
        }

        public static string ImageThumb(this Html html, string filepath, string nopicfile = "/css/images/icons/noimg.jpg", int width = 0, int height = 0, ThumbMode mode = Drawing.ThumbMode.Cut, IDictionary<string, object> htmlAttributes = null)
        {
            return HtmlTool.ImageThumb(filepath, nopicfile, width, height, mode, htmlAttributes);
        }

        /// <summary>
        /// 生成修改添加时的返回按钮
        /// 会生成两个表单对象 :
        /// 一个hidden 用于维护 “修改/添加”前的地址 get 后取 UrlReferrer,POST后取hidden提交的值
        /// 一个button用于直接跳转到之前的地址
        /// </summary>
        /// <param name="h"></param>
        /// <param name="urldefalt"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        public static string FormBackButton(this Html html, string urldefalt = null, string text = "返回", IDictionary<string, object> htmlAttributes = null, string backUrl = null)
        {
            return HtmlTool.FormBackButton(urldefalt, text, htmlAttributes, backUrl);
        }

        public static string ShowIsLock(this Html html, bool islock, string param, string changurl = "changelock.ashx")
        {
            return HtmlTool.ShowIsLock(islock, param, changurl);
        }
    }

    public class HtmlHelper<T> : Html where T : class
    {
        public T Model = null;

        public HtmlHelper(T model)
        {
            Model = model;
        }

        public string LabelFor(string name, bool autorequired = true, IDictionary<string, object> htmlAttributes = null)
        {
            var type = typeof(T);

            var p = type.GetProperty(name);

            if (p == null)
            {
                throw new Exception(type.FullName + "中没有字段:" + name);
            }

            var isrequired = !p.PropertyType.FullName.Equals(typeof(bool).FullName) && (type.GetProperty(name).GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute) != null;

            return HtmlTool.Label(name, isrequired && autorequired, htmlAttributes, p != null ? (p.GetDescription(type)) : name);
        }

        public string TextBoxFor(string name, IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            htmlAttributes = GetRangeValidateString(name, htmlAttributes);
            htmlAttributes = GetStringMaxLengthValidate(name, htmlAttributes);
            htmlAttributes = GetRemoteValidate(name, htmlAttributes);
            htmlAttributes = GetRegularExpressionValidate(name, htmlAttributes);
            htmlAttributes = GetEqualToValidate(name, htmlAttributes);

            return HtmlEx.TextBox(this, name, String.IsNullOrEmpty(value) ? null : value, htmlAttributes);
        }

        public string PasswordFor(string name, IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            htmlAttributes = GetRangeValidateString(name, htmlAttributes);
            htmlAttributes = GetStringMaxLengthValidate(name, htmlAttributes);
            htmlAttributes = GetRemoteValidate(name, htmlAttributes);
            htmlAttributes = GetRegularExpressionValidate(name, htmlAttributes);
            htmlAttributes = GetEqualToValidate(name, htmlAttributes);

            return HtmlEx.Password(this, name, String.IsNullOrEmpty(value) ? null : value, htmlAttributes);
        }

        public string CheckBoxFor(string name, IDictionary<string, object> htmlAttributes = null, bool haslabel = true)
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }
            return HtmlEx.CheckBox(this, name, value.ToLower().Equals("true"), String.IsNullOrEmpty(value) ? null : value, htmlAttributes, haslabel: haslabel);
        }

        public string TextAreaFor(string name, IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            htmlAttributes = GetRangeValidateString(name, htmlAttributes);
            htmlAttributes = GetStringMaxLengthValidate(name, htmlAttributes);
            htmlAttributes = GetRegularExpressionValidate(name, htmlAttributes);

            return HtmlEx.TextArea(this, name, String.IsNullOrEmpty(value) ? null : value, htmlAttributes);
        }

        public string HiddenFor(string name, IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            htmlAttributes = GetRangeValidateString(name, htmlAttributes);
            htmlAttributes = GetStringMaxLengthValidate(name, htmlAttributes);
            htmlAttributes = GetRemoteValidate(name, htmlAttributes);
            htmlAttributes = GetRegularExpressionValidate(name, htmlAttributes);
            htmlAttributes = GetEqualToValidate(name, htmlAttributes);

            return HtmlEx.Hidden(this, name, String.IsNullOrEmpty(value) ? null : value, htmlAttributes);
        }

        public string DistrictFor(string name, string optionLabels = "请选省,请选择市", IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            htmlAttributes = GetRangeValidateString(name, htmlAttributes);

            return HtmlEx.District(this, name, String.IsNullOrEmpty(value) ? null : value, optionLabels: optionLabels, htmlAttributes: htmlAttributes);
        }

        public string DropDownListFor(string name, IDictionary<string, string> optionlist, IDictionary<string, object> htmlAttributes = null, IEnumerable<object> selectedlist = null, string defaultoption = "-请选择-")
        {
            var value = "";
            if (Model != null)
            {
                var p = Model.GetType().GetProperty(name);
                if (p != null)
                {
                    var o = p.GetValue(Model, null);
                    value = o != null ? o.ToString() : "";
                }
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            htmlAttributes = GetRangeValidateString(name, htmlAttributes);

            return HtmlEx.DropDownList(this, name, optionlist, value, htmlAttributes, selectedlist, defaultoption);
        }

        public string DropDownListEnumFor(string name, string optionlabel = null, bool userEnumValue = false, IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            var type = typeof(T);
            PropertyInfo p = type.GetProperty(name);
            if (Model != null)
            {
                var o = p.GetValue(Model, null);
                value = o != null ? o.ToString() : "";
            }

            htmlAttributes = GetRequiredValidateString(name, htmlAttributes);
            var optionlist = new Dictionary<string, string>();
            if (optionlabel != null)
            {
                optionlist.Add("", optionlabel);
            }
            if (p != null)
            {
                var attr = p.GetValidAttribute<EnumDataTypeAttribute>(type);
                if (attr != null && attr.Count > 0)
                {
                    var l = attr[0].EnumType.ToList();
                    if (l != null && l.Count > 0)
                    {
                        foreach (var item in l)
                        {
                            optionlist.Add(userEnumValue ? item.EnumValue.ToString() : item.Key.ToString(), item.Key.ToString());
                        }
                    }
                }
            }

            return HtmlEx.DropDownList(this, name, optionlist, value, htmlAttributes);
        }

        public string ImageThumbFor(string name, string nopicfile = "/css/images/icons/noimg.jpg", int width = 0, int height = 0, ThumbMode mode = Drawing.ThumbMode.Cut, IDictionary<string, object> htmlAttributes = null)
        {
            var value = "";
            var type = typeof(T);
            PropertyInfo p = type.GetProperty(name);
            if (Model != null)
            {
                var o = p.GetValue(Model, null);
                value = o != null ? o.ToString() : "";
            }
            return HtmlEx.ImageThumb(this, value, nopicfile, width, height, mode, htmlAttributes);
        }

        #region Valid

        private IDictionary<string, object> CommonAddAttribute(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }
            if (!htmlAttributes.ContainsKey("data-val"))
            {
                htmlAttributes.Add("data-val", "true");
            }
            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "valid");
            }
            else
            {
                if (htmlAttributes["class"] == null || htmlAttributes["class"].ToString().IndexOf("valid", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    htmlAttributes["class"] = htmlAttributes["class"] + " " + " valid";
                }
            }

            return htmlAttributes;
        }

        /// <summary>
        /// 判断是否是必须
        /// </summary>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetRequiredValidateString(string name, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(T);
            var p = type.GetProperty(name);
            if (p == null)
            {
                throw new Exception("没有字段\"" + name + "\"");
            }
            var required = p.GetValidAttribute<RequiredAttribute>(type);

            if (required != null && required.Count > 0)
            {
                htmlAttributes = CommonAddAttribute(htmlAttributes);

                if (!htmlAttributes.ContainsKey("data-val-required"))
                {
                    htmlAttributes.Add("data-val-required", String.IsNullOrEmpty(required[0].ErrorMessage) ? required[0].FormatErrorMessage(p.GetDescription(type)) : required[0].ErrorMessage);
                }
            }
            return htmlAttributes;

        }

        /// <summary>
        /// 判断最大长度验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetStringMaxLengthValidate(string name, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(T);
            var p = type.GetProperty(name);
            var required = p.GetValidAttribute<StringLengthAttribute>(type);

            if (required != null && required.Count > 0)
            {
                htmlAttributes = CommonAddAttribute(htmlAttributes);

                if (!htmlAttributes.ContainsKey("data-val-length"))
                {
                    htmlAttributes.Add("data-val-length", String.IsNullOrEmpty(required[0].ErrorMessage) ? required[0].FormatErrorMessage(p.GetDescription(type)) : required[0].ErrorMessage);
                }
                if (!htmlAttributes.ContainsKey("data-val-length-max"))
                {
                    htmlAttributes.Add("data-val-length-max", required[0].MaximumLength);
                }
            }
            return htmlAttributes;

        }

        /// <summary>
        /// Ajax验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetRemoteValidate(string name, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(T);
            var required = type.GetProperty(name).GetValidAttribute<RemoteAttribute>(type);

            if (required != null && required.Count > 0)
            {
                htmlAttributes = CommonAddAttribute(htmlAttributes);

                if (!htmlAttributes.ContainsKey("data-val-remote"))
                {
                    htmlAttributes.Add("data-val-remote", required[0].ErrorMessage);
                }
                if (!htmlAttributes.ContainsKey("data-val-remote-additionalfields"))
                {
                    htmlAttributes.Add("data-val-remote-additionalfields", !String.IsNullOrEmpty(required[0].AdditionalFields) ? ("*." + required[0].AdditionalFields.Replace(",", ",*.")) : "");
                }
                if (!htmlAttributes.ContainsKey("data-val-remote-url"))
                {
                    htmlAttributes.Add("data-val-remote-url", required[0].Url);
                }
                if (!htmlAttributes.ContainsKey("data-val-remote-type"))
                {
                    htmlAttributes.Add("data-val-remote-type", required[0].HttpMethod);
                }
            }
            return htmlAttributes;
        }

        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetRegularExpressionValidate(string name, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(T);
            var required = type.GetProperty(name).GetValidAttribute<RegularExpressionAttribute>(type);

            if (required != null && required.Count > 0)
            {
                htmlAttributes = CommonAddAttribute(htmlAttributes);

                if (!htmlAttributes.ContainsKey("data-val-regex"))
                {
                    htmlAttributes.Add("data-val-regex", required[0].ErrorMessage);
                }
                if (!htmlAttributes.ContainsKey("data-val-regex-pattern"))
                {
                    htmlAttributes.Add("data-val-regex-pattern", required[0].Pattern);
                }
            }
            return htmlAttributes;
        }

        /// <summary>
        /// 对比验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetEqualToValidate(string name, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(T);
            var required = type.GetProperty(name).GetValidAttribute<EqualToAttribute>(type);

            if (required != null && required.Count > 0)
            {
                htmlAttributes = CommonAddAttribute(htmlAttributes);

                if (!htmlAttributes.ContainsKey("data-val-equalto"))
                {
                    htmlAttributes.Add("data-val-equalto", required[0].ErrorMessage);
                }
                if (!htmlAttributes.ContainsKey("data-val-equalto-other"))
                {
                    htmlAttributes.Add("data-val-equalto-other", "*." + required[0].EqualToField);
                }
            }
            return htmlAttributes;
        }

        private IDictionary<string, object> GetRangeValidateString(string name, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(T);
            var required = type.GetProperty(name).GetValidAttribute<RangeAttribute>(type);

            if (required != null && required.Count > 0)
            {
                htmlAttributes = CommonAddAttribute(htmlAttributes);

                if (!htmlAttributes.ContainsKey("data-val-range"))
                {
                    htmlAttributes.Add("data-val-range", required[0].ErrorMessage);
                }
                if (!htmlAttributes.ContainsKey("data-val-range-max"))
                {
                    htmlAttributes.Add("data-val-range-max", required[0].Maximum);
                }
                if (!htmlAttributes.ContainsKey("data-val-range-min"))
                {
                    htmlAttributes.Add("data-val-range-min", required[0].Minimum);
                }
            }
            return htmlAttributes;

        }

        #endregion

        #region Page

        /// <summary>
        /// 翻页链接生成，直接取Model里的数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="size">当前页左右各调多少个翻页链接</param>
        /// <param name="dataEx">
        ///     用于生成页码提示和数量提示的lambda表达式
        ///     默认 a => String.Format("<br />第 {0}/{1} 页 , 共 {2} 条, 每页{3}条", d.Page, d.TotalPageCount, d.TotalCount, d.PageSize);
        /// </param>
        /// <param name="htmlAttributes">每个链接的html属性</param>
        /// <returns></returns>
        public string PageBar(int size, Func<IPagerDataAndQuery, string> dataEx = null, IDictionary<string, object> htmlAttributes = null, IEnumerable<string> exceptQuery = null, bool isstatic = false)
        {
            var data = Model as IPagerDataAndQuery;

            return PageBar(data, dataEx, htmlAttributes, size, exceptQuery, isstatic);
        }


        /// <summary>
        /// 翻页链接生成，直接取Model里的数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="dataEx">
        ///     用于生成页码提示和数量提示的lambda表达式
        ///     默认 d => String.Format("<br />第 {0}/{1} 页 , 共 {2} 条, 每页{3}条", d.Page, d.TotalPageCount, d.TotalCount, d.PageSize);
        /// </param>
        /// <param name="size">当前页左右各调多少个翻页链接</param>
        /// <param name="htmlAttributes">每个链接的html属性</param>
        /// <param name="isnew">新版后台样式</param>
        /// <returns></returns>
        public string PageBar(Func<IPagerDataAndQuery, string> dataEx = null, int size = 4, IDictionary<string, object> htmlAttributes = null, IEnumerable<string> exceptQuery = null, bool isstatic = false, bool isnew = false, string templatekey = null)
        {
            var data = Model as IPagerDataAndQuery;
            return PageBar(data, dataEx, htmlAttributes, size, exceptQuery, isstatic, isnew, templatekey);
        }


        /// <summary>
        /// 生成翻页链接，指定数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="d">传入的Common.Library.Pager.PagerDataModel对象</param>
        /// <param name="htmlAttributes">每个链接的html属性</param>
        /// <param name="size">当前页左右各调多少个翻页链接</param>
        /// <param name="dataEx">
        ///     用于生成页码提示和数量提示的lambda表达式
        ///     默认 a => String.Format("<br />第 {0}/{1} 页 , 共 {2} 条, 每页{3}条", d.Page, d.TotalPageCount, d.TotalCount, d.PageSize);
        /// </param>
        /// <returns></returns>
        public string PageBar(object d, Func<IPagerDataAndQuery, string> dataEx = null, int size = 4, IDictionary<string, object> htmlAttributes = null, IEnumerable<string> exceptQuery = null, bool isstatic = false)
        {
            IPagerDataAndQuery data;
            if (d == null)
                data = Model as IPagerDataAndQuery;
            else
                data = d as IPagerDataAndQuery;
            return PageBar(data, dataEx, htmlAttributes, size, exceptQuery, isstatic);
        }



        /// <summary>
        /// 生成翻页链接，指定数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="d">传入的Common.Library.Pager.IPagerDataAndQuery 对象</param>
        /// <param name="htmlAttributes">每个链接的html属性</param>
        /// <param name="size">当前页左右各调多少个翻页链接</param>
        /// <param name="dataEx">
        ///     用于生成页码提示和数量提示的lambda表达式
        ///     默认 a => String.Format("<br />第 {0}/{1} 页 , 共 {2} 条, 每页{3}条", d.Page, d.TotalPageCount, d.TotalCount, d.PageSize);
        /// </param>
        /// <param name="exceptQuery">需要排除显示的QueryString</param>
        /// <returns></returns>
        public string PageBar(IPagerDataAndQuery d, Func<IPagerDataAndQuery, string> dataEx = null, IDictionary<string, object> htmlAttributes = null, int size = 4, IEnumerable<string> exceptQuery = null, bool isstatic = false, bool isnew = false, string templatekey = null)
        {
            htmlAttributes = htmlAttributes == null ? new Dictionary<string, object>() : htmlAttributes;
            var data = d as IPagerDataAndQuery;
            if (data == null) return null;
            if (dataEx == null) dataEx = a => String.Format("<br />第 {0}/{1} 页 , 共 {2} 条, 每页{3}条", a.Page, a.TotalPageCount, a.TotalCount, a.PageSize);

            var ru = GetRoute(data.QueryString, 0);
            if (exceptQuery != null)
            {
                var x = ru.Except(ru.Where(a => exceptQuery.Any(b => (b.ToLower() == a.Key.ToLower() || (a.Key.IndexOf(b + "[", StringComparison.OrdinalIgnoreCase) == 0 && a.Key.LastIndexOf("]") == a.Key.Length - 1)))));
                ru = x.ToDictionary(a => a.Key, a => a.Value);
            }
            StringBuilder sb = new StringBuilder();
            int min = data.Page - size;
            int max = data.Page + size;

            var url = HttpContext.Current.Request.Url.AbsolutePath;
            var result = false;
            var staticindex = "index";
            var exl = ".html";
            if (isstatic && HttpContext.Current.Request.RawUrl.IndexOf(url) == -1)
            {
                result = true;
                url = HttpContext.Current.Request.RawUrl.Replace("?make", "");
                if (url.IndexOf(exl, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    staticindex = url.Substring(url.LastIndexOf("/") + 1).Replace(exl, "");
                    if (staticindex.IndexOf("_") > -1)
                    {
                        staticindex = staticindex.Substring(0, staticindex.IndexOf("_"));
                    }
                    url = url.Substring(0, url.LastIndexOf("/"));
                }
            }
            ru = ru.Where(a => !String.IsNullOrWhiteSpace(a.Value.ToSafeString())).ToDictionary(a => a.Key, a => a.Value);

            if (!isnew)
            {
                if (data.Page > 1)
                {
                    ru["page"] = 1;
                    sb.AppendFormat("<a href=\"{0}\" {1} target=\"_self\">首页</a>", result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                    ru["page"] = data.Page - 1;
                    sb.AppendFormat("<a href=\"{0}\" {1} target=\"_self\">上一页</a>", result ? (ru["page"].ToInt() > 1 ? "/" + url.Trim('/') + "/" + staticindex + "_" + ru["page"] + exl : (url + "/" + staticindex + exl)) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                }
                if (data.Page > (size + 1))
                {
                    sb.Append("<span>...</span>");
                }
                for (int i = min; i <= data.TotalPageCount && i <= max; i++)
                {
                    if (i > 0)
                    {
                        if (i == data.Page)
                        {
                            ru["page"] = i;
                            sb.AppendFormat("<a class=\"number current cur\">{0}</a>", i);
                        }
                        else
                        {
                            ru["page"] = i;
                            sb.AppendFormat("<a href=\"{0}\" {1} target=\"_self\">{2}</a>", result ? (ru["page"].ToInt() > 1 ? "/" + url.Trim('/') + "/" + staticindex + "_" + ru["page"] + exl : (url + "/" + staticindex + exl)) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")), i);
                        }
                    }
                }
                if (data.Page < (data.TotalPageCount - size))
                {
                    sb.Append("<span>...</span>");
                }
                if (data.TotalPageCount > 1 && data.Page < data.TotalPageCount)
                {
                    ru["page"] = data.Page + 1;
                    sb.AppendFormat("<a href=\"{0}\" {1} target=\"_self\">下一页</a>", result ? (ru["page"].ToInt() > 1 ? "/" + url.Trim('/') + "/" + staticindex + "_" + ru["page"] + exl : url) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                    ru["page"] = data.TotalPageCount;
                    sb.AppendFormat("<a href=\"{0}\" {1} target=\"_self\">末页</a>", result ? (ru["page"].ToInt() > 1 ? "/" + url.Trim('/') + "/" + staticindex + "_" + ru["page"] + exl : url) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                }

                sb.Append(dataEx.Invoke(data));
                return new Regex(@"(\[\d+\]=)|(%5b\d%5d=)", RegexOptions.IgnoreCase).Replace(sb.ToString(), "=").Replace("index" + exl, "");
            }
            else
            {
                switch (templatekey)
                {
                    case "dtcms":
                        {
                            var dtsb = new StringBuilder();

                            dtsb.AppendLine("<div class=\"pagelist\">");
                            dtsb.AppendLine("<div class=\"l-btns\">");
                            dtsb.AppendLine("<span>显示</span><input name=\"psize\" type=\"text\" value=\"" + data.PageSize + "\" onchange=\"changepsize(this)\" onkeypress=\"if (WebForm_TextBoxKeyHandler(event) == false) return false;\" id=\"txtPageNum\" class=\"pagenum\" onkeydown=\"return checkNumber(event);\"><span>条/页</span>");
                            dtsb.AppendLine("</div>");
                            dtsb.AppendLine("<div class=\"default\"><span>共" + data.TotalCount + "记录</span>");
                            if (data.Page == 1)
                            {
                                dtsb.AppendLine("<span class=\"disabled\">«上一页</span>");
                                dtsb.AppendLine("<span class=\"current\">1</span>");
                            }
                            else
                            {
                                ru["page"] = data.Page - 1;
                                dtsb.AppendFormat("<a href=\"{0}\" {1}>«上一页</a>", (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                                ru["page"] = 1;
                                dtsb.AppendFormat("<a href=\"{0}\" {1}>1</a>", (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                            }
                            if (data.Page > (size + 1))
                            {
                                dtsb.Append("<span>...</span>");
                            }
                            for (int i = min; i <= data.TotalPageCount && i <= max; i++)
                            {
                                if (i > 0 && i != 1 && i != data.TotalPageCount)
                                {
                                    if (i == data.Page)
                                    {
                                        ru["page"] = i;
                                        dtsb.AppendFormat("<span class=\"current\">{0}</span>", i);
                                    }
                                    else
                                    {
                                        ru["page"] = i;
                                        dtsb.AppendFormat("<a href=\"{0}\" {1}>{2}</a>", result ? (ru["page"].ToInt() > 1 ? "/" + url.Trim('/') + "/" + staticindex + "_" + ru["page"] + exl : (url + "/" + staticindex + exl)) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")), i);
                                    }
                                }
                            }

                            if (data.Page < (data.TotalPageCount - size))
                            {
                                dtsb.Append("<span>...</span>");
                            }

                            if (data.Page == data.TotalPageCount || data.TotalPageCount < 2)
                            {
                                if (data.TotalPageCount > 1)
                                {
                                    dtsb.AppendLine("<span class=\"current\">" + data.TotalPageCount + "</span>");
                                }
                                dtsb.AppendLine("<span class=\"disabled\">下一页»</span>");
                            }
                            else
                            {
                                if (data.TotalPageCount > 1)
                                {
                                    ru["page"] = data.TotalPageCount;
                                    dtsb.AppendFormat("<a href=\"{0}\" {1}>{2}</a>", (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")), data.TotalPageCount);
                                }
                                ru["page"] = data.Page + 1;
                                dtsb.AppendFormat("<a href=\"{0}\" {1}>下一页»</a>", (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))), string.Join(" ", htmlAttributes.Select(a => a.Key + "=\"" + a.Value + "\"")));
                            }

                            //<a href=\"article_list.aspx?channel_id=1&amp;page=1\">«上一页</a>
                            //<a href=\"article_list.aspx?channel_id=1&amp;page=1\">1</a>
                            //<span class=\"current\">2</span>
                            //<a href=\"article_list.aspx?channel_id=1&amp;page=3\">3</a>
                            //<a href=\"article_list.aspx?channel_id=1&amp;page=3\">下一页»</a>
                            dtsb.AppendLine("</div>");
                            dtsb.AppendLine("</div>");

                            return dtsb.ToString();
                        }
                    default:
                        {
                            var astr = @"<a class=""ui-icon ace-icon fa {1} bigger-140"" {0}></a>";

                            sb.AppendLine(@"<td id=""grid-pager_center"" align=""center"">");
                            sb.AppendLine(@"<table cellspacing=""0"" cellpadding=""0"" border=""0"" style=""table-layout: auto;"" class=""ui-pg-table"">");
                            sb.AppendLine(@"<tbody>");
                            sb.AppendLine(@"<tr>");
                            sb.AppendLine(string.Format(@"<td id=""first_grid-pager"" class=""ui-pg-button ui-corner-all {0}"">", data.Page <= 1 ? "ui-state-disabled" : ""));
                            if (data.Page <= 1)
                            {
                                sb.AppendLine(string.Format(astr, "", "fa-angle-double-left"));
                            }
                            else
                            {
                                ru["page"] = 1;
                                sb.AppendLine(string.Format(astr, "href=\"" + (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))) + "\"", "fa-angle-double-left"));
                            }
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(string.Format(@"<td id=""prev_grid-pager"" class=""ui-pg-button ui-corner-all {0}"" style=""cursor: default;"">", data.Page <= 1 ? "ui-state-disabled" : ""));
                            if (data.Page <= 1)
                            {
                                sb.AppendLine(string.Format(astr, "", "fa-angle-left"));
                            }
                            else
                            {
                                ru["page"] = data.Page - 1;
                                sb.AppendLine(string.Format(astr, "href=\"" + (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))) + "\"", "fa-angle-left"));
                            }
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(@"<td class=""ui-pg-button ui-state-disabled"" style=""width: 4px; cursor: default;"">");
                            sb.AppendLine(@"<span class=""ui-separator""></span>");
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(@"<td dir=""ltr"">");
                            sb.AppendLine(@"Page");
                            sb.AppendLine(string.Format(@"<input class=""ui-pg-input"" type=""text"" size=""2"" maxlength=""7"" value=""{0}"" role=""textbox"">", data.Page));
                            sb.AppendLine(string.Format(@"of <span id=""sp_1_grid-pager"">{0}</span>", data.TotalPageCount));
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(@"<td class=""ui-pg-button ui-state-disabled"" style=""width: 4px; cursor: default;"">");
                            sb.AppendLine(@"<span class=""ui-separator""></span>");
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(string.Format(@"<td id=""next_grid-pager"" class=""ui-pg-button ui-corner-all {0}"" style=""cursor: default;"">", data.Page >= data.TotalPageCount ? "ui-state-disabled" : ""));
                            if (data.Page >= data.TotalPageCount)
                            {
                                sb.AppendLine(string.Format(astr, "", "fa-angle-right"));
                            }
                            else
                            {
                                ru["page"] = data.Page + 1;
                                sb.AppendLine(string.Format(astr, "href=\"" + (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))) + "\"", "fa-angle-right"));
                            }
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(string.Format(@"<td id=""last_grid-pager"" class=""ui-pg-button ui-corner-all {0}"">", data.Page >= data.TotalPageCount ? "ui-state-disabled" : ""));
                            if (data.Page >= data.TotalPageCount)
                            {
                                sb.AppendLine(string.Format(astr, "", "fa-angle-double-right"));
                            }
                            else
                            {
                                ru["page"] = data.TotalPageCount;
                                sb.AppendLine(string.Format(astr, "href=\"" + (result ? (url + "/" + staticindex + exl) : (url + "?" + string.Join("&", ru.Select(a => a.Key + "=" + a.Value)))) + "\"", "fa-angle-double-right"));
                            }
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(@"</tr>");
                            sb.AppendLine(@"</tbody>");
                            sb.AppendLine(@"</table>");
                            sb.AppendLine(@"</td>");
                            sb.AppendLine(@"<td id=""grid-pager_right"" align=""right"">");
                            sb.AppendLine(@"<div dir=""ltr"" style=""text-align: right"" class=""ui-paging-info"">");
                            sb.AppendLine(string.Format(@"View 1 - {0} of {1}</div>", data.PageSize, data.TotalCount));
                            sb.AppendLine(@"</td>");
                            return sb.ToString();
                        }
                }
            }
        }


        /// <summary>
        /// 生成含page的路由字典
        /// </summary>
        /// <param name="querymodel">ISearchQueryString</param>
        /// <param name="page"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetRoute(ISearchQueryString querymodel, int page)
        {
            IDictionary<string, object> r;
            if (querymodel == null)
            {
                r = new Dictionary<string, object>();
            }
            else
            {
                r = querymodel.QueryString;
            }
            if (r == null)
            {
                r = new Dictionary<string, object>();
            }
            if (!r.ContainsKey("page"))
                r.Add("page", page);


            return r;
        }

        #endregion
    }
}
