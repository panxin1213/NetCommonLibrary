using Admin.Model;
using Common.Library.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public class FrontWebViewPage : BaseWebViewPage
    {
        /// <summary>
        /// 返回广告元素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MvcHtmlString GetAdShow(int? id, bool nofollow = false)
        {
            try
            {
                if (id == null || id.Value < 1)
                {
                    return new MvcHtmlString("");
                }

                var m = ServiceMain.ADService.Get(id.Value);
                if (m == null)
                {
                    return new MvcHtmlString("");
                }
                var result = m.F_Ad_Link != null && (m.F_Ad_Link.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || m.F_Ad_Link.StartsWith("https://", StringComparison.OrdinalIgnoreCase));
                    
                if (m.F_Ad_Type == AdType.Html代码广告.ToInt())
                {
                    return new MvcHtmlString(m.F_Ad_Html.ConvertImgUnAbsoluteSrcToAbsolute().ToSafeHTML());
                }
                else if (m.F_Ad_Type == AdType.文字广告.ToInt())
                {
                    return new MvcHtmlString(string.Format("<a href=\"{1}\" {2} target=\"_blank\">{0}</a>"
                        , m.F_Ad_Des
                        , result ? m.F_Ad_Link : Url.AbsoluteUrl(m.F_Ad_Link)
                        , nofollow ? "rel=\"nofollow\"" : ""));
                }
                else if (m.F_Ad_Type == AdType.图片广告.ToInt())
                {
                    return new MvcHtmlString(string.Format("<a href=\"{1}\" {2} target=\"_blank\">{0}</a>"
                        , string.Format("<img src=\"{0}\" alt=\"{1}\" />", Url.ImageThumb(m.F_Ad_Image, 0, 0), m.F_Ad_Des)
                        , result ? m.F_Ad_Link : Url.AbsoluteUrl(m.F_Ad_Link)
                        , nofollow ? "rel=\"nofollow\"" : ""));
                }
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                return new MvcHtmlString(string.Empty);
            }

            return new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// 返回图片元素
        /// </summary>
        /// <param name="src">图片地址</param>
        /// <param name="htmlattributies">属性对象</param>
        /// <returns></returns>
        public MvcHtmlString GetImg(string src, object htmlattributies)
        {
            var nopicfile = string.Empty;
            TagBuilder t = new TagBuilder("img");

            var route = new RouteValueDictionary(htmlattributies);
            if (route.ContainsKey("class"))
            {
                route["class"] += " lazy";
            }
            else
            {
                route["class"] = "lazy";
            }
            t.MergeAttributes(route);
            t.MergeAttribute("original", src, true);
            t.MergeAttribute("src", Url.Res("f1/img/loading.gif"), true);
            if (!String.IsNullOrEmpty(nopicfile) && !nopicfile.Equals(src))
                t.MergeAttribute("onerror", String.Format("this.src='{0}';this.onerror=null", nopicfile), true);
            return MvcHtmlString.Create(t.ToString(TagRenderMode.SelfClosing));
        }


        public override void Execute()
        {
            base.Execute();
        }

    }

    public class FrontWebViewPage<T> : BaseWebViewPage<T>
    {
        /// <summary>
        /// 返回广告元素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MvcHtmlString GetAdShow(int? id, bool nofollow = false)
        {
            return new FrontWebViewPage().GetAdShow(id, nofollow);
        }

        /// <summary>
        /// 返回图片元素
        /// </summary>
        /// <param name="src">图片地址</param>
        /// <param name="htmlattributies">属性对象</param>
        /// <returns></returns>
        public MvcHtmlString GetImg(string src, object htmlattributies)
        {
            return new FrontWebViewPage().GetImg(src, htmlattributies);
        }

        public override void Execute()
        {
            base.Execute();
        }
    }
}
