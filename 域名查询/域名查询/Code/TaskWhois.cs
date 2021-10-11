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
    public class TaskWhois
    {
        private static Regex reg = new Regex(@">过期时间：\s*(?<count>[\d-年月日]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public TaskWhois(string domain, Action<string> ondata)
        {
            using (var x = new WebClient()) {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(String.Format("http://panda.www.net.cn/cgi-bin/check.cgi?area_domain={0}", domain)));

                request.Referer = "http://www.net.cn/";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.UserAgent = UserAgent.Get();
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                
                request.Timeout = 30000;
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

                    if (html.IndexOf("Domain name is available") > -1)
                    {
                        count = "可注";
                    }
                    else
                    {
                        if (html.IndexOf("<returncode>200</returncode>") > -1)
                        {
                            count = "不可注";
                        }
                        else
                        {
                            count = "错误";
                        }
                        /*
                        var m = reg.Match(html);
                        if (m.Success)
                            count = m.Groups["count"].Value.Replace(",", "");
                        else
                            count = "err";
                         */
                    }
                    response.Close();
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
