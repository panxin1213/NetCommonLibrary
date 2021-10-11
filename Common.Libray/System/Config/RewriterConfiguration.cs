using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;
using System.Configuration;

namespace System.Config
{
    [Serializable()]
    [XmlRoot("RewriterConfig")]
    public class RewriterConfiguration
    {
        private RewriterRuleCollection rules;
        /// <summary>        
        /// 该方法从web.config中读取规则集合，并使用了Cache以避免频繁IO操作        
        /// </summary>        
        /// <returns></returns>        
        public static RewriterConfiguration GetConfig()
        {
            //使用缓存            
            if (HttpContext.Current.Cache["RewriterConfig"] == null)
                HttpContext.Current.Cache.Insert("RewriterConfig", ConfigurationManager.GetSection("RewriterConfig"));
            return (RewriterConfiguration)HttpContext.Current.Cache["RewriterConfig"];
        }

        public RewriterRuleCollection Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        public static List<string> NoSuffix = new List<string> { ".jpg", ".js", ".gif", ".css", ".ico", ".png", ".bmp" };
    }
}
