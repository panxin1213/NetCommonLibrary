using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.Xml;

namespace System.Config
{
    public class RewriterConfig : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员

        /// <summary>        
        /// 该方法无需主动调用        
        /// 它在ConfigurationManager.GetSection()被调用时根据改配置节声明中所定义的类名和路径自动实例化配置节处理类        
        /// </summary>        
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XmlSerializer ser = new XmlSerializer(typeof(RewriterConfiguration));
            return ser.Deserialize(new XmlNodeReader(section));
        }

        #endregion
    }
}
