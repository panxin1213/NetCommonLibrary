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
    public class TaskBR
    {
        private static Regex reg = new Regex(@"'weightfont_link'>(?<count>\d+)</div>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public TaskBR(string domain, Action<string> ondata)
        {
            using (var x = new WebClient()) {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://link.7c.com/hander/Friendslink.ashx"));

                String postdata = String.Format("domain=http%3A%2F%2F{0}&strdo=smtl&strname=tl63",domain);
                request.Accept = "*/*";
                request.Referer = "http://link.7c.com/link/www.7c.com/";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postdata.Length;
                request.Method = "POST";
                request.Timeout = 10000;
                string count = "-";
                try
                {
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        var by = Encoding.UTF8.GetBytes(postdata);
                        requestStream.Write(by, 0, by.Length);
                    }
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
                        count = m.Groups["count"].Value.Replace(",", "");
                    else
                        count = "err";
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
