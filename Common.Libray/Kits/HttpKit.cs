namespace ChinaBM.Common
{
    using System;
    using System.IO;
    using System.Web;
    using System.Net;
    using System.Text;
    using System.Configuration;
    using System.Threading;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///  Http工具类
    /// </summary>
    public static class HttpKit
    {
        #region IsPost 当前请求是否是Post请求
        /// <summary>
        ///  当前请求是否是Post请求
        /// </summary>
        public static bool IsPost
        {
            get { return HttpContext.Current.Request.HttpMethod.Equals("POST"); }
        }
        #endregion

        #region IsGet 当然请求是否是Get请求
        /// <summary>
        ///  当然请求是否是Get请求
        /// </summary>
        public static bool IsGet
        {
            get { return HttpContext.Current.Request.HttpMethod.Equals("GET"); }
        }
        #endregion

        #region IsAjax 当前请求是否是Ajax请求
        /// <summary>
        ///  当前请求是否是Ajax请求
        /// </summary>
        public static bool IsAjax
        {
            get
            {
                string xmlRequestString = HttpContext.Current.Request.Headers["X-Requested-With"];
                if (string.IsNullOrEmpty(xmlRequestString))
                {
                    return false;
                }
                return xmlRequestString.Equals("XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
            }
        }
        #endregion

        #region IsBrowserRequest 当前请求是否来自浏览器
        /// <summary>
        /// 当前请求是否来自浏览器
        /// </summary>
        public static bool IsBrowserRequest
        {
            get
            {
                string[] browserNames = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
                string currentBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
                foreach (string browserName in browserNames)
                {
                    if (currentBrowser.IndexOf(browserName) >= 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        #region IsSearchEngineRequest 当前请求是否来自搜索引擎
        /// <summary>
        /// 当前请求是否来自搜索引擎
        /// </summary>
        public static bool IsSearchEngineRequest
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer == null)
                {
                    return false;
                }
                string[] searchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
                string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
                for (int i = 0; i < searchEngine.Length; i++)
                {
                    if (tmpReferrer.IndexOf(searchEngine[i]) >= 0)
                        return true;
                }
                return false;
            }
        }
        #endregion

        #region RawUrl 当前请求的原始Url
        /// <summary>
        /// 当前请求的原始Url(Url中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        public static string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }
        #endregion

        #region Url 获取当前的Url信息
        /// <summary>
        /// 获取当前的Url
        /// </summary>
        public static string Url
        {
            get { return HttpContext.Current.Request.Url.ToString(); }
        }
        #endregion

        #region UrlReferrer 上一个请求地址
        /// <summary>
        /// 上一个请求地址
        /// </summary>
        public static string UrlReferrer
        {
            get
            {
                string urlReferrer;
                try
                {
                    urlReferrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }
                catch
                {
                    return string.Empty;
                }
                return urlReferrer;
            }
        }
        #endregion

        #region 请求当前url的PathAndQuery

        public static string PathAndQuery
        {
            get { return HttpContext.Current.Request.Url.PathAndQuery; }
        }

        #endregion

        #region CurrentFullUrl 当前请求完整的Url
        /// <summary>
        /// 当前请求完整的Url
        /// </summary>
        public static string CurrentFullUrl
        {
            get { return HttpContext.Current.Request.Url.ToString(); }
        }
        #endregion

        #region CurrentHost 当前请求主机头
        /// <summary>
        /// 当前请求主机头
        /// </summary>
        public static string CurrentHost
        {
            get { return HttpContext.Current.Request.Url.Host; }
        }
        #endregion

        #region CurrentFullHost 当前请求完整主机头
        /// <summary>
        /// 当前请求完整主机头
        /// </summary>
        public static string CurrentFullHost
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                if (!request.Url.IsDefaultPort)
                {
                    return string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
                }
                return request.Url.Host;
            }
        }
        #endregion

        #region CurrentPageName 当前请求页面的名称
        /// <summary>
        /// 当前请求页面的名称
        /// </summary>
        public static string CurrentPageName
        {
            get
            {
                string[] urlArray = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                return urlArray[urlArray.Length - 1].ToLower();
            }
        }
        #endregion

        #region CurrentRequestIP 当前请求的IP地址
        /// <summary>
        /// 当前请求的IP地址
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string CurrentRequestIP
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null)
                {
                    return null;
                }

                string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result) || !ValidateKit.IsIP(result))
                {
                    return "127.0.0.1";
                }
                return result;
            }
        }
        #endregion

        #region WriteToEnd 向Http响应流中写入文本并立即终止响应
        /// <summary>
        ///  WriteToEnd 向Http响应流中写入文本并立即终止响应
        /// </summary>
        /// <param name="content">写入对象</param>
        public static void WriteToEnd(object content)
        {
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.End();
        }
        #endregion

        #region Redirect 请求重定向
        /// <summary>
        ///  请求重定向
        /// </summary>
        /// <param name="url">跳转Url地址</param>
        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url, true);
        }
        #endregion

        #region GetServerVariable 返回指定的服务器变量信息
        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="variableName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerVariable(string variableName)
        {
            if (HttpContext.Current.Request.ServerVariables[variableName] == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.ServerVariables[variableName];
        }
        #endregion

        #region GetUrlParam 获取指定Url参数的值
        /// <summary>
        ///  获取指定Url参数的值
        /// </summary>
        /// <param name="key">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetUrlParam(string key)
        {
            return GetUrlParam(key, false);
        }
        #endregion

        #region GetUrlParam 获取指定Url参数的值
        /// <summary>
        /// 获取指定Url参数的值
        /// </summary> 
        /// <param name="key">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行Sql安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetUrlParam(string key, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[key] == null)
            {
                return string.Empty;
            }
            if (sqlSafeCheck && !ValidateKit.IsSafeSqlString(HttpContext.Current.Request.QueryString[key]))
            {
                //return "unsafe string";
                return string.Empty;
            }
            return HttpContext.Current.Request.QueryString[key];
        }
        #endregion

        #region UrlParamCount 当前请求Url参数数量
        /// <summary>
        ///  当前请求Url参数数量
        /// </summary>
        public static int UrlParamCount
        {
            get { return HttpContext.Current.Request.QueryString.Count; }
        }
        #endregion

        #region GetFormParam 获取指定表单参数的值
        /// <summary>
        /// 获取指定表单参数的值
        /// </summary>
        /// <param name="key">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormParam(string key)
        {
            return GetFormParam(key, false);
        }
        #endregion

        #region GetFormParam 获取指定表单参数的值
        /// <summary>
        /// 获取指定表单参数的值
        /// </summary>
        /// <param name="key">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行Sql安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormParam(string key, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[key] == null)
            {
                return string.Empty;
            }
            if (sqlSafeCheck && !ValidateKit.IsSafeSqlString(HttpContext.Current.Request.Form[key]))
            {
                //return "unsafe string";
                return string.Empty;
            }
            return HttpContext.Current.Request.Form[key];
        }
        #endregion

        #region FormParamCount 当前请求表单参数数量
        /// <summary>
        ///  当前请求表单参数数量
        /// </summary>
        public static int FormParamCount
        {
            get { return HttpContext.Current.Request.Form.Count; }
        }
        #endregion

        #region WriteCookie 完整写入Cookie值
        /// <summary>
        /// 完整写入Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        public static void WriteCookie(string cookieName, string cookieValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region WriteCookie 完整写入Cookie值
        /// <summary>
        /// 完整写入Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="expires">Cookie过期时间(分钟)</param>
        public static void WriteCookie(string cookieName, string cookieValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region WriteCookieValue 写入Cookie单个值
        /// <summary>
        /// 写入Cookie单个值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">Cookie键</param>
        /// <param name="value">值</param>
        public static void WriteCookieValue(string cookieName, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie[key] = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region WriteCookieValue 写入Cookie单个值
        /// <summary>
        /// 写入Cookie单个值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">Cookie键</param>
        /// <param name="value">值</param>
        /// <param name="expires">Cookie过期时间(分钟)</param>
        public static void WriteCookieValue(string cookieName, string key, string value, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
            cookie[key] = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region GetCookie 读取Cookie完整值
        /// <summary>
        /// 读取Cookie完整值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null)
                {
                    return HttpUtility.UrlDecode(cookie.Value);
                }
            }
            return string.Empty;
        }
        #endregion

        #region GetCookieValue 读取Cookie某一值
        /// <summary>
        /// 读取Cookie某一值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">键名</param>
        /// <returns>值</returns>
        public static string GetCookieValue(string cookieName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie != null)
                {
                    return cookie[key];
                }
            }
            return string.Empty;
        }
        #endregion

        #region GetMapPath 获得当前绝对路径

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                if (path.IndexOf("?") > -1)
                {
                    path = path.Substring(0, path.IndexOf("?"));
                }

                return HttpContext.Current.Server.MapPath(path);
            }
            path = path.Replace("/", "\\").Trim('\\');
            //if (path.StartsWith("\\"))
            //{
            //    path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
            //}
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            //return @"D:\web\SmallGame\web\" + path;
        }

        #endregion

        #region RemoveCookie

        public static void RemoveCookie(string cookieName)
        {
            HttpContext.Current.Request.Cookies.Remove(cookieName);
        }

        #endregion

        #region 模拟静态文件

        public static void AnalogStaitc(DateTime etagtime)
        {
            var lasttime = etagtime.ToDateTime(DateTime.Now.Date);

            System.Web.HttpContext.Current.Response.ClearHeaders();
            //模拟静态文件
            string eTag = "\"" + lasttime.Ticks.ToString() + "\"";
            string last = String.Format("{0:R}", lasttime.ToUniversalTime());
            if (eTag.Equals(System.Web.HttpContext.Current.Request.Headers["If-None-Match"]) || last.Equals(System.Web.HttpContext.Current.Request.Headers["If-Modified-Since"]))
            {
                System.Web.HttpContext.Current.Response.Status = "304 Not Modified";
                //Response.End();
            }
            else
            {
                System.Web.HttpContext.Current.Response.AddHeader("ETag", eTag);
#if !DEBUG
                    System.Web.HttpContext.Current.Response.AddHeader("Expires", String.Format("{0:R}", DateTime.Now.AddDays(1).ToUniversalTime()));
#endif
                System.Web.HttpContext.Current.Response.AddHeader("Last-Modified", last);
            }
        }

        #endregion

        #region 生成静态文件

        public static bool ishtml = true;

        /// <summary>
        /// 生成静态文件方法
        /// </summary>
        /// <param name="islist">是否是列表页</param>
        /// <param name="ext">文件后缀</param>
        /// <param name="lasttime">文件最后更新时间，配合IStaticHtml接口一起使用</param>
        /// <returns></returns>
        public static bool MakeHtml(bool islist = false, string ext = ".htm", DateTime? lasttime = null)
        {
            if ((HttpContext.Current.Request.RawUrl.IndexOf(ext) > -1 || HttpContext.Current.Request.RawUrl.IndexOf(".") == -1) && HttpContext.Current.Request.RawUrl.IndexOf(ext + "?make") == -1)
            {
                var url = "";
                HttpKit.AnalogStaitc(lasttime != null ? lasttime.Value : DateTime.Now.Date);
                if (lasttime != null)
                {
                    url = HttpContext.Current.Request.RawUrl;

                    try
                    {
                        var staticurl = url.IndexOf(ext) > -1 ? url : (url.IndexOf(".") > -1 ? url : ("/" + url.Trim('/') + "/index" + ext));

                        var filepath = HttpKit.GetMapPath(staticurl);

                        if (File.Exists(filepath))
                        {
                            var file = new FileInfo(filepath);
                            if (file.LastWriteTime >= lasttime || File.Exists(HttpKit.GetMapPath("/error/nohtml.txt")))
                            {
                                using (StreamReader reader = new StreamReader(file.OpenRead()))
                                {
                                    HttpContext.Current.Response.Write(reader.ReadToEnd());
                                    HttpContext.Current.Response.End();
                                    return true;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }

                url = HttpContext.Current.Request.RawUrl.IndexOf(ext) > -1 ? HttpContext.Current.Request.RawUrl : "/" + HttpContext.Current.Request.RawUrl.Trim('/') + "/index" + ext;
                var path = HttpKit.GetMapPath(url);
                DirectoryEx.CreateFolder(path);

                using (WebClient client = new WebClient())
                {
                    var size = 0;
                    while (size < 9)
                    {
                        try
                        {
                            client.Encoding = Encoding.UTF8;
                            var s = client.DownloadString("http://" + ConfigurationManager.AppSettings["domain"] + (islist ? url + "?make" : HttpContext.Current.Request.Url.PathAndQuery));

                            using (StreamWriter writer = new StreamWriter(path, false))
                            {
                                writer.Write(s);
                                writer.Close();
                            }
                            HttpContext.Current.Response.Write(s);
                            HttpContext.Current.Response.End();
                            return true;
                        }
                        catch
                        {
                            Thread.Sleep(100);
                            size++;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region HttpPost提交页面

        public static string HttpPostSubmit(string subinfo, string url)
        {
            var cookie = "";

            return HttpPostSubmit(subinfo, url, ref cookie);
        }


        public static string HttpPostSubmit(string subinfo, string url, ref string cookies, string useragent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36", string referer = "", Dictionary<string, string> headers = null, Encoding encoding = null)
        {
            encoding = encoding == null ? Encoding.UTF8 : encoding;
            using (var client = new GZipWebClient())
            {
                //WebProxy proxy = new WebProxy();
                //proxy.UseDefaultCredentials = false;
                //proxy.Address = new Uri("http://127.0.0.1:8888"); // new Uri("http://183.239.167.122:8080");
                //client.Proxy = proxy;

                if (!String.IsNullOrWhiteSpace(referer))
                {
                    client.Headers.Add("Referer", referer);
                }
                if (!String.IsNullOrWhiteSpace(cookies))
                {
                    client.Headers.Add("Cookie", cookies);
                }

                if (headers != null && headers.Count > 0)
                {
                    foreach (var item in headers)
                    {
                        client.Headers.Add(item.Key, item.Value);
                    }
                }
                client.Headers.Add("Accept-Encoding", "gzip, deflate");
                client.Headers.Add("User-Agent", useragent);
                byte[] postData = encoding.GetBytes(subinfo);
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                byte[] responseData = client.UploadData(url, "POST", postData);
                
                var s = encoding.GetString(responseData);//解码

                cookies = CookieBind(cookies, client.ResponseHeaders["Set-Cookie"]);

                return s;
            }
        }

        /// <summary>
        /// 合并SetCookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="setCookies"></param>
        /// <returns></returns>
        public static string CookieBind(string cookies, string setCookies)
        {
            var setCookie = BindSetCookie(setCookies);

            if (String.IsNullOrWhiteSpace(cookies))
            {
                cookies = string.Join(";", setCookie.Split(';').Distinct());
            }
            else
            {
                var cookiesdic = CookieToDic(cookies);

                var setCookiedic = CookieToDic(setCookie);

                foreach (var item in setCookiedic)
                {
                    if (!String.IsNullOrWhiteSpace(item.Key))
                    {
                        cookiesdic[item.Key] = item.Value;
                    }
                }
                cookies = string.Join(";", cookiesdic.Select(a => !String.IsNullOrWhiteSpace(a.Value) ? a.Key + "=" + a.Value : a.Key));
            }

            return cookies;
        }
        
        private static Regex setCookieRegex = new Regex(@"(Path=/(;|,)?)|(Expires=(((?!;).)*);)|(HttpOnly(;|,)?)|(domain=(((?!(,|$|;)).)*)(,|$|;))|(userHit=true(;|,)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static string BindSetCookie(string setCookie)
        {
            if (setCookie == null)
            {
                return "";
            }
            var thissetCookie = setCookieRegex.Replace(setCookie, "");

            return string.Join(";", thissetCookie.Split(';').Select(a => a.Trim()));
        }

        private static Dictionary<string, string> CookieToDic(string cookies)
        {
            return cookies.Split(';').Select(a =>
            {
                if (a.IndexOf("=") > -1)
                {
                    return new KeyValuePair<string, string>(a.Split('=')[0], a.Split('=')[1]);
                }
                else
                {
                    return new KeyValuePair<string, string>(a, "");
                }
            }).GroupBy(a => a.Key).ToDictionary(a => a.Key, a => a.FirstOrDefault().Value);
        }

        #endregion



        #region HttpPost提交页面
        /// <summary>
        /// Post提交方法
        /// </summary>
        /// <param name="subinfo">提交数据内容</param>
        /// <param name="url">提交地址</param>
        /// <param name="contenttype">内容类型</param>
        /// <param name="async">是否异步</param>
        /// <param name="callback">异步回调函数</param>
        /// <returns></returns>
        public static string HttpPostSubmit(string subinfo, string url, string contenttype = "application/x-www-form-urlencoded", bool async = false, Action<object, UploadDataCompletedEventArgs> callback = null)
        {
            using (var client = new WebClient())
            {
                byte[] postData = Encoding.UTF8.GetBytes(subinfo);
                client.Headers.Add("Content-Type", contenttype);
                byte[] responseData = null;

                if (!async)
                {
                    responseData = client.UploadData(url, "POST", postData);
                }
                else
                {
                    client.UploadDataAsync(new Uri(url), "POST", postData, null);
                    client.UploadDataCompleted += new UploadDataCompletedEventHandler(callback);
                    return string.Empty;
                }

                return Encoding.UTF8.GetString(responseData);//解码
            }
        }

        #endregion

        #region 百度推送方法

        /// <summary>
        /// 百度推送方法
        /// </summary>
        /// <param name="urls">推送url集合</param>
        /// <returns></returns>
        public static int BaiDuPushPost(List<string> urls, out List<string> no_urls)
        {
            no_urls = new List<string>();
            if (urls == null || urls.Count == 0)
            {
                return 0;
            }

            var d = new Dictionary<string, List<string>>();

            foreach (var item in urls)
            {
                var doamin = item.Replace("http://", "");
                if (doamin.IndexOf("/") > -1)
                {
                    doamin = doamin.Substring(0, doamin.IndexOf("/"));
                }
                if (d.ContainsKey(doamin))
                {
                    d[doamin].Add(item);
                }
                else
                {
                    d.Add(doamin, new List<string> { item });
                }
            }

            var num = 0;
            foreach (var item in d)
            {
                var ol = new List<string>();
                num += BaiDuPushPost(item.Value, item.Key, out ol);
                no_urls.AddRange(ol);

            }
            return num;
        }

        /// <summary>
        /// 百度推送方法
        /// </summary>
        /// <param name="urls">推送url集合</param>
        /// <param name="domain">push的域名</param>
        /// <param name="no_urls">推送失败的url</param>
        /// <returns></returns>
        public static int BaiDuPushPost(List<string> urls, string domain, out List<string> no_urls)
        {
            no_urls = new List<string>();
            if (String.IsNullOrWhiteSpace(domain) || urls == null || urls.Count == 0)
            {
                return 0;
            }

            var posturl = System.Configuration.ConfigurationManager.AppSettings["BaiDuPushUrl"];

            if (String.IsNullOrWhiteSpace(posturl))
            {
                throw new Exception("web.config 中没有配置相应的 BaiDuPushUrl,请在appSettings中配置");
            }

            posturl += "&site=" + domain;

            if (domain.StartsWith("mip."))
            {
                posturl += "&type=mip";
            }

            try
            {

                var d = HttpPostSubmit(string.Join("\n", urls), posturl, "text/plain").JsonToDictionary();
                if (d.ContainsKey("not_same_site"))
                {
                    no_urls = (d["not_same_site"] as object[]).Select(a => a.ToSafeString()).ToList();

                    global::Common.Library.Log.Logger.Error("BaiDuPushPost", "BaiDuPushPost:" + d.ToJson(), null);
                }
                return d["success"].ToInt();
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("400") == -1)
                {
                    global::Common.Library.Log.Logger.Error("BaiDuPushPost", e.Message, e);
                }
                no_urls = urls;
                global::Common.Library.Log.Logger.Error("BaiDuPushPost", "BaiDuPushPost:posturl:" + posturl + ":" +e.Message, e);
                return 0;
            }
        }

        #endregion


        /// <summary>
        /// 是否支持webp
        /// </summary>
        /// <returns></returns>
        public static bool IsSupportWebP()
        {
            try
            {
                var r = System.Web.HttpContext.Current.Items["IsSupport"] as bool?;

                if (r == null)
                {
                    r = System.Web.HttpContext.Current.Request.Headers["Accept"].IndexOf("image/webp") > -1;
                    System.Web.HttpContext.Current.Items.Add("IsSupport", r);
                }

                return r.Value;
            }
            catch
            {
                return false;
            }
        }



        private static readonly Regex ismobileregex = new Regex(@"(phone|pod|iPhone|iPod|ios|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsMobile()
        {
            var ua = System.Web.HttpContext.Current.Request.UserAgent ?? "";

            return (ismobileregex.IsMatch(ua))
                        && ua.IndexOf("ipad", StringComparison.OrdinalIgnoreCase) == -1;
        }



        /// <summary>
        /// 判断url是否可以打开
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlCanOpen(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "head";
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 获取当前请求的useragent
        /// </summary>
        public static string CurrentRequestUserAgent
        {
            get
            {
                return HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.UserAgent : "";
            }
        }





        #region 随机获取useragent



        private static Regex reg = new Regex(@"\d{3,}", RegexOptions.Compiled);
        /// <summary>
        /// 把三位数字随机一下
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string matchEval(Match m)
        {
            if (m.Success)
            {
                int v = Int32.Parse(m.Value);
                return (new Random(Guid.NewGuid().GetHashCode()).Next(v / 2, v + 1)).ToString();

            }
            return null;
        }

        public static string GetNewUserAgent(bool random = false)
        {
            var r = useragent[new Random(Guid.NewGuid().GetHashCode()).Next(useragent.Length)];
            if (random)
                r = reg.Replace(r, new MatchEvaluator(matchEval));
            return r;
        }

        private static string[] useragent = new string[]{"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36 TheWorld 6",
                "Mozilla/5.0 (Windows NT 6.2; WOW64; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_1) AppleWebKit/604.3.5 (KHTML, like Gecko) Version/11.0.1 Safari/604.3.5",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.221 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36 MicroMessenger/6.5.2.501 NetType/WIFI WindowsWechat QBCore/3.43.901.400 QQBrowser/9.0.2524.400",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36 WnBrowser/2.0",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.4.3.17934",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6756.400 QQBrowser/10.3.2473.400",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A404 baiduboxapp/11.0.0.12 (Baidu; P2 12.0.1)",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.4.2.17629",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (Linux; U; Android 6.0.1; zh-cn; OPPO R9s Build/MMB29M) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.134 Mobile Safari/537.36 OppoBrowser/4.8.1",
                "Mozilla/5.0 (Linux; Android 7.1.2; vivo X9 Build/N2G47H; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/6538 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4094.1 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.5.0.17997",
                "Mozilla/5.0 (Linux; Android 8.1.0; BLA-AL00 Build/HUAWEIBLA-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.98 Safari/537.36 LBBROWSER",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E216 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6776.400 QQBrowser/10.3.2577.400",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.108 Safari/537.36 2345Explorer/8.8.3.16721",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/604.3.5 (KHTML, like Gecko) Version/12.0 MQQBrowser/8.7.1 Mobile/15B87 Safari/604.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11t Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/9190 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.221 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A404 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.104 Safari/537.36 Core/1.53.3485.400 QQBrowser/9.6.12190.400",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36 MicroMessenger/6.5.2.501 NetType/WIFI WindowsWechat QBCore/3.43.691.400 QQBrowser/9.0.2524.400",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Mobile/14G60 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.1; COL-AL10 Build/HUAWEICOL-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/2390 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/7735 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; INE-AL00 Build/HUAWEIINE-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1480 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; PBEM00 Build/OPM1.171019.026; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/7576 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4094.1 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 BIDUBrowser/6.x Safari/537.31",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (Linux; Android 8.0; VTR-AL00 Build/HUAWEIVTR-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/188 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; vivo X20A Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.6.7.1321(0x26060736) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.79 Safari/535.11 QIHU THEWORLD",
                "Mozilla/5.0 (Linux; Android 8.1.0; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/67.0.3396.87 XWEB/461 MMWEBSDK/180803 Mobile Safari/537.36 MMWEBID/76 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36 MicroMessenger/6.5.2.501 NetType/WIFI WindowsWechat QBCore/3.43.884.400 QQBrowser/9.0.2524.400",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.5.0.17997",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.98 Safari/537.36 LBBROWSER",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/6167 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.221 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6776.400 QQBrowser/10.3.2601.400",
                "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15G77 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15G77 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4094.1 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A366 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.3.2.17331",
                "Mozilla/5.0 (Linux; Android 7.1.2; vivo X9 Build/N2G47H; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/6538 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/602.3.12 (KHTML, like Gecko) Mobile/14C92 MicroMessenger/6.7.2 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0.1 Safari/605.1.15",
                "Mozilla/5.0 (Linux; Android 8.0; WAS-AL00 Build/HUAWEIWAS-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1344 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.3.2.17331",
                "Mozilla/5.0 (Linux; Android 5.0.2; Redmi Note 3 Build/LRX22G; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/9030 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; EVA-AL10 Build/HUAWEIEVA-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/5717 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.0; SM-G9200 Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1090 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15G77 MicroMessenger/6.7.2 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 4.4.4; OPPO R7s Build/KTU84P; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/2334 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_1) AppleWebKit/602.2.14 (KHTML, like Gecko) Version/10.0.1 Safari/602.2.14 QQBrowserLite/1.2.1",
                "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/5722 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.4.3.17934",
                "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36 TheWorld 6",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 BIDUBrowser/8.7 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Safari/605.1.15",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/602.3.12 (KHTML, like Gecko) Mobile/14C92 MicroMessenger/6.7.2 NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 7.0; MI MAX Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; HUAWEI NXT-CL00 Build/HUAWEINXT-CL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/2083 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11s Plus Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/5682 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/6669 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; MHA-AL00 Build/HUAWEIMHA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/4063 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.5.0.17997",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6756.400 QQBrowser/10.3.2565.400",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_1_1 like Mac OS X) AppleWebKit/602.2.14 (KHTML, like Gecko) Version/10.0 Mobile/14B100 Safari/602.1",
                "Mozilla/5.0 (Linux; Android 8.0.0; VIE-AL10 Build/HUAWEIVIE-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/11.0 baiduboxapp/11.0.0.11 (Baidu; P1 8.0.0)",
                "Mozilla/5.0 (Linux; Android 8.1; vivo Z1i Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044208 Mobile Safari/537.36 MicroMessenger/6.6.2.1240(0x26060235) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E302 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko Core/1.53.3427.400 QQBrowser/9.6.12449.400",
                "Mozilla/5.0 (Linux; U; Android 6.0.1; zh-cn; OPPO R9s Build/MMB29M) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.134 Mobile Safari/537.36 OppoBrowser/4.7.2",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 BIDUBrowser/8.7 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.0; PRA-AL00 Build/HONORPRA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/4688 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; COL-AL10 Build/HUAWEICOL-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/2390 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.5.0.17997",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6776.400 QQBrowser/10.3.2601.400",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.52",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 BIDUBrowser/8.7 Safari/537.36",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                "Mozilla/5.0 (Linux; U; Android 7.1.2; zh-cn; MI 5X Build/N2G47H) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.128 Mobile Safari/537.36 XiaoMi/MiuiBrowser/10.2.2",
                "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; FRD-AL10 Build/HUAWEIFRD-AL10) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.8.998 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; MI MAX Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; PRA-AL00 Build/HONORPRA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/4688 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0.1 Safari/605.1.15",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15G77 MicroMessenger/6.7.1 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A404 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 5.0.2; Redmi Note 3 Build/LRX22G; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/9030 MicroMessenger/6.7.3.1360(0x26070338) NetType/3gnet Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.5702.400 QQBrowser/10.2.1893.400",
                "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-cn; Mi Note 3 Build/OPM1.171019.019) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.128 Mobile Safari/537.36 XiaoMi/MiuiBrowser/10.2.2",
                "Mozilla/5.0 (Linux; Android 7.1.1; MP1602 Build/NMF26O; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/9115 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 MQQBrowser/8.9.1 Mobile/15E148 Safari/604.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.4.2.17629",
                "Mozilla/5.0 (Linux; Android 8.1.0; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/67.0.3396.87 XWEB/461 MMWEBSDK/180803 Mobile Safari/537.36 MMWEBID/76 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.108 Safari/537.36 UCBrowser/12.1.8.998",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/16A404 UCBrowser/12.1.7.1109 Mobile  AliApp(TUnionSDK/0.1.20.3)",
                "Mozilla/5.0 (Linux; Android 7.1.1; MEIZU E3 Build/NGI77B; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/51.0.2704.110 Mobile Safari/537.36 MMWEBID/9807 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; DUK-AL20 Build/HUAWEIDUK-AL20; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1599 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11t Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/9190 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; INE-TL00 Build/HUAWEIINE-TL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/8264 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 10.0; Trident/7.0; rv:11.0) like Gecko",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15G77 MicroMessenger/6.7.1 NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 5.0.2; Redmi Note 3 Build/LRX22G; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/9030 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A404 QQ/7.8.8.420 V1_IPH_SQ_7.8.8_1_APP_A Pixel/750 Core/WKWebView Device/Apple(iPhone 7) NetType/4G QBWebViewType/1 WKType/1",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.5790.400 QQBrowser/10.1.2082.400",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/3807 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/5722 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-CN; MI 8 SE Build/OPM1.171019.019) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.7.997 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.4.3.17879",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6776.400 QQBrowser/10.3.2601.400",
                "Mozilla/5.0 (Linux; Android 7.0; VIE-AL10 Build/HUAWEIVIE-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/11.0 baiduboxapp/11.0.0.11 (Baidu; P1 7.0)",
                "Mozilla/5.0 (Linux; Android 6.0; Redmi Note 4 Build/MRA58K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/157 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; LDN-AL00 Build/HUAWEILDN-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1258 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Safari/605.1.15",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0) Core/1.63.6776.400 QQBrowser/10.3.2577.400",
                "Mozilla/5.0 (Linux; Android 8.0; LLD-AL10 Build/HONORLLD-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/8211 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6721.400 QQBrowser/10.2.2535.400",
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; TRT-AL00A Build/HUAWEITRT-AL00A; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/2894 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6726.400 QQBrowser/10.2.2265.400",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1142 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; SM-G9500 Build/R16NW; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/7819 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; EVA-AL10 Build/HUAWEIEVA-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/602.3.12 (KHTML, like Gecko) Mobile/14C92 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 7.1.1; MP1602 Build/NMF26O; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6756.400 QQBrowser/10.3.2565.400",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) maxthon/4.9.4.3000 Chrome/39.0.2146.0 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0; Redmi Note 4 Build/MRA58K; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 4.4.4; OPPO R7s Build/KTU84P; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0.0; LLD-AL10 Build/HONORLLD-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/11.0 baiduboxapp/11.0.0.11 (Baidu; P1 8.0.0)",
                "Mozilla/5.0 (Linux; Android 6.0.1; OPPO R9sk Build/MMB29M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/1142 MicroMessenger/6.7.3.1360(0x26070338) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11t Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.0; SM-G9200 Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; LLD-AL10 Build/HONORLLD-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.104 Safari/537.36 Core/1.53.4882.400 QQBrowser/9.7.13059.400",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6726.400 QQBrowser/10.2.2265.400",
                "Mozilla/5.0 (Linux; Android 7.1.2; vivo X9 Build/N2G47H; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MMWEBID/3807 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.108 Safari/537.36 2345Explorer/8.8.0.16453",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.143 Crosswalk/24.53.595.0 XWEB/359 MMWEBSDK/180803 Mobile Safari/537.36 MMWEBID/76 MicroMessenger/6.7.3.1360(0x26070338) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Android 8.1.0; Mobile; rv:63.0) Gecko/63.0 Firefox/63.0",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.13; rv:63.0) Gecko/20100101 Firefox/63.0",
                "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-cn; Mi Note 2 Build/OPR1.170623.032) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.128 Mobile Safari/537.36 XiaoMi/MiuiBrowser/10.2.2",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.90 Safari/537.36 2345Explorer/9.3.2.17331",
                "Mozilla/5.0 (Linux; Android 8.0; VTR-AL00 Build/HUAWEIVTR-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:63.0) Gecko/20100101 Firefox/63.0",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36",
                "Mozilla/5.0 (iPad; CPU OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A404 AliApp(DingTalk/4.5.11) com.laiwang.DingTalk/10450412 Channel/201200 Pad/iPad language/zh-Hans-CN",
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                "Mozilla/5.0 (Linux; Android 7.0; MI MAX Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-cn; vivo X21A Build/OPM1.171019.011) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/8.9 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.1; vivo X21A Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 6.0.1; vivo Y66 Build/MMB29M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 MQQBrowser/8.9.1 Mobile/13B143 Safari/601.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Plusk Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/15G77 UCBrowser/12.1.7.1109 Mobile  AliApp(TUnionSDK/0.1.20.3)",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 Mobile/14G60 Safari/602.1",
                "Mozilla/5.0 (iPhone 6s; CPU iPhone OS 10_3_3 like Mac OS X) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.0 MQQBrowser/7.2.1 Mobile/14G60 Safari/8536.25 MttCustomUA/2 QBWebViewType/1",
                "Mozilla/5.0 (Linux; Android 7.1.2; vivo X9 Build/N2G47H; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; HUAWEI NXT-CL00 Build/HUAWEINXT-CL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15F79 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 7.0; HUAWEI NXT-DL00 Build/HUAWEINXT-DL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x2607036C) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; PRA-AL00 Build/HONORPRA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; INE-AL00 Build/HUAWEIINE-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E302 MicroMessenger/6.7.2 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_3 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/14G60 UCBrowser/12.0.2.1075 Mobile  AliApp(TUnionSDK/0.1.20.3)",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.143 Crosswalk/24.53.595.0 XWEB/359 MMWEBSDK/180801 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; LDN-AL00 Build/HUAWEILDN-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_0_2 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/14A456 UCBrowser/12.1.7.1109 Mobile  AliApp(TUnionSDK/0.1.20.3)",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_4) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.1 Safari/605.1.15",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; COL-AL10 Build/HUAWEICOL-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; PBEM00 Build/OPM1.171019.026; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; SM-G9500 Build/R16NW; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; BLA-TL00 Build/HUAWEIBLA-TL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; ALP-AL00 Build/HUAWEIALP-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044373 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6756.400 QQBrowser/10.3.2473.400",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15F79 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 5.0.2; Redmi Note 3 Build/LRX22G; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.0; HUAWEI NXT-AL10 Build/HUAWEINXT-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.6.7.1321(0x26060736) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 4.4.4; OPPO R7s Build/KTU84P; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; WAS-AL00 Build/HUAWEIWAS-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; PRA-AL00 Build/HONORPRA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16B92 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 5.0.2; Redmi Note 3 Build/LRX22G; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; BLA-TL00 Build/HUAWEIBLA-TL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-CN; COL-AL10 Build/HUAWEICOL-AL10) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.0.5.985 Mobile Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_1_2 like Mac OS X) AppleWebKit/604.3.5 (KHTML, like Gecko) Mobile/15B202 MicroMessenger/6.6.5 NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; VKY-AL00 Build/HUAWEIVKY-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.8.998 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; HUAWEI NXT-AL10 Build/HUAWEINXT-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/11.0 baiduboxapp/11.0.0.11 (Baidu; P1 7.0)",
                "Mozilla/5.0 (Linux; U; Android 7.1.1; zh-cn; OPPO R11 Build/NMF26X) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.134 Mobile Safari/537.36 OppoBrowser/4.7.2",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.26 Safari/537.36 Core/1.63.6726.400 QQBrowser/10.2.2265.400",
                "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; MHA-AL00 Build/HUAWEIMHA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-cn; DUK-AL20 Build/HUAWEIDUK-AL20) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/8.9 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; SM-G9500 Build/R16NW) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.8.998 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11t Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.0; MI MAX Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A366 MicroMessenger/6.6.6 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.143 Crosswalk/24.53.595.0 XWEB/359 MMWEBSDK/180801 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; COL-AL10 Build/HUAWEICOL-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_2 like Mac OS X) AppleWebKit/602.3.12 (KHTML, like Gecko) Mobile/14C92 MicroMessenger/6.7.3(0x16070321) NetType/3G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.0; MHA-AL00 Build/HUAWEIMHA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; vivo X21A Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.0; EVA-AL10 Build/HUAWEIEVA-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070337) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.98 Safari/537.36 LBBROWSER",
                "Mozilla/5.0 (Linux; Android 8.0; LLD-AL10 Build/HONORLLD-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; ALP-AL00 Build/HUAWEIALP-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044373 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 4.4.4; OPPO R7s Build/KTU84P; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044304 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; LLD-AL10 Build/HONORLLD-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; MHA-AL00 Build/HUAWEIMHA-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.8.998 Mobile Safari/537.36",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_1_2 like Mac OS X) AppleWebKit/604.3.5 (KHTML, like Gecko) Mobile/15B202 MicroMessenger/6.7.3(0x16070321) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 8.1; MI 8 SE Build/OPM1.171019.019; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 6.0.1; vivo Y55 Build/MMB29M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070333) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 7.1.1; zh-CN; MI MAX 2 Build/NMF26F) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.8.998 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.0; PRA-AL00 Build/HONORPRA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 11_1_2 like Mac OS X) AppleWebKit/604.3.5 (KHTML, like Gecko) Mobile/15B202 MicroMessenger/6.6.5 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; U; Android 8.0.0zh-cn; BKL-AL20 Build/HUAWEIBKL-AL20) AppleWebKit/537.36 (KHTML, like Gecko)Version/4.0 Chrome/57.0.2987.132 MQQBrowser/8.1 Mobile Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; COL-AL10 Build/HUAWEICOL-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)",
                "Mozilla/5.0 (Linux; Android 8.1; Redmi Note 5 Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 5.1; CUN-AL00 Build/HUAWEICUN-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Plusk Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko Core/1.53.4620.400 QQBrowser/9.7.13014.400",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.1.2 Safari/605.1.15",
                "Mozilla/5.0 (Linux; Android 8.1; INE-AL00 Build/HUAWEIINE-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E; InfoPath.3; Tablet PC 2.0)",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A366 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A366 MicroMessenger/6.7.1 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 8.1; BLA-TL00 Build/HUAWEIBLA-TL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 MQQBrowser/8.9.0 Mobile/15E148 Safari/604.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
                "Mozilla/5.0 (Linux; U; Android 7.1.1; zh-CN; OPPO A79k Build/N6F26Q) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.8.998 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.1.1; OS105 Build/NGI77B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.84 Mobile Safari/537.36",
                "Mozilla/5.0 (Linux; Android 6.0.1; KIW-AL10 Build/HONORKIW-AL10; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.6.7.1321(0x26060736) NetType/WIFI Language/zh_CN",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3534.4 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36",
                "Mozilla/5.0 (Linux; Android 7.0; SM-G9200 Build/NRD90M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.0; MHA-AL00 Build/HUAWEIMHA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/WIFI Language/zh_CN Process/tools",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 9_2_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Mobile/13D15 MicroMessenger/6.7.1 NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Linux; Android 8.1.0; PAFM00 Build/OPM1.171019.026; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/11.0 baiduboxapp/11.0.0.11 (Baidu; P1 8.1.0)",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/601.4.4 (KHTML, like Gecko) Version/9.0.3 Safari/601.4.4",
                "Mozilla/5.0 (Linux; Android 8.0; PRA-AL00 Build/HONORPRA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_5) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.1.2 Safari/603.3.8",
                "Mozilla/5.0 (Linux; Android 7.1.2; vivo X9 Build/N2G47H; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.6.7.1321(0x26060739) NetType/4G Language/zh_CN",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.104 Safari/537.36 Core/1.53.5538.400 QQBrowser/9.7.13332.400",
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0) Core/1.63.6756.400 QQBrowser/10.3.2473.400",
                "Mozilla/5.0 (Linux; Android 8.1; BLA-TL00 Build/HUAWEIBLA-TL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools",
                "Mozilla/5.0 (Linux; Android 8.1; PBEM00 Build/OPM1.171019.026; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044306 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070336) NetType/4G Language/zh_CN Process/tools"
       };


        #endregion
    }


    public class GZipWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest webrequest = (HttpWebRequest)base.GetWebRequest(address);
            webrequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webrequest.KeepAlive = true;
            webrequest.Timeout = 10000;
            //webrequest.ServicePoint.Expect100Continue = false;
            return webrequest;
        }
    }
}
