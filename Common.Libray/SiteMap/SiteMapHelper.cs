using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Common.Library.SiteMap
{
    /// <summary>
    /// SiteMap操作类
    /// </summary>
    public static class SiteMapHelper
    {

        /// <summary>
        /// 返回sitemap的XElement对象
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static XElement GetSiteMapXElement(IEnumerable<URL> urls)
        {
            if (urls == null || urls.Count() == 0)
            {
                return null;
            }

            var nodes = urls.Select(a => a.GetUrlXElement()).Where(a => a != null).ToArray();

            if (nodes.Length == 0)
            {
                return null;
            }

            var urlset = new XElement("urlset");
            urlset.Add(nodes);


            return urlset;
        }


        /// <summary>
        /// 返回sitemap字符串
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static string GetSiteMapString(IEnumerable<URL> urls)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine(GetSiteMapXElement(urls).ToSafeString());
            return sb.ToSafeString();
        }


        /// <summary>
        /// 保存sitemap
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool SaveSiteMap(IEnumerable<URL> urls, string filepath)
        {
            try
            {
                var document = GetSiteMapXElement(urls);

                if (document != null)
                {
                    System.IO.DirectoryEx.CreateFolder(filepath);
                    document.Save(filepath);

                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
