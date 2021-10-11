using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Linq.Expressions;

using System.Data.Pager;

using System.Data;
using System.Text.RegularExpressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 翻页链接生成 的 HtmlHelper的扩展
    /// </summary>
    public static class PagerHtmlExtensions
    {
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
        public static MvcHtmlString PageBar(this HtmlHelper html, int size, Func<IPagerDataAndQuery, string> dataEx = null, object htmlAttributes = null, IEnumerable<string> exceptQuery = null)
        {
            var data = html.ViewData.Model as IPagerDataAndQuery;

            return html.PageBar(data, dataEx, htmlAttributes, size, exceptQuery);
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
        /// <param name="area">Area</param>
        /// <returns></returns>
        public static MvcHtmlString PageBar(this HtmlHelper html, Func<IPagerDataAndQuery, string> dataEx = null, int size = 2, object htmlAttributes = null, IEnumerable<string> exceptQuery = null, string urlAppend = null, string defaultboxparent = null, string defaultbox = null, string area = null, bool showInput = true, bool shownum = true, string hoverclass = "number current")
        {
            var data = html.ViewData.Model as IPagerDataAndQuery;
            return html.PageBar(data, dataEx, htmlAttributes, size, exceptQuery, urlAppend, defaultboxparent, defaultbox, area, showInput, shownum, hoverclass);
        }
        /// <summary>
        /// 搜索结果标题获取
        /// </summary>
        /// <param name="html"></param>
        /// <param name="constStr">如果页码大于1需要显示"第x页"在最后，而Html.Title方法会加上网站名在最后，所以传入中间的字符</param>
        /// <returns></returns>
        public static MvcHtmlString GetPageSearchTitle(this HtmlHelper html, string constStr="")
        {
            var data = html.ViewData.Model as IPagerDataAndQuery;
            var q = data.QueryString as ISearchCondition;
            string pageStr = String.Empty;
            if (data.Page > 1)
            {
                pageStr = String.Format(" 第{0}页",data.Page);
            }
            return MvcHtmlString.Create(String.Format("{0}{1}{2}",q.GetSearchTitle(),constStr,pageStr));
        }

        /// <summary>
        /// 搜索结果Metakey获取
        /// </summary>
        /// <param name="html"></param>
        /// <param name="constStr"></param>
        /// <returns></returns>
        public static MvcHtmlString GetPageSearchMetaKey(this HtmlHelper html, string constStr = "")
        {
            var data = html.ViewData.Model as IPagerDataAndQuery;
            var q = data.QueryString as ISearchCondition;
            return MvcHtmlString.Create(String.Format("{0}{1}", q.GetSearchMetaKey(), constStr));
        }

        /// <summary>
        /// 搜索结果MetaContent获取
        /// </summary>
        /// <param name="html"></param>
        /// <param name="constStr"></param>
        /// <returns></returns>
        public static MvcHtmlString GetPageSearchMetaContent(this HtmlHelper html, string constStr = "")
        {
            var data = html.ViewData.Model as IPagerDataAndQuery;
            var q = data.QueryString as ISearchCondition;
            return MvcHtmlString.Create(String.Format("{0}{1}", q.GetSearchMetaContent(), constStr));
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
        public static MvcHtmlString PageBar(this HtmlHelper html, object d, Func<IPagerDataAndQuery, string> dataEx = null, int size = 2, object htmlAttributes = null, IEnumerable<string> exceptQuery = null)
        {
            IPagerDataAndQuery data;
            if (d == null)
                data = html.ViewData.Model as IPagerDataAndQuery;
            else
                data = d as IPagerDataAndQuery;
            return html.PageBar(data, dataEx, htmlAttributes, size, exceptQuery);
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
        /// <param name="area">Area</param>
        /// <param name="showInput">是否显示输入框</param>
        /// <returns></returns>
        public static MvcHtmlString PageBar(this HtmlHelper html, IPagerDataAndQuery d, Func<IPagerDataAndQuery, string> dataEx = null, object htmlAttributes = null, int size = 2, IEnumerable<string> exceptQuery = null, string urlAppend = null, string defaultboxparent = null, string defaultbox = null, string area = null, bool showInput = true, bool shownum = true, string hoverclass = "number current")
        {
            string fragment = null;
            if (!string.IsNullOrEmpty(urlAppend))
                fragment = urlAppend.TrimStart('#');
            var data = d as IPagerDataAndQuery;
            if (data == null) return null;
            if (dataEx == null) dataEx = a => String.Format("<br />第 {0}/{1} 页 , 共 {2} 条, 每页{3}条", a.Page, a.TotalPageCount, a.TotalCount, a.PageSize);

            var ru = GetRoute(data.QueryString, 0);
            if (!string.IsNullOrEmpty(area))
                ru.Add("Area", area);
            if (exceptQuery != null)
            {
                var x = ru.Except(ru.Where(a => exceptQuery.Any(b => (b.ToLower() == a.Key.ToLower() || (a.Key.IndexOf(b + "[", StringComparison.OrdinalIgnoreCase) == 0 && a.Key.LastIndexOf("]") == a.Key.Length - 1)))));
                ru = new RouteValueDictionary(x.ToDictionary(a => a.Key, a => a.Value));
            }
            var attrDict = new RouteValueDictionary(htmlAttributes);
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(defaultboxparent))
            {
                sb.Append("<" + defaultboxparent + ">");
            }
            int min = data.Page - size;
            int max = data.Page + size;
            if (data.Page > 1)
            {
                ru["page"] = 1;
                //ru.Remove("page");

                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + html.AbsoluteActionLink("首页", null, null, fragment, ru, attrDict.CopyAdd("data-page", 1).CopyAdd("class", "btn")) + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
                if (data.Page > 2)
                {
                    ru["page"] = data.Page - 1;
                    //if (!String.IsNullOrEmpty(ru.LastOrDefault().Key))
                    //{
                    //    ru[ru.LastOrDefault().Key] += urlAppend;
                    //}
                }
                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + html.AbsoluteActionLink("上一页", null, null, fragment, ru, attrDict.CopyAdd("data-page", data.Page - 1).CopyAdd("class", "btn")) + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
            }
            if (data.Page > (size + 1) && shownum && size > 0)
            {
                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + "<span>...</span>" + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
            }
            if (shownum)
            {
                for (int i = min; i <= data.TotalPageCount && i <= max; i++)
                {
                    if (i > 0)
                    {
                        if (i == data.Page)
                        {
                            ru["page"] = i;
                            sb.AppendFormat((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + " class=\"" + hoverclass + "\">" : "") + "<a class=\"" + hoverclass + "\">{0}</a>" + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""), i);
                        }
                        else
                        {
                            if (i == 1 && ru.ContainsKey("page")) ru.Remove("page");

                            ru["page"] = i;
                            sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + html.AbsoluteActionLink(i.ToString(), null, null, fragment, ru, attrDict.CopyAdd("data-page", i)) + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
                        }
                    }
                }
            }
            if (data.Page < (data.TotalPageCount - size) && shownum && size > 0)
            {
                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + "<span>...</span>" + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
            }
            if (data.TotalPageCount > 1 && data.Page < data.TotalPageCount)
            {
                ru["page"] = data.Page + 1;
                //if (!String.IsNullOrEmpty(ru.LastOrDefault().Key))
                //{
                //    ru[ru.LastOrDefault().Key] += urlAppend;
                //}
                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + html.AbsoluteActionLink("下一页", null, null, fragment, ru, attrDict.CopyAdd("data-page", data.Page + 1).CopyAdd("class", "btn")) + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
                ru["page"] = data.TotalPageCount;
                //if (!String.IsNullOrEmpty(ru.LastOrDefault().Key))
                //{
                //    ru[ru.LastOrDefault().Key] += urlAppend;
                //}
                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + html.AbsoluteActionLink("末页", null, null, fragment, ru, attrDict.CopyAdd("data-page", data.TotalPageCount).CopyAdd("class", "btn")) + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
            }
            ru.Remove("page");
            if (showInput) { 
                sb.Append((!String.IsNullOrEmpty(defaultbox) ? "<" + defaultbox + ">" : "") + "<input type=\"text\" size=\"3\" value=\"" + data.Page + "\" id=\"pagekey\"/><div style=\"display:none;\" id=\"pagediv\">" + html.AbsoluteActionLink("末页", null, ru, new RouteValueDictionary(htmlAttributes)).ToHtmlString() + "</div><input type=\"button\" value=\"确定\" id=\"pagebtn\" onclick=\"var page=$('#pagekey').val();if(page==''){return;}var url=$('#pagediv').find('a').attr('href');url=url.indexOf('?')>-1?url+'&':url+'?';url=url+'page='+page;location.href=url;\" />" + (!String.IsNullOrEmpty(defaultbox) ? "</" + defaultbox + ">" : ""));
            }

            if (!String.IsNullOrEmpty(defaultboxparent))
            {
                sb.Append("</" + defaultboxparent + ">");
            }
            sb.Append(dataEx.Invoke(data));
            return new MvcHtmlString(new Regex(@"(\[\d+\]=)|(%5b\d%5d=)", RegexOptions.IgnoreCase).Replace(sb.ToString(), "=").Replace("%23", "#"));
        }
        /// <summary>
        /// 生成含page的路由字典
        /// </summary>
        /// <param name="querymodel">ISearchQueryString</param>
        /// <param name="page"></param>
        /// <returns></returns>
        private static RouteValueDictionary GetRoute(ISearchQueryString querymodel, int page)
        {
            RouteValueDictionary r;
            if (querymodel == null)
            {
                r = new RouteValueDictionary();
            }
            else
            {
                r = new RouteValueDictionary(querymodel.QueryString);
            }
            if (!r.ContainsKey("page"))
                r.Add("page", page);


            return r;
        }



        /// <summary>
        /// 前台pagebar
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString PageBarFront(this HtmlHelper html, string hoveclass = "on", int size = 2, string itemtemplate = "<a href=\"{%Url%}\" class=\"{%on%}\">{%Page%}</a>", IEnumerable<string> exceptQuery = null)
        {
            var data = html.ViewData.Model as IPagerDataAndQuery;

            var action = html.ViewContext.RouteData.Values.TryGetValue("action", "").ToString();
            var controller = html.ViewContext.RouteData.Values.TryGetValue("controller", "").ToString();

            var sb = new StringBuilder();

            int min = data.Page - size;
            int max = data.Page + size;
            var Url = new UrlHelper(html.ViewContext.RequestContext);

            var ru = data.QueryString.QueryString.CopyRemove("psize");
            if (exceptQuery != null)
            {
                ru = ru.CopyRemove(exceptQuery.ToArray());
            }

            if (data.Page > 1)
            {
                sb.Append(itemtemplate.Replace("{%Url%}", Url.AbsoluteAction(action, controller, new RouteValueDictionary(ru.CopySet("page", 1))))
                    .Replace("{%Page%}", "首页").Replace("{%on%}", ""));
                sb.Append(itemtemplate.Replace("{%Url%}", Url.AbsoluteAction(action, controller, new RouteValueDictionary(ru.CopySet("page", data.Page - 1))))
                    .Replace("{%Page%}", "上一页").Replace("{%on%}", ""));
            }

            if (data.Page > (size + 1) && size > 0)
            {
                sb.Append(itemtemplate.Replace("{%Url%}", "javascript:void(0);")
                    .Replace("{%Page%}", "...").Replace("{%on%}", hoveclass));
            }

            for (int i = min; i <= data.TotalPageCount && i <= max; i++)
            {
                if (i > 0)
                {
                    if (i == data.Page)
                    {
                        sb.Append(itemtemplate.Replace("{%Url%}", "javascript:void(0);")
                        .Replace("{%Page%}", i.ToSafeString()).Replace("{%on%}", hoveclass));
                    }
                    else
                    {
                        sb.Append(itemtemplate.Replace("{%Url%}", Url.AbsoluteAction(action, controller, new RouteValueDictionary(ru.CopySet("page", i))))
                        .Replace("{%Page%}", i.ToSafeString()).Replace("{%on%}", ""));
                    }
                }
            }

            if (data.Page < (data.TotalPageCount - size) && size > 0)
            {
                sb.Append(itemtemplate.Replace("{%Url%}", "javascript:void(0);")
                    .Replace("{%Page%}", "...").Replace("{%on%}", hoveclass));
            }

            if (data.TotalPageCount > 1 && data.Page < data.TotalPageCount)
            {
                sb.Append(itemtemplate.Replace("{%Url%}", Url.AbsoluteAction(action, controller, new RouteValueDictionary(ru.CopySet("page", data.Page + 1))))
                    .Replace("{%Page%}", "下一页").Replace("{%on%}", ""));
                sb.Append(itemtemplate.Replace("{%Url%}", Url.AbsoluteAction(action, controller, new RouteValueDictionary(ru.CopySet("page", data.TotalPageCount))))
                    .Replace("{%Page%}", "尾页").Replace("{%on%}", ""));
            }


            return new MvcHtmlString(sb.ToString());
        }
    }
}
