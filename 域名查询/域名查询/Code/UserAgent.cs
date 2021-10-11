using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
namespace 域名查询.Code
{
   public class UserAgent
    {
       
       static Regex reg = new Regex(@"\d{3,}", RegexOptions.Compiled);
       /// <summary>
       /// 把三位数字随机一下
       /// </summary>
       /// <param name="m"></param>
       /// <returns></returns>
       private static string matchEval(Match m)
       {
           if (m.Success)
           {
               int v = Int32.Parse( m.Value);
               return (new Random(Guid.NewGuid().GetHashCode()).Next(v / 2, v + 1)).ToString();
                
           }
           return null;
       }

       public static string Get(bool random = false)
       {
           var r = useragent[new Random(Guid.NewGuid().GetHashCode()).Next(useragent.Length)];
           if (random)
           r = reg.Replace(r, new MatchEvaluator(matchEval));
           return r;
       }
       private static string[] useragent = new string[]{
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; CIBA; staticlogin:product=cbpro09&act=login&info=ZmlsZW5hbWU9UG93ZXJ3b3JkMjAwOVByby4yNTI2OS40MDExLmV4ZSZtYWM9N0FENkVDRTZFQUY1NEQ1QkE3QkQyNkE2NUQyOTU2RUImcGFzc3BvcnQ9JnZlcnNpb249MjAwOS4wOC4yMi4yLjI4NCZjcmFzaHR5cGU9MQ==&verify=ea99a7e73ee18305ebf639d7472af6b8)",
        "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; .NET CLR 2.0.50727)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 717)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; 360SE QQDownload 322)",
        "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19 QIHU 360EE",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; qdesk 2.4.1262.203)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 715)",  
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; AskTbNRO/5.12.5.17640; 2345Explorer)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; 360SE; 360SE; QQDownload 715)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 677; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 708; 360SE; 360SE)",
        "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; QQDownload 715)",
        "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; 360SE  QQDownload 715)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; QQDownload 708; qdesk 2.3.1186.202; SV1; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 708; KB974489)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 661; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; InfoPath.3; 360SE; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; GTB7.2; KB974489; InfoPath.2; SE 2.X MetaSr 1.0)", 
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; QQDownload 717; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; CNCDialer; KB974489; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; KB974488; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6.5; .NET CLR 2.0.50727)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; Zune 4.7; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; SV1; QQDownload 715; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; QQDownload 708)",
        "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.802.30 Safari/535.1 SE 2.X MetaSr 1.0",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB7.2.3230)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 708; KB974489; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729)",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; qdesk 2.3.1186.112)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; qdesk 2.3.1186.202)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; GTB6.6.3120; SV1)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1 qdesk 2.2.1200.200)",  
        "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100  Safari/534.30 TaoBrowser/1.2",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 63263)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 71732)",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)", 
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 69223; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 708323)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; QQDownload 708; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET4.0C; .NET4.0E; 4399Box.1357; 4399Box.1183; 4399Box.1357)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; AskTB5.6)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; AskTbSPC2/5.13.1.18107)",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; QQDownload 71807)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; GTB7.2.32003)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727) QQBrowser/6.14.15138.201",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET4.0C; .NET4.0E; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) QQBrowser/6.14.14517.201",
        "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.79 Safari/535.11 QIHU THEWORLD",
        "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19",
        "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.100  Safari/534.30 TaoBrowser/1.2",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) QQBrowser/6.13.13719.201",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)",
        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; QQDownload 708; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) )",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; InfoPath.2; SE 2.X MetaSr 1.0)",  
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; 360SE; 360SE)", 
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; QQDownload 708; .NET CLR 2.0.50727)", 
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; doyo 2.0.30458)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB7.1; .NET CLR 2.0.50727; 360SE)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; KB974488)",
        "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; GTB6.6; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)"
       };
       }
}
