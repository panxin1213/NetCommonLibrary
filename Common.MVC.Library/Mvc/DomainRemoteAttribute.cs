using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// area 域名 远程验证特性(重写了GetUrl取址方法,如需客户端,则需要jsonp)
    /// </summary>
    public class DomainRemoteAttribute : RemoteAttribute
    {
        /// <summary>
        /// 当前字段名称
        /// </summary>
        private string _thisFieldName = null;

        public DomainRemoteAttribute(string action, string controller, string areaName, string thisFieldName)
            : base(action, controller, areaName)
        {
            _thisFieldName = thisFieldName;
        }

        protected override string GetUrl(ControllerContext controllerContext)
        {
            return GetUrl(controllerContext.RequestContext);
        }

        private string GetUrl(RequestContext context)
        {
            var pathData = Routes.GetVirtualPathForArea(context,
                                                        RouteName,
                                                        RouteData);

            return new UrlHelper(context).AbsoluteUrl(pathData.VirtualPath, RouteData["area"].ToSafeString());
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var requestcontext = System.Web.HttpContext.Current.Request.RequestContext;

            var url = GetUrl(requestcontext);

            if (requestcontext != null && (!new Uri(url).AbsolutePath.Equals(System.Web.HttpContext.Current.Request.Url.AbsolutePath, StringComparison.OrdinalIgnoreCase)))
            {
                var param = new Dictionary<string, object>();

                var t = validationContext.ObjectInstance.GetType();

                param.Add(_thisFieldName, value);

                var fields = this.AdditionalFields.ToSafeString().Split(',').Where(a => !String.IsNullOrEmpty(a));

                foreach (var item in fields)
                {
                    var p = t.GetProperties().SingleOrDefault(a => a.Name.Equals(item));
                    if (p != null)
                    {
                        param.Add(item, p.GetValue(validationContext.ObjectInstance, null));
                    }
                }

                using (WebClient client = new WebClient())
                {
                    url += "?" + string.Join("&", param.Select(a => a.Key + "=" + a.Value));
                    if (client.DownloadString(url).ToSafeString().IndexOf("false") > -1)
                    {
                        return new ValidationResult("");
                    }
                }
            }


            return null;
        }
    }
}
