using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
namespace 域名查询.Code
{
    /// <summary>
    /// 百度外链
    /// </summary>
    public class TaskSogouWeight
    {
        private static Regex reg = new Regex(@"sogourank\s*=\s*(\d+)", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public TaskSogouWeight(string domain, Action<string> ondata)
        {
            using (var x = new WebClient())
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(string.Format("http://rank.ie.sogou.com/sogourank.php?ur=http://www.{0}/", domain)));

                //String postdata = String.Format("domain=http%3A%2F%2F{0}&strdo=smtl&strname=tl63",domain);
                //request.Accept = "*/*";
                //request.Referer = "http://link.7c.com/link/www.7c.com/";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.UserAgent = UserAgent.Get();
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
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
                        using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                        {
                            html = sr.ReadToEnd();
                        }
                    }


                    var m = reg.Match(html);

                    if (m.Success)
                        count = m.Groups[1].Value;
                    else
                        count = "err:no_match";
                }
                catch (Exception e)
                {
                    count = String.Format("err:{0}", e.Message);
                }
                try
                {
                    ondata(count);
                }
                catch (Exception) { }
            }

        }


    }
}
