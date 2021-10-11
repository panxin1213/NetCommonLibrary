using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Base
{
    /// <summary>
    /// 网站设置
    /// </summary>
    public class BaseConfig : ConfigurationSection
    {
        private static BaseConfig _current = ConfigurationManager.GetSection("baseConfig") as BaseConfig;

        public static string chooselink = null;
        public static BaseConfig Current
        {
            get
            {

                return _current;
            }
        }

        /// <summary>
        /// 路径相关设置
        /// </summary>
        [ConfigurationProperty("path")]
        public PathSetting Path
        {
            get
            {
                return base["path"] as PathSetting;
            }
        }
        /// <summary>
        /// URL相关设置
        /// </summary>
        [ConfigurationProperty("url")]
        public UrlSetting Url
        {
            get
            {
                return base["url"] as UrlSetting;
            }
        }


        /// <summary>
        /// 网站根目录绝对路径
        /// </summary>
        public string ServerRoot
        {
            get
            {
                return HttpContext.Current.Server.MapPath("/");
            }
        }
        /// <summary>
        /// 主域名不加www.
        /// </summary>
        [ConfigurationProperty("domain_base", IsRequired = true)]
        public string DomainBase
        {
            get
            {
                return this["domain_base"] as string ?? "";
            }
        }

        [ConfigurationProperty("site_name")]
        public string SiteName
        {
            get
            {
                return this["site_name"] as string ?? "";
            }
        }

        [ConfigurationProperty("seo_key")]
        public string SeoKey
        {
            get
            {
                return this["seo_key"] as string ?? "";
            }
        }

        /// <summary>
        /// 接口类型配置
        /// </summary>
        [ConfigurationProperty("servicemainparam")]
        public ServiceMainElement ServiceMainParam
        {
            get
            {
                return (base["servicemainparam"] as ServiceMainElement);
            }
        }

        /// <summary>
        /// 路由配置
        /// </summary>
        [ConfigurationProperty("routes")]
        private RoutesSetting routesSetting
        {
            get { return base["routes"] as RoutesSetting; }
        }

        /// <summary>
        /// 路由字典
        /// </summary>
        public IDictionary<string, string> Routes
        {
            get
            {
                if (routesSetting == null) return new Dictionary<string, string>();

                return routesSetting.Cast<RoutesSetting.RouteItem>()
                    .ToDictionary(route => route.Name, route => route.Domain);
            }
        }


        public class PathSetting : ConfigurationElement
        {
            /// <summary>
            /// js和css合并文件hander的地址
            /// </summary>
            [ConfigurationProperty("resource")]
            public string Resource
            {
                get
                {
                    return this["resource"] as string ?? "";
                }
            }
            /// <summary>
            /// 图片缩略图在图片服务器中的地址
            /// </summary>
            [ConfigurationProperty("image_thumb", IsRequired = true)]
            public string ImageThumb
            {
                get
                {
                    return this["image_thumb"] as string ?? "";
                }
            }
        }


        public class UrlSetting : ConfigurationElement
        {
            public readonly string Path_Server_Root = HttpContext.Current.Server.MapPath("/");
            /// <summary>
            /// 登录链接
            /// </summary>
            [ConfigurationProperty("login")]
            public string Login
            {
                get
                {
                    if (chooselink != null)
                    {
                        return chooselink;
                    }
                    return base["login"] as string;
                }
            }
            /// <summary>
            /// 注册链接
            /// </summary>
            [ConfigurationProperty("register")]
            public string Register
            {
                get
                {
                    return base["register"] as string;
                }
            }
            /// <summary>
            /// 退出链接
            /// </summary>
            [ConfigurationProperty("logout")]
            public string Logout
            {
                get
                {
                    return base["logout"] as string;
                }
            }


            /// <summary>
            /// 主图片服务器，用于上传文件等必须是主域名下的域名。为了不跨域
            /// </summary>
            [ConfigurationProperty("image_master", IsRequired = true)]
            public string ImageMaster
            {
                get
                {
                    return base["image_master"] as string;
                }
            }

            /// <summary>
            /// 主图片服务器，用于上传文件等必须是主域名下的域名。为了不跨域
            /// 说明：这个是为了之前图片服务器没有设置域名，image_master有地方已经用了
            /// </summary>
            [ConfigurationProperty("image_master_server")]
            public string ImageMasterServer
            {
                get
                {
                    return base["image_master_server"] as string;
                }
            }

            public List<string> ImageServers
            {
                get
                {
                    var x = _imageServers;
                    if (x != null)
                    {
                        var r = new List<string>();
                        foreach (BaseConfig.UrlSetting.UrlImageServerItems.UrlImageServerItem o in x)
                        {
                            r.Add(o.Url);
                        }
                        return r;
                    }
                    else
                    {
                        throw new Exception("至少定义一个图片服务器地址");
                    }
                }
            }
            /// <summary>
            /// 副图片服务器列表，（可以为其它域名）
            /// </summary>
            [ConfigurationProperty("image_servers")]
            private UrlImageServerItems _imageServers
            {
                get
                {
                    return (this["image_servers"] as UrlImageServerItems);
                }
            }



            /// <summary>
            /// 附图片服务器
            /// </summary>
            public class UrlImageServerItems : ConfigurationElementCollection
            {

                protected override ConfigurationElement CreateNewElement()
                {
                    return new UrlImageServerItem();
                }
                protected override object GetElementKey(ConfigurationElement element)
                {
                    return ((UrlImageServerItem)element).Url;
                }

                public class UrlImageServerItem : ConfigurationElement
                {
                    [ConfigurationProperty("url", IsRequired = true)]
                    public string Url
                    {
                        get
                        {
                            return base["url"] as string;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 路由配置
        /// </summary>
        public class RoutesSetting : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new RouteItem();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((RouteItem)element).Name;
            }

            /// <summary>
            /// 单个路由配置项
            /// </summary>
            public class RouteItem : ConfigurationElement
            {
                /// <summary>
                /// 路由名称
                /// </summary>
                [ConfigurationProperty("name", IsRequired = true)]
                public string Name
                {
                    get { return base["name"] as string; }
                }

                /// <summary>
                /// 路由域名
                /// </summary>
                [ConfigurationProperty("domain", IsRequired = true)]
                public string Domain
                {
                    get { return base["domain"] as string; }
                }
            }
        }

        /// <summary>
        /// ServiceMain类型元素
        /// </summary>
        public class ServiceMainElement : ConfigurationElement
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
