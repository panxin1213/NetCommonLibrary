using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Routing
{
    /// <summary>
    /// 注册域名路由
    /// </summary>
    public static class RouteCollectionEx
    {
        /// <summary>
        /// 注册域名路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Route MapRoute(this RouteCollection routes, string name, string domain, string url)
        {
            return MapRoute(routes, name, url, null /* defaults */, (object)null /* constraints */);
        }
        /// <summary>
        /// 注册域名路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <returns></returns>
        public static Route MapRoute(this RouteCollection routes, string name, string domain, string url, object defaults)
        {
            return MapRoute(routes, name ,domain, url, defaults, (object)null /* constraints */);
        }
        /// <summary>
        /// 注册域名路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static Route MapRoute(this RouteCollection routes, string name, string domain, string url, object defaults, object constraints)
        {
            return MapRoute(routes, name ,domain, url, defaults, constraints, null /* namespaces */);
        }
        /// <summary>
        /// 注册域名路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapRoute(this RouteCollection routes, string name, string domain, string url, string[] namespaces)
        {
            return MapRoute(routes, name,domain, url, null /* defaults */, null /* constraints */, namespaces);
        }
        /// <summary>
        /// 注册域名路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapRoute(this RouteCollection routes, string name, string domain, string url, object defaults, string[] namespaces)
        {
            return MapRoute(routes, name,domain, url, defaults, null /* constraints */, namespaces);
        }
        /// <summary>
        /// 注册域名路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="constraints"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        public static Route MapRoute(this RouteCollection routes, string name ,string domain, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            Route route = new DomainRoute(domain,url, new MvcRouteHandler())
            {
                Defaults = CreateRouteValueDictionary(defaults),
                Constraints = CreateRouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            
            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }
            if (route.Defaults.ContainsKey("Area"))
            {
                route.DataTokens["area"] = route.Defaults["area"];
                bool useNamespaceFallback = (namespaces == null || namespaces.Length == 0);
                route.DataTokens["UseNamespaceFallback"] = useNamespaceFallback;
            }
            



            routes.Add(name, route);

            return route;
        }

        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            var dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new RouteValueDictionary(dictionary);
            }

            return new RouteValueDictionary(values);
        }

    }
}
