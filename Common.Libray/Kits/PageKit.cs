namespace ChinaBM.Common
{
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web.UI;
    using System;

    public static class PageKit
    {
        #region CreateHtml 生成静态html

        public static bool CreateHtml(string path, string outPath, out string htmls)
        {

            return CreateHtml(path, outPath, out htmls, Encoding.Default);
        }

        /// <summary>
        ///  生成静态html
        /// </summary>
        /// <param name="path">动态页地址</param>
        /// <param name="outPath">静态页存放地址</param>
        public static bool CreateHtml(string path, string outPath, out string htmls, Encoding ecode)
        {
            bool result = false;
            htmls = string.Empty;
            try
            {
                Page page = new Page();
                string html = GetHtmlByUrl("http://" + HttpKit.CurrentFullHost + path, ecode);
                FileStream fileStream;
                if (File.Exists(HttpKit.GetMapPath(outPath)))
                {
                    File.Delete(HttpKit.GetMapPath(outPath));
                    fileStream = File.Create(HttpKit.GetMapPath(outPath));
                }
                else
                {
                    if (!File.Exists(HttpKit.GetMapPath(outPath.Substring(0, outPath.LastIndexOf("/")))))
                    {
                        Directory.CreateDirectory(HttpKit.GetMapPath(outPath.Substring(0, outPath.LastIndexOf("/"))));
                    }
                    fileStream = File.Create(HttpKit.GetMapPath(outPath));
                }
                if (!String.IsNullOrEmpty(html))
                {
                    byte[] bytes = ecode.GetBytes(html.ToString());
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }
                result = true;
                htmls = html;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        #endregion
        #region GetHtmlByUrl 根据Url获取Html
        /// <summary>
        /// 根据Url获取Html
        /// </summary>
        /// <param name="url">合法的Url地址</param>
        /// <returns></returns>
        public static string GetHtmlByUrl(string url)
        {
            return GetHtmlByUrl(url, Encoding.Default);
        }

        public static string GetHtmlByUrl(string url, Encoding encode)
        {
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Timeout = 20000;
                webRequest.Method = "Get";//20秒超时
                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                using (Stream stream = webResponse.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(stream, encode))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                if (e.Message == "远程服务器返回错误: (404) 未找到。")
                {
                    return string.Empty;
                }
                if (e.Message == "远程服务器返回错误: (500) 内部服务器错误。")
                {
                    return string.Empty;
                }
                if (e.Message.IndexOf("超时") > -1)
                {
                    return string.Empty;
                }
                return string.Empty;
            }
        }

        public static string GetHtmlByUrl(string url, Encoding encode, string param)
        {
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Timeout = 20000;
                webRequest.Method = "Post";//20秒超时
                byte[] buf = System.Text.Encoding.GetEncoding("utf-8").GetBytes(param);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = buf.Length;

                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                using (Stream stream = webResponse.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(stream, encode))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                return e.Message.ToString();
                if (e.Message == "远程服务器返回错误: (404) 未找到。")
                {
                    return string.Empty;
                }
                if (e.Message == "远程服务器返回错误: (500) 内部服务器错误。")
                {
                    return string.Empty;
                }
                if (e.Message.IndexOf("超时") > -1)
                {
                    return string.Empty;
                }
                return string.Empty;
            }
        }

        #endregion
        #region  SaveFile

        public static bool SaveFile(string filepath, string url)
        {
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Timeout = 20000;
                webRequest.Method = "Get";//20秒超时
                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                using (Stream stream = webResponse.GetResponseStream())
                {
                    string file = filepath.Substring(0, filepath.LastIndexOf("\\"));
                    if (!File.Exists(file))
                    {
                        Directory.CreateDirectory(file);
                    }
                    BinaryReader binreader = new BinaryReader(stream);
                    byte[] bytes = binreader.ReadBytes((int)webResponse.ContentLength);
                    FileStream fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public static Stream GetInternetFile(string url)
        {
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Timeout = 20000;
                webRequest.Method = "Get";//20秒超时
                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                return webResponse.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }

        #endregion

    }
}
