using System.Web.Routing;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Core.Base;

namespace System.Web.Mvc
{
    /// <summary>
    /// 生成绝对路径(扩展UrlHelper)
    /// </summary>
    public static class AbsoluteUrlHelperEx
    {
        /// <summary>
        /// 生成绝对路径
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, object routeValues = null)
        {
            return AbsoluteAction(urlHelper, actionName, null, routeValues);
        }

        /// <summary>
        /// 生成绝对路径
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, RouteValueDictionary values)
        {
            return AbsoluteAction(urlHelper, actionName, null, values);
        }

        /// <summary>
        /// 生成绝对路径
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, string controllerName,
            object routeValues = null)
        {
            var values = new RouteValueDictionaryEx(routeValues);
            return AbsoluteAction(urlHelper, actionName, controllerName, values);
        }

        /// <summary>
        /// 生成绝对路径
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper urlHelper, string actionName, string controllerName,
            RouteValueDictionary values)
        {
            string protocol = "http";
            var url = urlHelper.RequestContext.HttpContext.Request.Url;
            if (url != null)
                protocol = url.Scheme;

            var host = urlHelper.RequestContext.HttpContext.Request.Url.Host;

            var area = values.FirstOrDefault(a => a.Key.Equals("area", StringComparison.OrdinalIgnoreCase)).Value.ToSafeString().ToLower();

            if (String.IsNullOrWhiteSpace(area))
            {
                area = urlHelper.RequestContext.RouteData.Values.TryGetValue("area", "").ToSafeString();
            }

            if (!String.IsNullOrWhiteSpace(area))
            {
                if (BaseConfig.Current.Routes.ContainsKey(area))
                {
                    host = BaseConfig.Current.Routes[area];
                }
            }

            var must = new[] { "area", "controller", "action" };
            var removekeys = new Dictionary<string, object>();
            foreach (var item in urlHelper.RequestContext.RouteData.Values)
            {
                if (!must.Any(a => a.Equals(item.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    removekeys.Add(item.Key, item.Value);
                }
            }
            foreach (var item in removekeys)
            {
                urlHelper.RequestContext.RouteData.Values.Remove(item.Key);
            }


            var returnurl = urlHelper.Action(actionName, controllerName, values, protocol, host);

            foreach (var item in removekeys)
            {
                urlHelper.RequestContext.RouteData.Values.Add(item.Key, item.Value);
            }

            return returnurl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public static string DomainRouteUrl(this UrlHelper urlHelper, string routeName) {
            return DomainRouteUrl(urlHelper, routeName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string DomainRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues) {
            return DomainRouteUrl(urlHelper, routeName, new RouteValueDictionaryEx(routeValues));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string DomainRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues) {
            string protocol = "http";
            var url = urlHelper.RequestContext.HttpContext.Request.Url;
            if (url != null)
                protocol = url.Scheme;
            var virtualPathData = urlHelper.RouteCollection.GetVirtualPathForArea(urlHelper.RequestContext, routeName,
                routeValues);
            if (virtualPathData != null) {
                var route = virtualPathData.Route as DomainRoute;
                if (route != null) {
                    var domain = route.GetDomain(routeValues);
                    return urlHelper.RouteUrl(routeName, routeValues, protocol, domain);
                }
            }
            return urlHelper.RouteUrl(routeName, routeValues, protocol);
        }
    }
}
