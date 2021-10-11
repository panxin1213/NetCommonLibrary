using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Web
{
    /// <summary>
    /// MVC 的Response扩展
    /// </summary>
    public static class ResponseEx
    {
        /// <summary>
        /// 输出一个文件名头（判断不同浏览器）
        /// </summary>
        /// <param name="response"></param>
        /// <param name="fileName"></param>
        public static void FileName(this HttpResponseBase response,String fileName)
        {
            var request = HttpContext.Current.Request;
            var encodeName = HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("+","%20");

            if (request.UserAgent.Contains("MSIE") || request.UserAgent.Contains("Safari") || request.UserAgent.Contains("Chrome"))
            {
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + encodeName + "\"");
            }
            else {
                response.AddHeader("Content-Disposition", "attachment;filename*=\"utf8''" + encodeName + "\"");
            }
        }
    }
}
