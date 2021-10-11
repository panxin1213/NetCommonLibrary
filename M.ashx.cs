using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using ChinaBM.Common;
using Common.Library.Log;
using System.Collections.Specialized;
using System.Drawing;
using ImageResizer;

namespace ChinaBM.IMG.ChinaYigui.Res
{
    public class M : IHttpHandler
    {
        private static readonly Regex PathRegex = GetRegex();
        private Regex NewContentImgSrcRegex = new Regex(@"/web6/(?<picmm>.*)/(?<picurl>.*).jpg", RegexOptions.Compiled);
        private const string siteName = ".chinayigui.com";
        public static Regex GetRegex()
        {
            //u/#width#_#height#_#mode##path#
            var x = BMConfig.Current.Path.ImageThumb;

            return new Regex(x.Replace("#width#", @"(?<width>\d+)")
              .Replace("#height#", @"(?<height>\d+)")
              .Replace("#mode#", @"(?<mode>\w+)")
              .Replace("#path#", @"(?<path>.+$)"), RegexOptions.Compiled | RegexOptions.Singleline);
        }
        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            var Response = context.Response;
            var Server = context.Server;
            var match = PathRegex.Match(Request.RawUrl);
            if (match == null || !match.Success)
            {
                try
                {
                    Response.ContentType = "image/jpeg";
                    Response.TransmitFile(Request.RawUrl);
                    return;
                }
                catch
                {
                    Response.Write("err");
                    return;
                }
            }

            Stopwatch timer = new Stopwatch();
            timer.Start();
            int width = Convert.ToInt32(match.Groups["width"].Value);
            int height = Convert.ToInt32(match.Groups["height"].Value);

            if (width > 2048)
            {
                width = 2048;
            }

            if (height > 2048)
            {
                height = 2048;
            }

            string m = match.Groups["mode"].Value;

            switch (m)
            {
                case "c":
                    m = "crop";
                    break;
                case "w":
                    {
                        height = 0;
                        m = "";
                    }
                    break;
                case "h":
                    {
                        width = 0;
                        m = "";
                    }
                    break;
                case "o":
                    {
                        m = "max";
                    }
                    break;
                default:
                    {
                        m = "";
                    }
                    break;
            }


            var externalurl = match.Groups["path"].Value;

            var ql = new List<string>();

            if (externalurl.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
            {
                externalurl = externalurl.Substring(0, externalurl.Length - 5);
                ql.Add("format=webp");
            }

            if (externalurl.StartsWith("/most/", StringComparison.OrdinalIgnoreCase))
            {
                externalurl = "http://www.chinamost.net/" + externalurl.ToLower().Replace("/most/", "");
            }

            if (!externalurl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && externalurl.StartsWith("/web6/", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    //通过正则，添加密钥（密钥格式：(图片URL+序列号).md5），验证web6图片
                    var match2 = NewContentImgSrcRegex.Match(externalurl);
                    //var base64path = EncryptKit.DecodeBase64(match2.Groups["picurl"].Value.Replace("$EFG$", "/"));
                    var picmmStr = EncryptKit.ToUpperMd5(match2.Groups["picurl"].Value + ConfigurationManager.AppSettings["ImageApiSafeKey"]);
                    //判断是否为非法链接
                    if (!match2.Groups["picmm"].Value.Equals(picmmStr))
                    {
                        throw new Exception("非法链接");
                        return;
                    }
                    //var base64path = EncryptKit.DecodeBase64(originalImagePath.Replace("$EFG$", "/").Replace(".jpg", "").Replace("/web6/", "").Replace(picmmStr,""));
                    var base64path = EncryptKit.DecodeBase64(match2.Groups["picurl"].Value.Replace("$EFG$", "/"));
                    //var base64path = "www.chinachugui.com/images/1.jpg";
                    if (base64path.IndexOf("/") != -1)
                    {
                        externalurl = "http://" + base64path;
                    }
                    else
                    {
                        throw new Exception("错误链接:" + base64path);
                    }

                }
                catch (Exception e)
                {
                    Logger.Error(this, e.Message, e);
                    Response.Write("err:" + e.Message + "::::" + e.StackTrace);
                    return;
                }
            }

            if (externalurl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var extthum = externalurl.Replace("http://", "");
                    extthum = "/external" + extthum.Substring(extthum.IndexOf("/"));
                    var mapextthum = HttpKit.GetMapPathVirtual(extthum);
                    if (!File.Exists(mapextthum))
                    {
                        DirectoryEx.CreateFolder(mapextthum);
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(externalurl, mapextthum);
                        }
                    }
                    externalurl = extthum;
                }
                catch (Exception e)
                {
                    Logger.Error(this, e.Message, e);
                    Response.Write("err:" + e.Message + "::::" + e.StackTrace);
                    return;
                }
            }


