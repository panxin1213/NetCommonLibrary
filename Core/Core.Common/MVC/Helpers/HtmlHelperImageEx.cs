using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Globalization;
using Core.Common;
using ChinaBM.Common;
using Core.Base;
namespace System.Web.Mvc
{
    /// <summary>
    /// 图片相关扩展
    /// </summary>
    public static class HtmlHelperImageEx
    {

        public static string ImageThumbUrl = BaseConfig.Current.Path.ImageThumb;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="file"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string ImageThumb(this UrlHelper h, string file, int width = 0, int height = 0, System.Drawing.ThumbMode mode = Drawing.ThumbMode.Cut)
        {
            if (string.IsNullOrEmpty(file))
            {
                return HtmlHelperResEx.GetServerPath("/Res/choose/Picture/user/zanwu.gif");
            }

            if (file.IndexOf(h.ImageServerMaster("")) > -1)
            {
                file = file.Replace(h.ImageServerMaster(""), "");
            }

            if (file.IndexOf("http://", StringComparison.OrdinalIgnoreCase) > -1)
            {
                file = "/web6/" + EncryptKit.EncodeBase64(file.ToLower().Trim().Replace("http://", "")).Replace("/", "$EFG$") + ".jpg";
            }
            var m = "c"; //跟M.ashx对应
            switch (mode)
            {
                case Drawing.ThumbMode.Height:
                    m = "h";
                    break;
                case Drawing.ThumbMode.Width:
                    m = "w";
                    break;
                case Drawing.ThumbMode.WidthAndHeight:
                    m = "a";
                    break;
                case Drawing.ThumbMode.WidthOrHeight:
                    m = "o";
                    break;
            }
            var url = HtmlHelperResEx.GetServerPath(ImageThumbUrl.Replace("#path#", file).Replace("#mode#", m).Replace("#width#", width.ToString()).Replace("#height#", height.ToString()));
            if (!url.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                return HttpKit.IsSupportWebP() ? url + ".webp" : url;
            }

            return url;
            //return string.Format("{0}{1}", ImageServer.Random(), ImageThumbUrl.Replace("#path#", file).Replace("#mode#", m).Replace("#width#", width.ToString()).Replace("#height#", height.ToString()));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="file"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="nopicfile"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageThumb(this HtmlHelper h, string file,string nopicfile, int width = 0, int height = 0, System.Drawing.ThumbMode mode = Drawing.ThumbMode.Cut ,object htmlattribute=null)
        {
            if (String.IsNullOrEmpty(nopicfile))
                throw new Exception("nopicfile 参数不能为空");

            nopicfile = HtmlHelperResEx.GetServerPath(nopicfile);
            if (!String.IsNullOrEmpty(file))
                file = new UrlHelper(h.ViewContext.RequestContext).ImageThumb(file, width, height, mode);
            else
                file = nopicfile;

            return h.ImageAbsolute(file, nopicfile, htmlattribute);

        }
        public static MvcHtmlString ImageThumbFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression,string nopicfile, int width = 0, int height = 0, System.Drawing.ThumbMode mode = Drawing.ThumbMode.Cut ,object htmlattribute=null)
        {
            var v = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return html.ImageThumb(String.Format("{0}",v.Model), nopicfile, width, height, mode, htmlattribute);
        }
        /// <summary>
        /// 输出img 传入地址为相对路径
        /// </summary>
        /// <param name="h"></param>
        /// <param name="file"></param>
        /// <param name="nopicfile"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper h, string file, string nopicfile, object htmlattrbute = null)
        {
            if (String.IsNullOrEmpty(nopicfile))
                throw new Exception("nopicfile 参数不能为空");

            nopicfile = HtmlHelperResEx.GetServerPath(file);
            if (String.IsNullOrEmpty(file))
                file = nopicfile;
            else
                file = HtmlHelperResEx.GetServerPath(file);
            return h.ImageAbsolute(file, nopicfile, htmlattrbute);
        }
        /// <summary>
        /// 输入img标签，传入的值都为绝对地址
        /// </summary>
        /// <param name="h"></param>
        /// <param name="file"></param>
        /// <param name="nopicfile"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageAbsolute(this HtmlHelper h, string file, string nopicfile, object htmlattrbute = null)
        {
            if (file.IndexOf(".swf", StringComparison.OrdinalIgnoreCase) > -1)
            {
                TagBuilder t = new TagBuilder("embed");
                t.MergeAttributes(new RouteValueDictionary(htmlattrbute));
                t.MergeAttribute("type", "application/x-shockwave-flash", true);
                t.MergeAttribute("quality", "high", true);
                t.MergeAttribute("src", file.Replace("/u/0-0-c/", "/"), true);

                return MvcHtmlString.Create(t.ToString(TagRenderMode.SelfClosing));
            }
            else
            {
                TagBuilder t = new TagBuilder("img");
                t.MergeAttributes(new RouteValueDictionary(htmlattrbute));
                t.MergeAttribute("src", file, true);
                if (!String.IsNullOrEmpty(nopicfile) && !nopicfile.Equals(file))
                    t.MergeAttribute("onerror", String.Format("this.src='{0}';this.onerror=null", nopicfile), true);
                return MvcHtmlString.Create(t.ToString(TagRenderMode.SelfClosing));
            }
        }
        
    }
}
