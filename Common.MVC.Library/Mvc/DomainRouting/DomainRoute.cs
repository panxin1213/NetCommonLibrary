using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace System.Web.Routing
{
    /// <summary>
    /// 自定义域名路由
    /// </summary>
    public class DomainRoute : Route
    {
        /// <summary>
        /// 域名参数 暂不支持controller action
        /// </summary>
        public string Domain { get; internal set; }


        /// <summary>
        /// 
        /// </summary>
        private RouteValueDictionary _domainTokens {get ;set;}
 

        /// <summary>
        /// 域名的正则(初始化时生成)
        /// </summary>
        private Regex _domainRegex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, IRouteHandler routeHandler)
            :base(url,routeHandler)
        {
            Init(domain);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults)
            : base(url, defaults, new MvcRouteHandler())
        {
            Init(domain);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            Init(domain);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        public DomainRoute(string domain, string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            Init(domain);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="url"></param>
        /// <param name="defaults"></param>
        /// <param name="routeHandler"></param>
        public DomainRoute(string domain, string url, object defaults, IRouteHandler routeHandler)
            : base(url, new RouteValueDictionary(defaults), routeHandler)
        {
            Init(domain);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            
            var domain = GetRequestDomain(httpContext);
            var match = _domainRegex.Match(domain);
            
            if (!match.Success) return null;

            //必须完全重写基类才能完全支持，暂无时间 注册路由时的 constraints 参数 (在域名中的参数)
            var r = base.GetRouteData(httpContext);
            if (r == null) return null;
            foreach (var n in _domainRegex.GetGroupNames())
            {
                if (!char.IsNumber(n, 0))
                {

                    this._domainTokens[n] = match.Groups[n].Value;
                    r.Values[n] = match.Groups[n].Value; //添加路由值 以便在action中直接使用参数名
                    //r.DataTokens[n] = match.Groups[n].Value; //
                    //this.DataTokens[n] = match.Groups[n].Value;
                }
            }
            
            return r;
            
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            Remove_domainTokens(values);

            // Description: 将路由中的area, controller, action转为小写
            // Author: 丁宁
            // Date: 2014-06-21
            //LowerRouteValues(values);
            //LowerRouteValues(requestContext.RouteData.Values);
            //LowerRouteValues(Defaults);


            //这段是在url结尾加上"/"
            var virtualPath = base.GetVirtualPath(requestContext, values);
            if (virtualPath != null && !string.IsNullOrEmpty(virtualPath.VirtualPath))
            {
                int qsIndex = virtualPath.VirtualPath.IndexOf("?", StringComparison.Ordinal);

                string newPath = qsIndex >= 0
                    //? virtualPath.VirtualPath.Substring(0, qsIndex).ToLowerInvariant()
                    //: virtualPath.VirtualPath.ToLowerInvariant();
                    ? virtualPath.VirtualPath.Substring(0, qsIndex)
                    : virtualPath.VirtualPath;

                var reg = new Regex(@"\w+\.\w+$");//不匹配以.这样结尾的
                if (newPath.Length > 0 && !reg.IsMatch(newPath) && newPath[newPath.Length - 1] != '/')
                    newPath += "/";

                if (qsIndex >= 0)
                    newPath += virtualPath.VirtualPath.Substring(qsIndex);

                virtualPath.VirtualPath = newPath;
            }


            //将路由中的area, controller, action转为小写
            VirtualPathData path = virtualPath;
            if (path != null)
            {
                string virtualPathstr = path.VirtualPath;
                int lastIndexOf = virtualPathstr.LastIndexOf("?");
                if (lastIndexOf != 0)
                {
                    if (lastIndexOf > 0)
                    {
                        string leftPart = virtualPathstr.Substring(0, lastIndexOf).ToLowerInvariant();
                        string queryPart = virtualPathstr.Substring(lastIndexOf);
                        path.VirtualPath = leftPart + queryPart;
                    }
                    else
                    {
                        path.VirtualPath = path.VirtualPath.ToLowerInvariant();
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// 把域名中的参数解析出来
        /// </summary>
        /// <returns></returns>
        public string GetDomain(RouteValueDictionary values)
        {
            string hostname = Domain;
            

            foreach (KeyValuePair<string, object> pair in values.Concat(this.DataTokens).Concat(this._domainTokens))
            {
                var v = pair.Value as string;
                if (!string.IsNullOrEmpty(v))
                    hostname = hostname.Replace("{" + pair.Key + "}", v);
                
            }
            return hostname;
        }
        private void Remove_domainTokens(RouteValueDictionary values)
        {
            foreach (var k in this._domainTokens.Keys)
            {
                if (!k.Equals("controller", StringComparison.OrdinalIgnoreCase) && !k.Equals("action", StringComparison.OrdinalIgnoreCase) && !k.Equals("area", StringComparison.OrdinalIgnoreCase))
                    values.Remove(k);
            }
        }
        private string GetRequestDomain(HttpContextBase httpContent)
        {
            var domain = httpContent.Request.Headers["HOST"];
            if (!string.IsNullOrEmpty(domain))
            {
                var i = domain.IndexOf(":");
                if (i > 0)
                {
                    domain = domain.Substring(0, i);
                }
            }
            else
            {
                domain = httpContent.Request.Url.Host;
            }
            return domain.ToLower();
        }
        private void Init(string domain)
        {
            Domain = domain.ToLower();
            // 替换
            domain = domain.Replace("/", @"\/?")
                .Replace(".", @"\.?")
                .Replace("-", @"\-?")
                .Replace("{", @"(?<")
                .Replace("}", @">([a-zA-Z0-9_]*))");
            _domainRegex = new Regex("^" + domain + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);


            _domainTokens = new RouteValueDictionary();
            foreach (var n in _domainRegex.GetGroupNames())
            {
                if (!char.IsNumber(n, 0))
                {
                    _domainTokens[n] = String.Empty;
                }
            }
           



        }

        private static readonly string[] RouteKeys = {"controller", "action", "area"};

        private void LowerRouteValues(RouteValueDictionary routeValues) {
            try
            {
                //new一个新的防止多线程循环修改集合 change by lbq 2014-07-22
                var newRouteValues = new RouteValueDictionaryEx(routeValues);
                foreach (var key in RouteKeys)
                {
                    if (!newRouteValues.ContainsKey(key)) continue;

                    var value = newRouteValues[key];
                    if (value == null) continue;

                    var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);

                    newRouteValues[key] = valueString.ToLower();
                }

                var otherKeys = newRouteValues.Keys.Except(RouteKeys, StringComparer.InvariantCultureIgnoreCase).ToArray();
                foreach (var key in otherKeys)
                {
                    var value = newRouteValues[key];
                    newRouteValues.Remove(key);
                    newRouteValues[key.ToLower()] = value;
                    //routeValues.Add(key.ToLower(), value);
                }
                routeValues = newRouteValues;
            }
            catch (Exception)
            {                  
                throw;
            }
        }
    }
}
