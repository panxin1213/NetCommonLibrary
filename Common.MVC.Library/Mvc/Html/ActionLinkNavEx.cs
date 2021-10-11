using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
namespace System.Web.Mvc.Html
{
    /// <summary>
    /// htmlHelper 导航扩展
    /// </summary>
    public static class ActionLinkNavEx
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="linkText"></param>
        ///// <param name="actionName"></param>
        ///// <returns></returns>
        //public static MvcHtmlString NavActionLink(this string htmlHelper, string linkText, string actionName)
        //{
        //    return htmlHelper.NavActionLink(linkText, actionName, null, null, null, null, null, null);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return htmlHelper.NavActionLink(linkText, actionName, null, null, null, null, new RouteValueDictionary(routeValues), null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return htmlHelper.NavActionLink(linkText, actionName, null, null, null, null, routeValues, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return htmlHelper.NavActionLink(linkText, actionName, controllerName, null, null, null, null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.NavActionLink(linkText, actionName, null, null, null, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.NavActionLink(linkText, actionName, null, null, null, null, routeValues, htmlAttributes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.NavActionLink(linkText, actionName, controllerName, null, null, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.NavActionLink(linkText, actionName, controllerName, null, null, null, routeValues, htmlAttributes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="protocol"></param>
        /// <param name="hostName"></param>
        /// <param name="fragment"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return htmlHelper.NavActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        private static readonly RouteValueDictionary emptyRoute = new RouteValueDictionary();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="protocol"></param>
        /// <param name="hostName"></param>
        /// <param name="fragment"></param>
        /// <param name="routeValues">路由值 如果设置_default=1，当前controller所有action都会选中该链接</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString NavActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            
            var cacheKey = "_nav_route_cache" + htmlHelper.GetHashCode();
            //当前View里的路由
            var RouteCurrent = System.Web.HttpContext.Current.Items[cacheKey] as RouteValueDictionary;
            //当前View里的模型状态
            var ViewState = System.Web.HttpContext.Current.Items[cacheKey + "_viewstate"] as IDictionary<string, object>;
            if (RouteCurrent == null)
            {
                RouteCurrent = htmlHelper.ViewContext.RouteData.Values;
                System.Web.HttpContext.Current.Items.Add(cacheKey, RouteCurrent);

                ViewState = htmlHelper.ViewData.ModelState.Where(a=>a.Value.Errors.Count==0).Select(a => new KeyValuePair<string, object>(a.Key, a.Value.Value.AttemptedValue)).ToDictionary(a => a.Key, a => a.Value);
                System.Web.HttpContext.Current.Items.Add(cacheKey + "_viewstate", ViewState);
            }



            controllerName = string.IsNullOrEmpty(controllerName) ? routeValues.TryGetValue("Controller", RouteCurrent.TryGetValue("Controller", null)).ToString() : controllerName;
            actionName = string.IsNullOrEmpty(actionName) ? routeValues.TryGetValue("Action", RouteCurrent.TryGetValue("Action", null)).ToString() : actionName;
            //把设置中的路由补充完整 ,Controller,Action
            var RouteSet = new List<KeyValuePair<string, object>>();
            RouteSet.Add(new KeyValuePair<string, object>("Controller", controllerName));
            
            //因为routeValues也可设置Controller和Action所以。如果在有显式设置了Controller或Action时移除 routeValues里设置的值
            if (linkText == "合作客户")
            {
                var x =DateTime.Now;
            }
            if (routeValues == null)
            {
                routeValues = emptyRoute;
            }
            routeValues.Remove("Controller");
            routeValues.Remove("Action");
            if (linkText == "业务到款审对")
            {
                var x = DateTime.Now;
            }
            //默认选中所有Action
            var isdefault = routeValues.TryGetValue("default", "").ToString().Equals("1");

            //排除的Action值
            var excludeAction = routeValues.TryGetValue("excludeAction", "").ToString().ToLower();

            bool isExcluded = true;
            if (!String.IsNullOrEmpty(excludeAction))
                isExcluded = ("," + excludeAction + ",").IndexOf("," + (RouteCurrent.TryGetValue("Action", null)) + ",") >= -1;

            routeValues.Remove("default");
            if (isdefault&&!isExcluded) //如果指定的Action = "Index" 在其它Action值时也选中
            {
                
            }
            else
            {
                RouteSet.Add(new KeyValuePair<string, object>("Action", actionName));
            }
           
            //是否同一area下
            var sameArea = routeValues.TryGetValue("Area", "").ToString().Equals(htmlHelper.ViewContext.RouteData.DataTokens.TryGetValue("Area", "").ToString(), StringComparison.OrdinalIgnoreCase);

            if ( sameArea && RouteCurrent.ContainsAll(RouteSet) && (isdefault || ViewState.ContainsAll(routeValues, new string[] { "Area" })))
            {
                if (htmlAttributes == null)
                    htmlAttributes = new Dictionary<string, object>();
                if (htmlAttributes.ContainsKey("class"))
                {
                    htmlAttributes["class"] += " current";
                }
                else
                {
                    htmlAttributes.Add("class", "current");
                }
            };
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }
        /*
        private static bool RouteValueEquals(this RouteValueDictionary routeValues, object[] navRouteDatas)
        {
            if (navRouteDatas == null) return false;

        }*/
        /*
        /// <summary>
        /// 检查当前的路由都包含navRouteData的属性
        /// </summary>
        /// <param name="routeValues">当前路由</param>
        /// <param name="navRouteData">设置的路由</param>
        /// <returns></returns>
        public static bool ContainsAll(this RouteValueDictionary routeValues, object navRouteData)
        {
            var navRoute = new RouteValueDictionary(navRouteData);

            foreach (var r in navRoute)
            {
                if (!routeValues.TryGetValue(r.Key, null).Equals(r.Value))
                {
                    return false;
                }
            }
            return true;
        }*/
        /// <summary>
        /// 设置默认未设置设置的一些MVC必有的路由，如导航没有设置
        /// </summary>
        /// <param name="routeValues"></param>
        /// <param name="navRoute"></param>
        private static void SetMvcValueDefalut(RouteValueDictionary routeValues, RouteValueDictionary navRoute)
        {
            SetMvcValue(routeValues, navRoute, "Controller");
            SetMvcValue(routeValues, navRoute, "Action");
            SetMvcValue(routeValues, navRoute, "Area");
        }
        /// <summary>
        /// 设置单个路由，如导航果没有设置
        /// </summary>
        /// <param name="routeValues"></param>
        /// <param name="navRoute"></param>
        /// <param name="key"></param>
        private static void SetMvcValue(RouteValueDictionary routeValues, RouteValueDictionary navRoute, string key)
        {
            if (!navRoute.ContainsKey(key)) navRoute.Add(key, routeValues.TryGetValue(key, null));
        }
    }
}
