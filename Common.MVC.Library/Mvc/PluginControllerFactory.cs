using System.Web.Routing;
using Common.Library.Plugin;

namespace System.Web.Mvc
{
    /// <summary>
    /// 自定义控制器工厂
    /// </summary>
    public class PluginControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(RequestContext requestContext, string controllerName) {
            var area = requestContext.RouteData.Values["area"];
            if (area == null) {
                if (!requestContext.RouteData.DataTokens.TryGetValue("area", out area)) {
                    return null;
                }
            }
            return PluginManager.GetControllerType(controllerName, area.ToString());
        }
    }
}
