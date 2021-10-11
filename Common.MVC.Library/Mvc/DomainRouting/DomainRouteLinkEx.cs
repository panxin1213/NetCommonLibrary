using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Diagnostics;

namespace System.Web.Mvc.Html
{


    /// <summary>
    /// 支持二级域名的链接，绝对地址 
    /// </summary>
    public  static class DomainRouteLinkEx
    {
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return Link(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(), new RouteValueDictionary());
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <param name="routeValues">路由值</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return Link(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <param name="routeValues">路由值</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return Link(htmlHelper, linkText, actionName, null /* controllerName */, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName">Action名</param>
        /// <param name="linkText">链接文字</param>
        /// <param name="routeValues">路由值</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return Link(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, new RouteValueDictionary());
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <param name="routeValues">路由值</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return Link(htmlHelper, linkText, actionName, null /* controllerName */, routeValues, htmlAttributes);
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <param name="controllerName">Controller名</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return Link(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <param name="controllerName">Controller名</param>
        /// <param name="routeValues">路由值</param>
        /// <param name="htmlAttributes">Html属性</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return Link(htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 链接 【待优化】
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">Action名</param>
        /// <param name="controllerName">Controller名</param>
        /// <param name="routeValues">路由值</param>
        /// <param name="htmlAttributes">Html属性</param>
        /// <returns></returns>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            
            var vpd = htmlHelper.RouteCollection.GetVirtualPathForArea(htmlHelper.ViewContext.RequestContext, routeValues);
            string url;
            if (vpd != null && vpd.Route != null && typeof(DomainRoute).IsAssignableFrom(vpd.Route.GetType()))
            {
                var x = vpd.Route as DomainRoute;
                url = HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, "http", x.GetDomain(routeValues), null, routeValues, htmlAttributes);
            }
            else
            {
                url = HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null /* routeName */, actionName, controllerName, routeValues, htmlAttributes);
            }

            return MvcHtmlString.Create(url);

        }
        
        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues)
        //{
        //    return RouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues));
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues)
        //{
        //    return RouteLink(htmlHelper, linkText, routeValues, new RouteValueDictionary());
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName)
        //{
        //    return RouteLink(htmlHelper, linkText, routeName, (object)null /* routeValues */);
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues)
        //{
        //    return RouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues));
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues)
        //{
        //    return RouteLink(htmlHelper, linkText, routeName, routeValues, new RouteValueDictionary());
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, object routeValues, object htmlAttributes)
        //{
        //    return RouteLink(htmlHelper, linkText, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        //{
        //    return RouteLink(htmlHelper, linkText, null /* routeName */, routeValues, htmlAttributes);
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        //{
        //    return RouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        //}

        //public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        //{
        //    var vpd = htmlHelper.RouteCollection.GetVirtualPathForArea(htmlHelper.ViewContext.RequestContext, routeValues);
        //    if (vpd != null && typeof(DomainRoute).IsAssignableFrom(vpd.Route.GetType()))
        //    {
        //        var x = vpd.Route as DomainRoute;
        //        return MvcHtmlString.Create(HtmlHelper.GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, routeValues, htmlAttributes));
        //    }
        //    else { 
            
        //    }
        //}

        
    }
}
