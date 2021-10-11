using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using ChinaBM.Common;

namespace Core.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Service集合类
        /// </summary>
        protected ServiceMain ServiceMain = null;


        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            ServiceMain = System.Web.HttpContext.Current.GetServiceMain();
        }



        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var area = filterContext.RouteData.Values.TryGetValue("area", "").ToSafeString();

            if (area.Equals("mobile", StringComparison.OrdinalIgnoreCase))
            {
                HttpKit.WriteCookie("mobile", null, -360);
            }
            else
            {
                if (HttpKit.GetUrlParam("mobile", true).ToSafeString().Equals("pc", StringComparison.OrdinalIgnoreCase))
                {
                    HttpKit.WriteCookie("mobile", "pc");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
