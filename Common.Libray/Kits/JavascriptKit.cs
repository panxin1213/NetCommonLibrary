namespace ChinaBM.Common
{
    using System.Web;

    public static class JavascriptKit
    {
        public static void RegisterScript(string scriptContent)
        {
            HttpContext.Current.Response.Write("<script type=\"text/javascript\">" + scriptContent + "</script>");
        }

        public static void WindowRedirect(string url)
        {
            RegisterScript("window.parent.location.href='" + url + "';");
        }

        public static void CurrentRedirect(string url)
        {
            RegisterScript("window.location.href='" + url + "';");
        }

        #region JsonCharFilter Json特符字符过滤
        /// <summary>
        /// Json特符字符过滤
        /// </summary>
        /// <param name="targetString">要过滤的源字符串</param>
        /// <returns>返回过滤的字符串</returns>
        public static string JsonCharFilter(string targetString)
        {
            targetString = targetString.Replace("\\", "\\\\");
            targetString = targetString.Replace("\b", "\\\b");
            targetString = targetString.Replace("\t", "\\\t");
            targetString = targetString.Replace("\n", "\\\n");
            targetString = targetString.Replace("\n", "\\\n");
            targetString = targetString.Replace("\f", "\\\f");
            targetString = targetString.Replace("\r", "\\\r");
            return targetString.Replace("\"", "\\\"");
        }
        #endregion
       


    }
}
