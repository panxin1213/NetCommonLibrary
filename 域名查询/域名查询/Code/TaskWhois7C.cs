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
    public class TaskWhois7C
    {
        private static Regex reg = new Regex(@"whoistdline""><span class=""predd"">(?<count>[\d-]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public TaskWhois7C(string domain, Action<string> ondata)
        {
            using (var x = new WebClient()) {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://whois.7c.com/hander/WhoIsServices.ashx"));

                String postdata = String.Format("domain={0}&strdo=dm", domain);
                request.Accept = "*/*";
                request.Referer = "http://whois.7c.com/";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.UserAgent = UserAgent.Get();
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postdata.Length;
                request.Method = "POST";
                request.Timeout = 5000;
                using(Stream requestStream = request.GetRequestStream()){
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
                string count = "-";
                if (html.IndexOf("不存在此域名whois信息") > -1)
                {
                    count = "可注";
                }
                else
                {
                    var m = reg.Match(html);
                    if (m.Success)
                        count = m.Groups["count"].Value.Replace(",", "");
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
