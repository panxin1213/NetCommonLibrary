using ChinaBM.Common;
using Core.Manage.ManageShip;
using Core.Web;
using Core.Web.Controllers;
using Core.Web.ManageShip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Core.Admin.Controllers
{
    public class ManageController : BaseController
    {
        protected new CustomPrincipal User { get; private set; }
        protected CustomIdentity Identity { get; private set; }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var controller = filterContext.RouteData.Values["Controller"].ToSafeString();
            var action = filterContext.RouteData.Values["Action"].ToSafeString();

            var User = filterContext.HttpContext.Items[ManageCookieManager.HttpContextKey] as CustomPrincipal;

            System.Web.HttpContext.Current.User = User;

            Identity = User.Identity as CustomIdentity;

            if (Identity == null || !Identity.IsAuthenticated)
            {
                filterContext.Result = Redirect(Url.AbsoluteAction("login", "home", new { area = "manage", r_url = Request.GetFullUrl() }));
                ManageProvider.Logout();
                return;
            }

            if (Identity.IsSupper)
            {
                return;
            }

            var clist = ManagePermissions.GetAllPermissions();

            ViewBag.ControllerList = clist;//页面取得总权限

            if (clist.Any(a => a.Value.Any(b => b.Controller.Equals(ConvertKit.Convert(controller, string.Empty), StringComparison.OrdinalIgnoreCase) 
                && b.Action.Equals(ConvertKit.Convert(action, string.Empty), StringComparison.OrdinalIgnoreCase))))
            {
                if (!User.IsInRight(ConvertKit.Convert(controller, string.Empty), ConvertKit.Convert(action, string.Empty)))
                {

                    filterContext.Result = this.JavascriptContent("alert('你没有权限访问!');history.back();");

                    return;
                }
            }

        }
    }
}
