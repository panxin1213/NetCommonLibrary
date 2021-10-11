using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Plugins.QQ.Section
{
    /// <summary>
    /// QQ登录应用配置
    /// </summary>
    public class QQConfigSection : ConfigurationSection
    {
        private static QQConfigSection _current = ConfigurationManager.GetSection("qqconfigsection") as QQConfigSection;

        public static QQConfigSection Current
        {
            get
            {
                return _current;
            }
        }

        /// <summary>
        /// 授权回调地址
        /// </summary>
        [ConfigurationProperty("authurl", IsRequired = true)]
        public DefaultElement AuthUrl
        {
            get
            {
                return (base["authurl"] as DefaultElement);
            }
        }

        /// <summary>
        /// 应用程序key
        /// </summary>
        [ConfigurationProperty("appkey", IsRequired = true)]
        public DefaultElement AppKey
        {
            get
            {
                return (base["appkey"] as DefaultElement);
            }
        }

        /// <summary>
        /// 应用程序key
        /// </summary>
        [ConfigurationProperty("appsecret", IsRequired = true)]
        public DefaultElement AppSecret
        {
            get
            {
                return (base["appsecret"] as DefaultElement);
            }
        }

        /// <summary>
        /// 服务商
        /// </summary>
        [ConfigurationProperty("server", IsRequired = true)]
        public DefaultElement Server
        {
            get
            {
                return (base["server"] as DefaultElement);
            }
        }

        /// <summary>
        /// 接口类型配置
        /// </summary>
        [ConfigurationProperty("operation", IsRequired = true)]
        public OperationElement Operation
        {
            get
            {
                return (base["operation"] as OperationElement);
            }
        }

        /// <summary>
        /// 是否是移动端
        /// </summary>
        public bool IsMobile
        {
            get
            {
                return false;
            }
        }




        /// <summary>
        /// 默认配置元素
        /// </summary>
        public class DefaultElement : ConfigurationElement
        {
            [ConfigurationProperty("value", IsRequired = true)]
            public string Value
            {
                get
                {
                    return (base["value"] as string);
                }
            }
        }

        /// <summary>
        /// 接口类型元素
        /// </summary>
        public class OperationElement : ConfigurationElement
        {
            /// <summary>
            /// 程序集
            /// </summary>
            [ConfigurationProperty("assembly", IsRequired = true)]
            public string Assembly
            {
                get
                {
                    return (base["assembly"] as string);
                }
            }

            /// <summary>
            /// 类型名称
            /// </summary>
            [ConfigurationProperty("type", IsRequired = true)]
            public string Type
            {
                get
                {
                    return (base["type"] as string);
                }
            }
        }
    }

}
