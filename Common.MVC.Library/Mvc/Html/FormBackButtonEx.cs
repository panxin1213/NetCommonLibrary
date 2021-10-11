using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 返回按钮扩展
    /// </summary>
    public static class FormBackButtonEx
    {
        /// <summary>
        /// 要返回URL表单名
        /// </summary>
        public static readonly string Hidden_Name = "_sm_back_url_";
        /// <summary>
        /// 定时返回的按钮
        /// </summary>
        /// <param name="h"></param>
        /// <param name="second"></param>
        /// <param name="urldefalt"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString FormAutoBackButton(this HtmlHelper h, int second = 2, string urldefalt = null, string text = "返回", object htmlAttributes = null)
        {
            var b = h.FormBackButton(urldefalt, text, htmlAttributes).ToHtmlString();
            var j = string.Format("<script>setTimeout(function(){{location=document.getElementById('{0}').value}},{1}000)</script>", Hidden_Name, second);
            return MvcHtmlString.Create(b + j);
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
        public static MvcHtmlString FormBackButton(this HtmlHelper h, string urldefalt = null, string text = "返回", object htmlAttributes = null, string tagname = "input")
        {
            return FormBackButton(h, null, urldefalt, text, htmlAttributes, tagname);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="urldefalt"></param>
        /// <param name="text"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="tagname"></param>
        /// <param name="directLink">直接跳转链接</param>
        /// <returns></returns>
        public static MvcHtmlString FormBackButton(this HtmlHelper h, string directLink, string urldefalt = null, string text = "返回", object htmlAttributes = null, string tagname = "input")
        {
            var referrer = System.Web.HttpContext.Current.Request.UrlReferrer;
            var backUrl = !String.IsNullOrWhiteSpace(directLink) ? directLink : (referrer == null ?
                UrlHelper.GenerateUrl(null, null, null, null, h.RouteCollection, h.ViewContext.RequestContext, true)
                :
                referrer.ToString()); //取不到来路就取action = index。

            var sb = new StringBuilder();
            if ("POST".Equals(System.Web.HttpContext.Current.Request.HttpMethod))
            {
                var re = h.ViewContext.HttpContext.Request[Hidden_Name];
                if (re != null)
                    backUrl = re.ToString();
            }
            backUrl = String.IsNullOrEmpty(backUrl) ? urldefalt : backUrl;

            //hidden
            TagBuilder hidden = new TagBuilder("input");
            hidden.MergeAttribute("type", "hidden");
            hidden.MergeAttribute("value", backUrl);
            hidden.MergeAttribute("id", Hidden_Name);
            hidden.MergeAttribute("name", Hidden_Name);
            sb.Append(hidden.ToString(TagRenderMode.SelfClosing));

            TagBuilder button = null;
            if (tagname.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                button = new TagBuilder("a");
                button.InnerHtml = text;
            }
            else
            {
                button = new TagBuilder("input");
                button.MergeAttribute("type", "button");
                button.MergeAttribute("value", text);
            }
            //input
            button.MergeAttributes<string, object>(new RouteValueDictionary(htmlAttributes));
            button.MergeAttribute("onclick", "if(parent!=null&&parent.document.getElementById('_sm_back_url_')!=null){parent.location=parent.document.getElementById('" + Hidden_Name + "').value;}else{location=document.getElementById('" + Hidden_Name + "').value;}");
            if (tagname.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(button.ToString());
            }
            else
            {
                sb.Append(button.ToString(TagRenderMode.SelfClosing));
            }

            return new MvcHtmlString(sb.ToString());
        }

    }
}
