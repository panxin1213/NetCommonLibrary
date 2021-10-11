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
    /// 备案查询
    /// </summary>
    public class TaskWhoisBeiAn
    {
        private static Regex reg = new Regex(@">过期时间：\s*(?<count>[\d-年月日]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public TaskWhoisBeiAn(string domain, Action<string> ondata)
        {
            using (var x = new WebClient())
            {

                string count = "-";
                try
                {
                    x.Encoding = Encoding.UTF8;
                    var s = x.DownloadString("http://www.beianbeian.com/search/" + domain);
                    if (s.IndexOf("没有符合条件的记录, 即未备案") > -1 || s.IndexOf("[反查]") == -1)
                    {
                        s = x.DownloadString(string.Format("http://icp.alexa.cn/index.php?q={0}&code=显示验证码&icp_host=jxcainfo", domain));

                        if (s.IndexOf("请输入验证码查询最新备案信息") > -1 || s.IndexOf("主办单位名称") == -1)
                        {
                            count = "否";
                        }
                        else
                        {
                            count = "是";
                        }
                    }
                    else
                    {
                        count = "是";
                    }
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
