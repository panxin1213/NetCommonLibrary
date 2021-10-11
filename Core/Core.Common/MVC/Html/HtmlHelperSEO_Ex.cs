using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Pager;
using System.Text.RegularExpressions;
using Common.Library;
using Core.Common.Service;
using Core.Base;
using Admin.Model;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperSEO_Ex
    {
        /// <summary>
        /// 页面SEO信息
        /// </summary>
        /// <param name="html">html对象</param>
        /// <param name="defaultTitle">默认title</param>
        /// <param name="defaultMetaKey">默认metakey</param>
        /// <param name="defaultMetaContent">默认metacontent</param>
        /// <param name="separatetuple">title ,key ,des 关键词分隔字符默认("_",",","、")</param>
        /// <returns></returns>
        public static MvcHtmlString GetSEOTitleKeyAndContent(this HtmlHelper html, string defaultTitle = "", string defaultMetaKey = "", string defaultMetaContent = "", Tuple<string, string, string> separatetuple = null)
        {
            if (separatetuple == null)
            {
                separatetuple = new Tuple<string, string, string>("_", ",", "、");
            }
            var action = html.ViewContext.RouteData.Values["action"].ToString();
            var controller = html.ViewContext.RouteData.Values["controller"].ToString();
            var area = html.ViewContext.RouteData.Values.TryGetValue("area", "").ToString();
            var link = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            var ismobile = false;
            if (link.StartsWith("http://m.", StringComparison.OrdinalIgnoreCase))
            {
                link = link.Replace("http://m.", "http://www.");
                ismobile = true;
            }

            using (IDbConnection db = Conn.GetByKey("Admin"))
            {
                var _cssv = new SEO_Service(db);

                var m = _cssv.Get(action, controller, area, link.ToSafeString().Trim('/'));

                if (m != null)
                {
                    var data = html.ViewData.Model as IPagerDataAndQuery;

                    if (m.F_Seo_Type == SEOType.单页.ToInt() || data == null)
                    {
                        defaultTitle = m.F_Seo_Title;
                        defaultMetaKey = m.F_Seo_MetaKey;
                        defaultMetaContent = m.F_Seo_MetaContent;
                    }
                    else
                    {
                        var q = data.QueryString as ISearchCondition;

                        var l = q.GetSearchKeyList();

                        if (l.Count > 0 && ((l.ContainsKey("pro") && l["pro"].Count > 0) || (l.ContainsKey("next") && l["next"].Count > 0)))
                        {
                            var regex = new Regex(@"\[\{(?<key>(key|keystr))\}\{(?<param>((?!\}).)*)\}\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                            var matchs = regex.Matches(m.F_Seo_Search_Title);
                            if (matchs.Count > 0)
                            {
                                foreach (Match match in matchs)
                                {
                                    defaultTitle = m.F_Seo_Search_Title.Replace(match.Value, AnalyzeString(match.Groups["key"].Value, l, match.Groups["param"].Value, separatetuple.Item1));
                                    m.F_Seo_Search_Title = defaultTitle;
                                }
                            }
                            else
                            {
                                defaultTitle = !String.IsNullOrEmpty(m.F_Seo_Search_Title) ? m.F_Seo_Search_Title : m.F_Seo_Title;
                            }

                            matchs = regex.Matches(m.F_Seo_Search_MetaKey);
                            if (matchs.Count > 0)
                            {
                                foreach (Match match in matchs)
                                {
                                    defaultMetaKey = m.F_Seo_Search_MetaKey.Replace(match.Value, AnalyzeString(match.Groups["key"].Value, l, match.Groups["param"].Value, separatetuple.Item2));
                                    m.F_Seo_Search_MetaKey = defaultMetaKey;
                                }
                            }
                            else
                            {
                                defaultMetaKey = !String.IsNullOrEmpty(m.F_Seo_Search_MetaKey) ? m.F_Seo_Search_MetaKey : m.F_Seo_MetaKey;
                            }

                            matchs = regex.Matches(m.F_Seo_Search_MetaContent);
                            if (matchs.Count > 0)
                            {
                                foreach (Match match in matchs)
                                {
                                    defaultMetaContent = m.F_Seo_Search_MetaContent.Replace(match.Value, AnalyzeString(match.Groups["key"].Value, l, match.Groups["param"].Value, separatetuple.Item3));
                                    m.F_Seo_Search_MetaContent = defaultMetaContent;
                                }
                            }
                            else
                            {
                                defaultMetaContent = !String.IsNullOrEmpty(m.F_Seo_Search_MetaContent) ? m.F_Seo_Search_MetaContent : m.F_Seo_Controller;
                            }

                        }
                        else
                        {
                            defaultTitle = m.F_Seo_Title;
                            defaultMetaKey = m.F_Seo_MetaKey;
                            defaultMetaContent = m.F_Seo_MetaContent;
                        }
                        
                    }

                    if (data != null && data.Page > 1)
                    {
                        if (defaultTitle.EndsWith("_" + BaseConfig.Current.SiteName))
                        {
                            defaultTitle = defaultTitle.Replace("_" + BaseConfig.Current.SiteName, string.Format("_第{0}页_", data.Page) + BaseConfig.Current.SiteName);
                        }
                        else
                        {
                            defaultTitle = defaultTitle + "_第" + data.Page + "页";
                        }
                    }
                }
            }
            defaultTitle = System.Web.HttpContext.Current.Server.HtmlEncode(defaultTitle);
            defaultMetaKey = System.Web.HttpContext.Current.Server.HtmlEncode(defaultMetaKey);
            defaultMetaContent = System.Web.HttpContext.Current.Server.HtmlEncode(defaultMetaContent);

            if (ismobile && defaultTitle.IndexOf("移动版") == -1)
            {
                defaultTitle = defaultTitle.Replace(BaseConfig.Current.SiteName, BaseConfig.Current.SiteName + "移动版");
            }

            return MvcHtmlString.Create(string.Format("<title>{0}</title>\n<meta name=\"keywords\" content=\"{1}\" />\n<meta name=\"description\" content=\"{2}\" />", defaultTitle, defaultMetaKey, defaultMetaContent));
        }


        private static string AnalyzeString(string key, Dictionary<string, List<string>> l, string param, string delimiter)
        {
            var strs = param.Split(',');

            return string.Join(delimiter, strs.Select(a =>
            {
                switch (key.ToLower())
                {
                    case "keystr":
                        {
                            var s = a.ToString();

                            if (l.ContainsKey("pro") && l["pro"].Count > 0)
                            {
                                s = string.Join("", l["pro"]) + s;
                            }

                            if (l.ContainsKey("next") && l["next"].Count > 0)
                            {
                                s = s + string.Join("", l["next"]);
                            }
                            return s;
                        }
                    default:
                        {
                            List<string> el = new List<string>();

                            if (l.ContainsKey("pro") && l.Count > 0)
                            {
                                el = l["pro"].Select(b => b + a).ToList();
                            }

                            if (l.ContainsKey("next") && l["next"].Count > 0)
                            {
                                el.AddRange(l["next"].Select(b => a + b).ToList());
                            }

                            return string.Join(delimiter, el);
                            
                        }
                }

            }));
        }
    }
}
