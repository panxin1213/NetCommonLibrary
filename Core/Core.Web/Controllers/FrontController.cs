using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Text.RegularExpressions;
using Core.Base;
using ChinaBM.Common;

namespace Core.Web.Controllers
{
    public class FrontController : BaseController
    {
        private static readonly Regex mobileurlregex = new Regex(@"^http(s)?://(?<root>((?!\.).)*)\.(((?!/).)*)/(?<end>(.)*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex pcurlregex = new Regex(@"^http://" + BaseConfig.Current.Routes["mobile"] + "(?:/)?(?<root>(?:(?!/).)*)/?(?<end>(.)*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var area = filterContext.RouteData.Values.TryGetValue("area", "").ToSafeString();

            if (!area.Equals("mobile", StringComparison.OrdinalIgnoreCase))
            {
                var mobileurl = ViewBag.MobileUrl as string;

                if (mobileurl == null)
                {
                    var match = mobileurlregex.Match(filterContext.HttpContext.Request.Url.ToSafeString());

                    if (match.Success)
                    {
                        var root = match.Groups["root"].Value;
                        if (root.ToSafeString().ToLower() != "www")
                        {
                            root = root + "/";
                        }
                        else
                        {
                            root = "";
                        }
                        var end = match.Groups["end"].Value;

                        ViewBag.MobileUrl = string.Format("http://{0}/{1}{2}", BaseConfig.Current.Routes["mobile"], root, end).ToLower();
                    }
                }
            }
            else
            {
                var pcurl = ViewBag.PcUrl as string;

                if (pcurl == null)
                {
                    //var match = pcurlregex.Match(filterContext.HttpContext.Request.Url.ToSafeString());

                    //if (match.Success)
                    //{
                    //    var root = match.Groups["root"].Value;
                    //    var end = match.Groups["end"].Value;

                    //    if (end.IndexOf("/") > -1 && !String.IsNullOrWhiteSpace(root))
                    //    {
                    //        ViewBag.PcUrl = string.Format("http://{0}/{1}", root + "." + BaseConfig.Current.DomainBase, end);
                    //    }
                    //    else if (String.IsNullOrWhiteSpace(end) && String.IsNullOrWhiteSpace(root))
                    //    {
                    //        ViewBag.PcUrl = string.Format("http://{0}/", BaseConfig.Current.Routes["www"]);
                    //    }
                    //    else if (end.IndexOf("/") == -1 && !String.IsNullOrWhiteSpace(root))
                    //    {
                    //        var isroute = BaseConfig.Current.Routes.ContainsKey(root.ToLower());
                    //        ViewBag.PcUrl = string.Format("http://{0}/{1}", (isroute ? BaseConfig.Current.Routes[root.ToLower()] : BaseConfig.Current.Routes["www"]), isroute ? end : root + "/" + end);
                    //    }

                    //}

                    ViewBag.PcUrl = filterContext.HttpContext.Request.Url.ToSafeString().Replace("http://m.", "http://www.");
                }
            }

            base.OnActionExecuted(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var area = filterContext.RouteData.Values.TryGetValue("area", "").ToSafeString();

            if (!area.Equals("mobile", StringComparison.OrdinalIgnoreCase))
            {
                var mobileurl = ViewBag.MobileUrl as string;

                if (!String.IsNullOrWhiteSpace(mobileurl))
                {
                    if (HttpKit.IsMobile()
                        && HttpKit.GetCookie("mobile") != "pc"
                        )
                    {
                        filterContext.HttpContext.Response.Clear();
                        filterContext.HttpContext.Response.Redirect(mobileurl);
                        filterContext.HttpContext.Response.End();
                        return;
                    }
                }
            }

            base.OnResultExecuted(filterContext);
        }
    }
}
