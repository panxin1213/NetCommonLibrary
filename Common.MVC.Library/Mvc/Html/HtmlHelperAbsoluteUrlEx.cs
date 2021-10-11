using System.Collections.Generic;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 根据指定的Action生成包含绝对路径的a标签
    /// </summary>
    public static class HtmlHelperAbsoluteUrlEx
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            object routeValues, object htmlAttributes)
        {
            return AbsoluteActionLink(htmlHelper, linkText, actionName, null, new RouteValueDictionary(routeValues),
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return AbsoluteActionLink(htmlHelper, linkText, actionName, null, routeValues, htmlAttributes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, object routeValues, object htmlAttributes)
        {
            return AbsoluteActionLink(htmlHelper, linkText, actionName, controllerName,
                new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return AbsoluteActionLink(htmlHelper, linkText, actionName, controllerName, null, routeValues,
                htmlAttributes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="fragment"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string fragment, object routeValues, object htmlAttributes)
        {
            return AbsoluteActionLink(htmlHelper, linkText, actionName, controllerName, fragment,
                new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="fragment"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AbsoluteActionLink(this HtmlHelper htmlHelper, string linkText, string actionName,
            string controllerName, string fragment, RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            RequestContext requestContext = htmlHelper.ViewContext.RequestContext;
            string protocol = "http";
            var url = requestContext.HttpContext.Request.Url;
            if (url != null)
                protocol = url.Scheme;

            // 如果没有设置Area，则默认使用www
            //if (!routeValues.ContainsKey("Area"))
            //    routeValues.Add("Area", "www");

            var virtualPathData = htmlHelper.RouteCollection.GetVirtualPathForArea(requestContext, routeValues);
            if (virtualPathData != null)
            {
                var route = virtualPathData.Route as DomainRoute;
                if (route != null)
                {
                    var domain = route.GetDomain(routeValues);
                    return
                        MvcHtmlString.Create(HtmlHelper.GenerateLink(requestContext, htmlHelper.RouteCollection,
                            linkText, null, actionName, controllerName, protocol, domain, fragment, routeValues, htmlAttributes));
                }
            }
            return
                MvcHtmlString.Create(HtmlHelper.GenerateLink(requestContext, htmlHelper.RouteCollection, linkText, null,
                    actionName, controllerName, routeValues, htmlAttributes));
        }
    }
}
