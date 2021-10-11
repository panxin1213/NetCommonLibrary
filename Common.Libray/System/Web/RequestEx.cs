using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web
{
    /// <summary>
    /// request扩展
    /// </summary>
    public static class RequestEx
    {
        /// <summary>
        /// 获取完整的URL地址，包括host
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetFullUrl(this HttpRequestBase request)
        {
            return string.Format("{0}://{1}{2}", request.IsSecureConnection ? "https" : "http", request.ServerVariables["HTTP_HOST"], request.RawUrl);
        }
        /// <summary>
        /// 获取完整的URL地址，包括host
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetFullUrl(this HttpRequest request)
        {
            return new HttpRequestWrapper(request).GetFullUrl();
        }
        /// <summary>
        /// 获取当前访问的域名跟目录 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRootUrl(this HttpRequestBase request)
        {
            return string.Format("{0}://{1}", request.IsSecureConnection ? "https" : "http", request.ServerVariables["HTTP_HOST"]);
        }
        /// <summary>
        /// 获取当前访问的域名跟目录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRootUrl(this HttpRequest request)
        {
            return new HttpRequestWrapper(request).GetRootUrl();
        }

        public static string ReplaceQueryValue(this HttpRequest request, string querykey, string replacevalue)
        {
            var url = request.Url.ToString();

            if (url.IndexOf("?") > -1)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }

            var param = "";
            if (!String.IsNullOrEmpty(replacevalue))
            {
                param = "?" + querykey + "=" + System.Web.HttpContext.Current.Server.UrlEncode(replacevalue);
            }

            foreach (var item in request.QueryString.AllKeys)
            {
                if (item.Equals(querykey, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var values = request.QueryString.GetValues(item);

                foreach (var citem in values)
                {
                    if (!String.IsNullOrEmpty(citem))
                    {
                        if (!String.IsNullOrEmpty(param))
                        {
                            param += "&";
                        }

                        param += item + "=" + System.Web.HttpContext.Current.Server.UrlEncode(citem);
                    }
                }
            }

            if (!String.IsNullOrEmpty(param) && param.IndexOf("?") == -1)
            {
                param = "?" + param;
            }

            if (!String.IsNullOrEmpty(param))
            {
                url += param;
            }

            return url;
        }

        public static string RemoveQueryByValue(this HttpRequest request, string value)
        {
            var url = request.Url.ToString();

            if (url.IndexOf("?") > -1)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }

            var param = "";

            foreach (var item in request.QueryString.AllKeys)
            {
                var values = request.QueryString.GetValues(item);

                foreach (var citem in values)
                {
                    if (!String.IsNullOrEmpty(citem) && !citem.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!String.IsNullOrEmpty(param))
                        {
                            param += "&";
                        }

                        param += item + "=" + System.Web.HttpContext.Current.Server.UrlEncode(citem);
                    }
                }
            }

            if (!String.IsNullOrEmpty(param) && param.IndexOf("?") == -1)
            {
                param = "?" + param;
            }

            if (!String.IsNullOrEmpty(param))
            {
                url += param;
            }

            return url;
        }
        
    }
}
