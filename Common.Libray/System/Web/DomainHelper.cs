using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Web
{

    /// <summary>
    /// 解析后的域名部分
    /// </summary>
    public struct DomainPart 
    {
        /// <summary>
        /// 得到首页地址
        /// </summary>
        /// <returns></returns>
        public string GetHomeLink() {
            return Suffix+SubDomain+"."+DomainWithPort+"/";
        }
        /// <summary>
        /// 子域名 a.b.c.xxxx.com.cn 的 a.b.c
        /// </summary>
        public string SubDomain { get; internal set; }
        /// <summary>
        /// 域名a.b.c.xxx.com.cn 的 xxx.com.cn
        /// </summary>
        public string BaseDomain { get; internal set; }


        /// <summary>
        /// 整个域名后缀 .com.cn
        /// </summary>
        public string Suffix { get; internal set; }

        /// <summary>
        /// 基本域名加端口
        /// </summary>
        public string DomainWithPort { get; internal set; }

        /// <summary>
        /// xxx.com.cn的xxx
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// 域名类型 com,net ...
        /// </summary>
        public string Type { get; internal set; }
        /// <summary>
        /// 域名国家 cn,us,uk,ru,jp .....
        /// </summary>
        public string Country {get;internal set;}
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; internal set; }
    }
    /// <summary>
    /// 域名分析
    /// </summary>
    public static class DomainHelper
    {

        /// <summary>
        /// 域名解析正则
        /// </summary>
        private static Regex _regDomainParser = new Regex(@"^(?:http://)?(?<subdomain>[\w\.\-]+)*\.*(?<domain>[\w\-]+)\.(?<com>[\w]{2,3})?\.?(?<countryextension>[\w]{2})?\:?(?<Port>\d*)(?:/.*)?$", RegexOptions.RightToLeft | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 得到当前域名的DomainPart
        /// </summary>
        /// <returns></returns>
        public static DomainPart CurrentDomainPart()
        { 
            return ParseDomain(HttpContext.Current.Request.Headers["host"]);
        }
        /// <summary>
        /// 是否当前域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsLocalDoamin(string url)
        {
            return ParseDomain(url).BaseDomain.Equals(CurrentDomainPart().BaseDomain, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary> 
        /// Input: test.domain.com.au  Output: domain.com.au 
        /// Input: xyz.domain.com        Output: domain.com 
        /// Input: xyz.domain.co.uk      Output: domain.co.uk 
        /// Input: www.domain.com        Output: domain.com 
        /// In short, strips out all the subdomain part and returns the rest 
        /// of URL. 
        /// </summary> 
        /// <param name="refUrl">Url which is identical to Request.Url.Host</param> 
        /// <returns>Stripped out Url.</returns> 
        public static DomainPart ParseDomain(string refUrl)
        {
            DomainPart result = new DomainPart();
            refUrl = refUrl ?? "";
            Match match1 =  _regDomainParser.Match(refUrl);
            if (match1.Success)
            {
                result.SubDomain = match1.Groups["subdomain"].Value;
                result.Name = match1.Groups["domain"].Value;
                result.Type = match1.Groups["com"].Value;
                result.Country = match1.Groups["countryextension"].Value;
                result.Port = match1.Groups["Port"].Value;
                if (String.IsNullOrEmpty(result.Type))
                {
                    result.Type = result.Country;
                    result.Suffix = result.Country;
                }
                else
                {
                    if (!String.IsNullOrEmpty(result.Country))
                    {
                        result.Suffix = result.Type + "." + result.Country;
                    }
                    else
                    {
                        result.Suffix = result.Type;
                    }
                }

                result.DomainWithPort = result.BaseDomain = result.Name + "." + result.Suffix;

                
                if (!String.IsNullOrEmpty(result.Port))
                {
                    result.DomainWithPort += ":" + result.Port;
                }

            }
            else {
                result.DomainWithPort = result.BaseDomain = refUrl;
            }

            return result;
        }
    }
}
