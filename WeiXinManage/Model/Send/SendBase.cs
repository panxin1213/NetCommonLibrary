using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace WeiXinManage.Model
{
    public class SendBase
    {
        /// <summary>
        /// 接收方账号，必须
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号，必须
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间，必须
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 消息类型，必须
        /// </summary>
        protected string MsgType { get; set; }

        /// <summary>
        /// 返回模型对应xml字符串
        /// </summary>
        /// <returns></returns>
        public string GetXmlString()
        {
            var document = new XElement("xml");

            var type = this.GetType();

            var nocdata = new[] { "ArticleCount", "CreateTime" };

            foreach (var item in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (item.PropertyType.IsGenericType)
                {
                    var l = item.GetValue(this, null) as List<Article>;

                    var nodelist = new XElement("Articles");
                    foreach (var citem in l)
                    {
                        var node = new XElement("item");
                        node.Add(citem.GetType().GetProperties().Select(a => new XElement(a.Name, new XCData(a.GetValue(citem, null).ToSafeString()))).ToArray());
                        nodelist.Add(node);
                    }
                    document.Add(nodelist);
                }
                else
                {
                    var value = item.GetValue(this, null).ToSafeString();
                    if (nocdata.Any(a => a.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        document.Add(new XElement(item.Name, value));
                    }
                    else
                    {
                        document.Add(new XElement(item.Name, new XCData(value)));
                    }
                }
            }

            return document.ToString();
        }
    }
}
