using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Common.Library.SiteMap
{
    public class URL
    {
        public string Loc { get; set; }

        public DateTime? Lastmod { get; set; }

        public string ChangeFreq { get; set; }

        public string Priority { get; set; }

        public bool IsMobile { get; set; }

        /// <summary>
        /// 自定义分组ID
        /// </summary>
        public object Additional { get; set; }

        /// <summary>
        /// 返回XElement对象，当loc或lastmod不存在时返回null
        /// </summary>
        /// <returns></returns>
        public XElement GetUrlXElement()
        {
            var node = new XElement("url");

            if (String.IsNullOrEmpty(Loc) || Lastmod == null)
            {
                return null;
            }

            node.Add(new XElement("loc") { Value = Loc });


            if (IsMobile)
            {
                var mn = new XElement("mobile");
                mn.SetAttributeValue("type", "mobile");
                node.Add(mn);
            }

            node.Add(new XElement("lastmod") { Value = Lastmod.Value.ToString("yyyy-MM-dd") });//THH:mm:ss+08:00

            if (!String.IsNullOrEmpty(ChangeFreq))
            {
                node.Add(new XElement("changefreq") { Value = ChangeFreq });
            }

            if (Priority.ToInt() > 1)
            {
                Priority = "1.0";
            }

            if (Priority.ToInt() < 0)
            {
                Priority = "0.6";
            }

            node.Add(new XElement("priority") { Value = Priority.ToDouble().ToString("0.0") });


            return node;
        }

        /// <summary>
        /// 返回url对象字符串
        /// </summary>
        /// <returns></returns>
        public string GetUrlString()
        {
            return GetUrlXElement().ToSafeString();
        }
    }
}
