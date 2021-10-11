using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace System
{
    /// <summary>
    /// Helper classes for URL handler
    /// </summary>

    public static class UrlUtility
    {
        /// <summary>
        /// Convert relative url to absolute url
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string ToAbsolute(string relativeUrl, object routeValues = null)
        {
            var uri = new Uri(relativeUrl, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
            {
                if (HttpContext.Current != null)
                {
                    Uri requestUri = HttpContext.Current.Request.Url;
                    //var mvcHandler = HttpContext.Current.CurrentHandler as MvcHandler;
                    
                    //if (mvcHandler != null)
                    {
                        var values = routeValues == null
                            ? new RouteValueDictionary()
                            : new RouteValueDictionary(routeValues);
                        // 如果没有设置Area，则默认使用www
                        if (!values.Any(a => a.Key.Equals("area", StringComparison.OrdinalIgnoreCase)))
                            values.Add("Area", "www");
                        //var virtualPathData = RouteTable.Routes.GetVirtualPathForArea(mvcHandler.RequestContext, values);
                        var virtualPathData = RouteTable.Routes.GetVirtualPathForArea(HttpContext.Current.Request.RequestContext, values);
                        var route = virtualPathData.Route as DomainRoute;
                        if (route != null)
                        {
                            var builder = new UriBuilder(requestUri.Scheme, route.GetDomain(values), requestUri.Port, relativeUrl);
                            uri = builder.Uri;
                        }
                    }
                }
            }
            return uri.ToString();
        }
    }
}
