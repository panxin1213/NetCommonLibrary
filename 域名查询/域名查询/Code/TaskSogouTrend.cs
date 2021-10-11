using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace 域名查询.Code
{
    /// <summary>
    /// 百度外链
    /// </summary>
    public class TaskSogouTrend
    {
        private static int cookieCount = 0;
        private  CookieContainer _cookieContainer = null;
        private  string _useragent = string.Empty ;

        private static readonly IDictionary<string, CookieContainer> Cookies = new Dictionary<string, CookieContainer>();

        public void Init()
        {
            _useragent = UserAgent.Get();
            lock (Cookies)
            {
                if (!Cookies.ContainsKey(_useragent))
                {
                    _cookieContainer = new CookieContainer();
                    Cookies[_useragent] = _cookieContainer;
                }
                else
                {
                    _cookieContainer = Cookies[_useragent];
                }
            }
        }

        public string GetHTML(string domain,bool isretry=false)
        {
            using (var x = new WebClient())
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("http://index.sogou.com/sidx?type=0&query={0}&newstype=10{1}", domain,isretry?"&repp=1":"")));

                //String postdata = String.Format("domain=http%3A%2F%2F{0}&strdo=smtl&strname=tl63",domain);
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Referer = "http://index.sogou.com/";
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                request.UserAgent = _useragent;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                request.CookieContainer = _cookieContainer;

                //request.Headers.Add("cookie", "CXID=552E9724FC0674178A158A87BD6D1F9F; SUID=D0AF0AAF2141900A55D2E64200076418; SUV=1441638819722499; sct=18; LSTMV=229%2C211; LCLKINT=1158; ad=ukllllllll2q1mBjlllllVBxDZYlllllrZ$s1lllll9llllljOxlw@@@@@@@@@@@; IPLOC=CN4301; wuid=AAGPMAP2DQAAAAqSMzE3HA8AZAM=; fromwww=1; ld=u60jAkllll2QqNeMlllllVBqT4clllllrZ6lOkllll9lllllpylll5@@@@@@@@@@; usid=6mlvijkSmQJ-dG_A; SNUID=246BEA9F44416186B521C6D244EE8F67; JSESSIONID=abcJuWhw7T8vH_x9t0yfv; SNUID=347BFD8F53517190FD4CDACD54C5A6DC");

                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = postdata.Length;
                //request.Method = "GET";
                request.Timeout = 10000;
                string count = "-";

                try
                {

                    var response = request.GetResponse();
                    string html = "";
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(936)))
                        {
                            html = sr.ReadToEnd();
                        }
                    }
                    return html;

                }
                catch (Exception e)
                {
                    throw;
                }
                return "err";
            }
        }
        /// <summary>
        /// 获取正常结果
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public string GetHTMLResult(string domain,int retrycount = 0)
        {
            string html = string.Empty;
            try
            {
                html = GetHTML(domain,retrycount>0);
            }
            catch (Exception e)
            {
                if (++retrycount > 2)
                {
                    throw;
                }
                Thread.Sleep(500);
                html = GetHTMLResult(domain, ++retrycount);
            }
            if (html.IndexOf("userindex", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return html;
            }
            if (html.IndexOf("document.cookie") > -1) //搜狗防刷
            {

                var mt = reg_cookie.Matches(html);

                foreach (Match m in mt)
                {
                    foreach (var c in m.Groups[1].Value.Split(';'))
                    {
                        var d = c.Split('=');

                        if (d.Length > 1)
                        {
                            _cookieContainer.SetCookies(new Uri("http://index.sogou.com"), d[0] + "=" + d[1]);
                            //cookieContainer.Add(new Cookie(d[0], d[1], "/", ".sogou.com"));
                        }

                    }
                }
                Thread.Sleep(500);
                html = GetHTML(domain, true);
                if (html.IndexOf("document.cookie") > -1) //搜狗防刷
                {
                    html = GetHTMLResult(domain, ++retrycount);
                }
            }
            return html;
        }

        private static Regex reg_cookie = new Regex(@"document.cookie\s*\=\s*""((?:\w+=\w+[;,])+)[^""]+""", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //private static Regex reg_cookie2 = new Regex(@"document.cookie\s*\=\s*""([^""]+)""", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex reg = new Regex(@"""userIndexes"":""((?:\d+?,?)+)""", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public TaskSogouTrend(string domain, Action<string> ondata)
        {
            Init();
            string count = "-";
            string html = string.Empty;
            try
            {
                html = GetHTMLResult(domain);
            }
            catch (Exception e)
            {
                count = String.Format("err:{0}", e.Message);
            }



            if (!string.IsNullOrEmpty(html) && html.IndexOf("没有找到") == -1)
            {
                var m = reg.Match(html);

                if (m.Success)
                {
                    count = m.Groups[1].Value;
                    int avg = (int) count.Split(',').Reverse().Take(365).Average(a => int.Parse(a));
                    count = avg.ToString();
                }
                else
                {
                    count = "err:no_match";
                }
            }
            

            try
            {
                ondata(count);
            }
            catch (Exception)
            {
            }
        }

        //using (var x = new WebClient())
            //{

            //    HttpWebRequest request =
            //        (HttpWebRequest)
            //            WebRequest.Create(
            //                new Uri(string.Format("http://index.sogou.com/")));

            //    //String postdata = String.Format("domain=http%3A%2F%2F{0}&strdo=smtl&strname=tl63",domain);
            //    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //    //request.Referer = "http://index.sogou.com/";
            //    request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            //    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            //    request.UserAgent = useragent;
            //    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;


            //    request.CookieContainer = cookieContainer;
            //    request.GetResponse();
            //}

            //using (var x = new WebClient()) {

            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("http://index.sogou.com/sidx?type=0&query={0}&newstype=10", domain)));

            //    //String postdata = String.Format("domain=http%3A%2F%2F{0}&strdo=smtl&strname=tl63",domain);
            //    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //    request.Referer = "http://index.sogou.com/";
            //    request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            //    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            //    request.UserAgent = useragent;
            //    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;


            //    request.CookieContainer = cookieContainer;
            //    //request.Headers.Add("cookie", "CXID=552E9724FC0674178A158A87BD6D1F9F; SUID=D0AF0AAF2141900A55D2E64200076418; SUV=1441638819722499; sct=18; LSTMV=229%2C211; LCLKINT=1158; ad=ukllllllll2q1mBjlllllVBxDZYlllllrZ$s1lllll9llllljOxlw@@@@@@@@@@@; IPLOC=CN4301; wuid=AAGPMAP2DQAAAAqSMzE3HA8AZAM=; fromwww=1; ld=u60jAkllll2QqNeMlllllVBqT4clllllrZ6lOkllll9lllllpylll5@@@@@@@@@@; usid=6mlvijkSmQJ-dG_A; SNUID=246BEA9F44416186B521C6D244EE8F67; JSESSIONID=abcJuWhw7T8vH_x9t0yfv; SNUID=347BFD8F53517190FD4CDACD54C5A6DC");
                
            //    //request.ContentType = "application/x-www-form-urlencoded";
            //    //request.ContentLength = postdata.Length;
            //    //request.Method = "GET";
            //    request.Timeout = 10000;
            //    string count = "-";
                
            //    try
            //    {
                    
            //        var response = request.GetResponse();
            //        string html = "";
            //        using (Stream stream = response.GetResponseStream())
            //        {
            //            using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(936)))
            //            {
            //                html = sr.ReadToEnd();
            //            }
            //        }

            //        if (html.IndexOf("没有找到")==-1)
            //        {
            //            var m = reg.Match(html);

            //            if (m.Success)
            //            {
            //                count = m.Groups[1].Value;
            //                int avg = (int) count.Split(',').Reverse().Take(365).Average(a => int.Parse(a));
            //                count = avg.ToString();
            //            }
            //            else
            //            {
            //                count = "err:no_match";
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        count = String.Format("err:{0}", e.Message);
            //    }
            //    try
            //    {
            //        ondata(count);
            //    }
            //    catch (Exception) { }
            //}
          
       


    }
}
