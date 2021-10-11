using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// 域名路由链接
    /// </summary>
    public static class DomainRouteUrlEx
    {
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static  string Link(this UrlHelper urlHelper, string actionName)
        {
            return urlHelper.Link(actionName, null, null, null, null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, object routeValues)
        {
            return urlHelper.Link(actionName, null,new RouteValueDictionary( routeValues), null, null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, RouteValueDictionary routeValues)
        {
            return urlHelper.Link(actionName, null, routeValues, null, null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, string controllerName)
        {
            return urlHelper.Link(actionName, controllerName, null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues)
        {
            return urlHelper.Link(actionName, controllerName, new RouteValueDictionary( routeValues), null, null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return urlHelper.Link(actionName,controllerName, routeValues ?? new RouteValueDictionary(),null,null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, string protocol)
        {
            return urlHelper.Link(actionName, controllerName,new RouteValueDictionary(routeValues), protocol, null);
        }
        /// <summary>
        /// 域名路由链接
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="protocol"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static string Link(this UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            if (String.IsNullOrEmpty(hostName))
            {
                VirtualPathData vpd = urlHelper.RouteCollection.GetVirtualPathForArea(urlHelper.RequestContext,  routeValues);
                if (vpd != null&& vpd.Route != null && typeof(DomainRoute).IsAssignableFrom(vpd.Route.GetType()))
                {
                    var x = vpd.Route as DomainRoute;
                    var r = urlHelper.Action(actionName, controllerName, routeValues, "http", x.GetDomain(routeValues));
                    return r;
                }

                

            }
            return urlHelper.Action(actionName,controllerName,routeValues,protocol,hostName);
        }
    }
}
