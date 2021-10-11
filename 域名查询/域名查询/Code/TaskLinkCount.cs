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
    public class TaskLinkCount 
    {
        //private static string[] baidu_domain = { "61.135.169.125", "115.239.210.27", "119.75.218.70", "119.75.217.56", "220.181.111.148", "119.75.218.77", "115.239.210.26", "220.181.111.147", "220.181.111.149", "119.75.217.109", "61.135.169.105", "119.75.217.109", "220.181.112.143" };
        private static string[] baidu_domain = { "www.baidu.com","www1.baidu.com", "www5.baidu.com", "www7.baidu.com"}; //www6,www12限制比较多 www1不稳定
        /// <summary>
        /// 判断结果中多少不同的顶级域名数量
        /// <span class="g" 开头为正常 <para class="g" ms为百度相关域名，排除掉
        /// </summary>
        private static Regex reg_basedomain = new Regex(@"<span\s+class=""c-showurl"">\s*(?<domain>(?:\w|\.|</?b>)+)(?!<)/", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex reg = new Regex(@">百度为您找到相关结果约?(?<count>[\d,]*)个", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static int index = 0;
        private static int domain_length = baidu_domain.Length;
        private static Regex reg_cookie = new Regex(@"(?:Domain|Path|Expires|max-age|version)\s*=\s*[^;=]*([,;]|$)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="ondata">a = 外链，b=域名数，c=首页第一</param>
        public TaskLinkCount(string domain, Action<string,string,string> ondata)
        {
            
           
                int i = 0;
                lock (reg)
                {
                    i=index++;
                    if (index >= domain_length)
                    {
                        index = 0;
                    }
                }
                string bd_domain = baidu_domain[i];
                string domain_urlencode = Public.UrlEncode(domain);
                string user_agent = UserAgent.Get();
                string bd_url = String.Format(@"http://{1}/s?wd=%22{0}%22&rn=50", domain_urlencode, bd_domain);

                StringBuilder sb_cookie = new StringBuilder();

                //var request = (HttpWebRequest)WebRequest.Create(new Uri(String.Format(@"http://{0}/recommend/b.gif", bd_domain)));

                ////request.Headers.Add("Cookie", sb_cookie.ToString());
                //request.AllowAutoRedirect = true;
                ////request.CookieContainer = new CookieContainer();
                //request.Accept = "*/*";
                //request.Referer = bd_url;
                //request.Headers.Add("Accept-Language", "zh-CN");
                //request.Headers.Add("Accept-Encoding", "gzip, deflate");
                //request.UserAgent = user_agent;
                //request.KeepAlive = true;
                //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                //request.Timeout = 5000;
                //StringBuilder sb_cookie = new StringBuilder();
                ////CookieContainer cookie = null ;
                //try
                //{
                //    using (var response = (HttpWebResponse)request.GetResponse())
                //    {
                //        sb_cookie.Append(reg_cookie.Replace(response.Headers.Get("Set-Cookie") ?? "", ""));

                //    }
                //    //cookie = request.CookieContainer;
                //}
                //catch (Exception e)
                //{

                //}

                //var request = (HttpWebRequest)WebRequest.Create(new Uri(String.Format(@"http://c.baidu.com/c.gif?t=0&q={0}&p=0&pn=1", domain_urlencode)));

                //request.Headers.Add("Cookie", sb_cookie.ToString());
                //request.AllowAutoRedirect = true;
                ////request.CookieContainer = new CookieContainer();
                //request.Accept = "*/*";
                //request.Referer = bd_url;
                //request.Headers.Add("Accept-Language", "zh-CN");
                //request.Headers.Add("Accept-Encoding", "gzip, deflate");
                //request.UserAgent = user_agent;
                //request.KeepAlive = true;
                //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                //request.Timeout = 5000;

                ////CookieContainer cookie = null ;
                //try
                //{
                //    using (var response = (HttpWebResponse)request.GetResponse())
                //    {
                //        sb_cookie.Append(reg_cookie.Replace(response.Headers.Get("Set-Cookie") ?? "", ""));

                //    }
                //    //cookie = request.CookieContainer;
                //}
                //catch (Exception e)
                //{

                //}
                //request.Abort();

                var request = (HttpWebRequest)WebRequest.Create(new Uri(bd_url));

                request.CookieContainer = new CookieContainer();
                request.Headers.Add("Cookie", sb_cookie.ToString());
                request.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, */*";
                request.Referer = String.Format("http://{0}/",bd_domain);
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.UserAgent = user_agent;
                //request.KeepAlive = true;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                //结果数量
                string count = "-";
                //域名数量
                string count2 = "-";
                //查询域名是否第一
                string isFirst = "";
                try
                {
                    string html = "";
                    using (var response = request.GetResponse())
                    {
                        //sb_cookie.Append(reg_cookie.Replace(response.Headers.Get("Set-Cookie") ?? "", ""));
                        
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                            {
                                html = sr.ReadToEnd();
                                sr.Close();
                            }
                            stream.Close();
                        }
                        response.Close();
                    }
                    request.Abort();
                    if (html.IndexOf("noresult.html") > -1) //没有记录
                    {
                        count = "0";
                    }
                    else
                    {
                        var m = reg.Match(html);

                        if (m.Success)
                            count = m.Groups["count"].Value.Replace(",", "");
                        else
                            count = "0";

                        var md = reg_basedomain.Matches(html);
                            
                        //用来统计域名数量的字典
                        var domain_dic = new Dictionary<string, int>();
                           
                        if (md.Count > 0)
                        {
                            ///判断首页第一，在结果大于1时才肯定
                            if (md.Count>1 && GetDomain(md[0].Groups["domain"].Value).Trim().EndsWith("www."+domain, StringComparison.OrdinalIgnoreCase))
                                isFirst = "T";
                            foreach (Match mt in md)
                            {
                                string dm = GetDomain(mt.Groups["domain"].Value);
                                if (domain_dic.ContainsKey(dm))
                                {
                                    domain_dic[dm] += 1;
                                }
                                else
                                {
                                    domain_dic[dm] = 1;
                                }
                            }
                        }
                        count2 = domain_dic.Count.ToString();
                    }
                       
                   





                }
                catch (Exception e)
                {
                    count = String.Format("err:{0},{1}", e.Message, bd_domain);
                }
                try
                {


                    ondata(count, count2, isFirst);
                    

                }
                catch (Exception) { }
                
                //Thread.Sleep(10);
          
        }
        static Regex html_tag = new Regex(@"</?[^>]>|<|>|/", RegexOptions.Compiled| RegexOptions.Singleline);
        /// <summary>
        /// 过滤域名中html标签 如www.<b>51.la</b>
        /// </summary>
        /// <param name="domain"></param>
        public static string GetDomain(string domain)
        {
            return html_tag.Replace(domain??"","");
        }

    }
}
