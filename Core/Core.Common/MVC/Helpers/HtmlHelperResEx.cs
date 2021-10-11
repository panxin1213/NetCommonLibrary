using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Core.Common;
using Core.Base;
using ChinaBM.Common;
namespace System.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlHelperResEx
    {

        private static string ImageServerMasterUrl = BaseConfig.Current.Url.ImageMaster;
        private static string ResourceUrl = BaseConfig.Current.Path.Resource;// ConfigurationManager.AppSettings["Path_Resource"];

        /// <summary>
        /// 输出合并后的css
        /// </summary>
        /// <param name="h"></param>
        /// <param name="pa">文件列表</param>
        /// <returns></returns>
        public static MvcHtmlString Css(this HtmlHelper h, params string[] pa)
        {

            //var url = GetServerPath(ResourceUrl.Replace("#type#", "css").Replace("#files#", String.Join(";", pa)));
            var url = string.Format("{0}{1}", BaseConfig.Current.Url.ImageMaster, ResourceUrl.Replace("#type#", "css").Replace("#files#", String.Join(";", pa)));
            
            return MvcHtmlString.Create(String.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", url));
        }
        /// <summary>
        /// 输出合并后的js
        /// </summary>
        /// <param name="h"></param>
        /// <param name="pa">文件列表</param>
        /// <returns></returns>
        public static MvcHtmlString Js(this HtmlHelper h, params string[] pa)
        {
            //var url = GetServerPath(ResourceUrl.Replace("#type#", "js").Replace("#files#", String.Join(";", pa)));
            var url = string.Format("{0}{1}", BaseConfig.Current.Url.ImageMaster, ResourceUrl.Replace("#type#", "js").Replace("#files#", String.Join(";", pa)));

            return MvcHtmlString.Create(String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", url));
        }

        /// <summary>
        /// 输出一个图片服务器中文件地址 图片或文件
        /// </summary>
        /// <param name="h"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string File(this UrlHelper h, string file)
        {
            return GetServerPath(file);
        }
        public static string ImageServerMaster(this UrlHelper h, string file)
        {
            return ImageServerMaster(h, file, "/Res/v1/Picture/none.jpg");
        }
        /// <summary>
        /// 增加默认图片参数
        /// </summary>
        /// <param name="h"></param>
        /// <param name="file">图片地址</param>
        /// <param name="defaultImg">默认图片地址</param>
        /// <returns></returns>
        public static string ImageServerMaster(this UrlHelper h, string file,string defaultImg)
        {
            if (string.IsNullOrEmpty(file))
            {
                return GetServerPath(defaultImg);
            }
            if (file.IndexOf("http://", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return file;
            }
            return String.Format("{0}{1}", ImageServerMasterUrl, file);
        }

        public static string Res(this UrlHelper h, string file)
        {
            return File(h, "/res/" + file.Trim('/'));
        }
        public static string GetServerPath(string path)
        {
            return string.Format("{0}{1}", ImageServerHelper.Get(path), path);
        }

        /// <summary>
        /// 批量替换内容中绝对路径为相对路径
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertImgAbsoluteSrcToUnAbsolute(this String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            string Pattern = @"(?<=<img(?:(?!src=)[\s\S])*src=(""|'))[^""]+(?=""|')";//匹配img标签中的src
            Regex reg = new Regex(Pattern);
            return reg.Replace(s, new MatchEvaluator(GetUnAbsoluteImgUrl));
        }



        private static Regex sltregex = new Regex(@"/u/\d+-\d+-([aohwc])/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 批量替换内容中相对路径为绝对路径
        /// </summary>
        /// <param name="s"></param>
        /// <param name="height">内容缩略图高度</param>
        /// <param name="width">内容缩略图宽度</param>
        /// <returns></returns>
        public static string ConvertImgUnAbsoluteSrcToAbsolute(this String s, int width = 0, int height = 0)
        {
            string Pattern = @"(?<=<img(?:(?!src=)[\s\S])*src=(""|'))[^""]+(?=""|')";//匹配img标签中的src
            Regex reg = new Regex(Pattern);
            //return reg.Replace(s, new MatchEvaluator(GetAbsoluteImgUrl)).Replace("local.", "");
            return reg.Replace(s, new MatchEvaluator(match =>
            {
                if (string.IsNullOrEmpty(match.Value))
                {
                    return string.Empty;
                }
                var u = new Uri(match.Value, UriKind.RelativeOrAbsolute);
                if (u.IsAbsoluteUri)
                {
                    return match.Value;
                }

                var url = match.Value;

                if (!sltregex.Match(url).Success)
                {
                    url = string.Format("/u/{0}-{1}-c", width, height) + url + (HttpKit.IsSupportWebP() ? ".webp" : "");
                }
                return GetServerPath(url);
            }));
        }

        public static string GetUnAbsoluteImgUrl(Match match)
        {
            if (string.IsNullOrEmpty(match.Value))
            {
                return string.Empty;
            }
            try
            {
                var s = match.Value;

                var smatch = sltregex.Match(s);

                if (smatch.Success)
                {
                    s = s.Replace(smatch.Value, "/");
                }
                s = s.Replace(".webp", "");

                if (BaseConfig.Current.Url.ImageServers.Any(a => s.IndexOf(a) > -1))
                {
                    return new Uri(s, UriKind.RelativeOrAbsolute).AbsolutePath;
                }
                return s;
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 字符串内替换Img内容
        /// </summary>
        /// <param name="s"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static string ImgChange(this String s, string alt)
        {
            var titleregex = new Regex(" title=\"(((?!\")(.|\\s))*)\"", RegexOptions.IgnoreCase);
            if (!String.IsNullOrWhiteSpace(alt))
            {
                var regex = new Regex(@"<img(((?!>)(.|\s))*)>", RegexOptions.IgnoreCase);

                var matchs = regex.Matches(s);

                foreach (Match item in matchs)
                {
                    var str = item.Value;
                    if (str.IndexOf(" alt", StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        str = "<img alt=\"" + alt + "\" " + str.Replace("<img", "");
                    }

                    s = s.Replace(item.Value, str);
                }
            }

            if (titleregex.Match(s).Success)
            {
                s = titleregex.Replace(s, "");
            }
            return s;
        }
    }
}