            if (!File.Exists(HttpKit.GetMapPathVirtual(externalurl)))
            {
                Response.StatusCode = 404;
                return;
            }

            string eTag = Request.Headers["If-None-Match"];
            string last = Request.Headers["If-Modified-Since"];

            var ofile = new FileInfo(HttpKit.GetMapPathVirtual(externalurl));

            //if (ofile.Extension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
            //{
            //    var fileEtag = ("\"" + ofile.LastWriteTime.Ticks.ToString() + "\"");
            //    var filelast = String.Format("{0:R}", ofile.LastWriteTime.ToUniversalTime());


            //    if (fileEtag.Equals(eTag) || filelast.Equals(last))
            //    {
            //        Response.Status = "304 Not Modified";
            //    }
            //    else
            //    {
            //        Response.AddHeader("ETag", fileEtag);
            //        Response.AddHeader("Last-Modified", filelast);
            //        Response.ContentType = "image/gif";
            //        Response.TransmitFile(externalurl);
            //    }
            //}

            var ismark = false;

            try
            {
                var extmarkpath = ConfigurationManager.AppSettings["ExtMarkPath"].ToSafeString().Split('|').Where(a => !String.IsNullOrEmpty(a)).ToArray();
                var inmarkpath = ConfigurationManager.AppSettings["InMarkPath"].ToSafeString().Split('|').Where(a => !String.IsNullOrEmpty(a)).ToArray();

                ismark = externalurl.IndexOf("/nomark/") == -1 && ofile.LastWriteTime > new DateTime(2015, 4, 1) &&
                    (extmarkpath.Length == 0
                    || String.IsNullOrEmpty(Request.UrlReferrer.ToSafeString())
                    || !extmarkpath.Any(a => Request.UrlReferrer.AbsolutePath.Trim('/').Equals(a.Trim('/'), StringComparison.OrdinalIgnoreCase))
                    || inmarkpath.Any(a => Request.UrlReferrer.AbsolutePath.Trim('/').Equals(a.Trim('/'), StringComparison.OrdinalIgnoreCase)));
            }
            catch (Exception)
            {
                ismark = false;
            }

            var orpath = BMConfig.Current.Url.ImageServers[0] + externalurl;


