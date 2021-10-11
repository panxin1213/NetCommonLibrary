using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChinaBM.Common;

namespace Common.Library.Kits
{
    public static class BaiduXML
    {
        public static void Generate(IEnumerable<BaiduXMLModel> l)
        {
            for (var i = 0; i < (l.Count() % 100 > 0 ? (l.Count() / 100) + 1 : l.Count() / 100); i++)
            {
                var urlset = new XElement("urlset");
                urlset.Add(l.Where((a, b) => b >= 100 * i && b < (i + 1) * 100).Select(a => a.XMLModel).ToArray());
                urlset.Save(HttpKit.GetMapPath(string.Format("/baidu{0}.xml", i == 0 ? "" : "_" + (i + 1))));
            }
        }
    }

    public class BaiduXMLModel
    {
        public BaiduXMLModel(string date, string url)
        {
            Date = date;
            Url = url;
        }

        public string Date { get; set; }

        public string Url { get; set; }

        public XElement XMLModel
        {
            get
            {
                var url = new XElement("url", new XElement("loc", new XCData(Url)), new XElement("lastmod", new XCData(Date)), new XElement("changefreq", "always"), new XElement("priority", "1"));
                return url;
            }
        }
    }
}