            try
            {
                var orimgUri = new Uri(orpath);
                var extension = orimgUri.AbsolutePath.Split('.');
                String thumbnailPath = Server.MapPath(String.Format(@"/Cache/ImageThumb/{0}.thumb_{1}_{2}_{3}{5}.{4}", orimgUri.AbsolutePath.Trim('/'), m.ToString(), width, height, ql.Contains("format=webp") ? "webp" : extension[extension.Length - 1], ismark ? "_mark" : "")); //缩略图地址


                //string eTag = "\"" + tfile.LastWriteTime.Ticks.ToString() + "\"";
                //                string last = String.Format("{0:R}", tfile.LastWriteTime.ToUniversalTime());


                //Response.RedirectPermanent(orpath);
                FileInfo tfile = null;
                if (File.Exists(thumbnailPath))
                {
                    tfile = new FileInfo(thumbnailPath);
                }
                if (tfile == null || ConfigurationManager.AppSettings["CacheUpdateTime"].ToDateTime(new DateTime(1900, 1, 1)) > tfile.LastWriteTime)
                {
                    if (width == 0 && height == 0 || ismark)
                    {
                        using (var orimage = Image.FromFile(HttpKit.GetMapPathVirtual(externalurl)))
                        {


                            if (ismark)
                            {
                                var rwidth = width == 0 ? orimage.Width : width;
                                var rheight = height == 0 ? (orimage.Height * rwidth / orimage.Width) : height;

                                ismark = (rwidth >= ConfigurationManager.AppSettings["MinAddMarkWidth"].ToInt(300)) && (rheight >= ConfigurationManager.AppSettings["MinAddMarkWidth"].ToInt(300));
                            }

                            if (width == 0 && height == 0)
                            {
                                if (orimage.Width > 3000 || orimage.Height > 3000)
                                {
                                    Response.ContentType = GetContentType(ofile.Extension.Replace(".", ""));
                                    Response.TransmitFile(externalurl);
                                    return;
                                }
                                width = orimage.Width;
                                height = orimage.Height;
                            }
                        }
                    }

                    if (width > 0)
                    {
                        ql.Add("width=" + width);
                    }
                    if (height > 0)
                    {
                        ql.Add("height=" + height);
                    }
                    if (!String.IsNullOrEmpty(m) && (width > 0 || height > 0))
                    {
                        ql.Add("mode=" + m);
                    }

                    if (ismark)
                    {
                        ql.Add("watermark=mark01");
                    }

                    var c = new ImageResizer.Configuration.Config(new ResizerSection(
                    @"<resizer>
	                    <plugins>
		                    <add name=""MvcRoutingShim"" />
		                    <add name=""PrettyGifs"" />
		                    <add name=""WebPEncoder"" downloadnativedependencies=""true""/>
		                    <add name=""WebPDecoder"" downloadnativedependencies=""true""/>
		                    <add name=""Watermark"" />
                            <add name=""AnimatedGifs"" />
	                    </plugins>
	                    <watermarks>
		                    <otherimages path=""~/watermarks"" right=""20"" bottom=""20"" />
		                    <image name=""mark01"" path=""~/watermarks/mark.png"" bottom=""20"" right=""20"" />
	                    </watermarks>
                    </resizer>
                        "));
                    c.Plugins.LoadPlugins();
                    string s = c.GetDiagnosticsPage();


                    c.BuildImage(HttpKit.GetMapPathVirtual(externalurl), thumbnailPath, string.Join("&", ql));

                    tfile = new FileInfo(thumbnailPath);
                }

                if (tfile != null && tfile.Exists)
                {
                    var fileEtag = ("\"" + tfile.LastWriteTime.Ticks.ToString() + "\"");
                    var filelast = String.Format("{0:R}", tfile.LastWriteTime.ToUniversalTime());

                    if (fileEtag.Equals(eTag) || filelast.Equals(last))
                    {
                        Response.Status = "304 Not Modified";
                    }
                    else
                    {
                        Response.CacheControl = "public";
                        Response.Expires = 1296000;
                        Response.AddHeader("ETag", fileEtag);
                        Response.AddHeader("Last-Modified", filelast);
                        Response.ContentType = GetContentType(ql.Contains("format=webp") ? "webp" : extension[extension.Length - 1]);
                        Response.TransmitFile(thumbnailPath);
                    }
                }
                else
                {
                    Response.StatusCode = 404;
                    Response.End();
                }

            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
                Response.Write("err:" + e.Message + "::::" + e.StackTrace);
            }

            timer.Stop();
            Response.AddHeader("e", timer.Elapsed.TotalMilliseconds.ToString("0.0000"));

        }




        private string GetContentType(string extension)
        {
            if ("png".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/png"; //Changed from image/x-png to image/png on May 14, 2011, per http://www.w3.org/Graphics/PNG/
            else if ("jpg".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/jpeg";
            else if ("jpeg".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/jpeg";
            else if ("gif".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/gif";
            else if ("bmp".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/x-ms-bmp";
            else if ("tiff".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/tiff";
            else if ("webp".Equals(extension, StringComparison.OrdinalIgnoreCase))
                return "image/webp";
            else
            {
                throw new ArgumentOutOfRangeException("Unsupported format " + extension);
            }
        }


        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}