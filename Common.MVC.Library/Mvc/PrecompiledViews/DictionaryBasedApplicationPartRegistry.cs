using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.WebPages;

namespace Common.MVC.Library.Mvc.PrecompiledViews
{
    public class DictionaryBasedApplicationPartRegistry : IApplicationPartRegistry
    {
        private static readonly Type webPageType = typeof(WebPageRenderingBase);
        private readonly Dictionary<string, Type> registeredPaths = new Dictionary<string, Type>();
        public virtual Type GetCompiledType(string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }
            if (virtualPath.StartsWith("/"))
            {
                virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            }
            if (!virtualPath.StartsWith("~"))
            {
                virtualPath = ((!virtualPath.StartsWith("/")) ? ("~/" + virtualPath) : ("~" + virtualPath));
            }
            virtualPath = virtualPath.ToLower();
            if (!this.registeredPaths.ContainsKey(virtualPath))
            {
                return null;
            }
            return this.registeredPaths[virtualPath];
        }
        public void Register(Assembly applicationPart)
        {
            ((IApplicationPartRegistry)this).Register(applicationPart, null);
        }
        public virtual void Register(Assembly applicationPart, string rootVirtualPath)
        {
            foreach (Type current in
                from type in applicationPart.GetTypes()
                where type.IsSubclassOf(DictionaryBasedApplicationPartRegistry.webPageType)
                select type)
            {
                ((IApplicationPartRegistry)this).RegisterWebPage(current, rootVirtualPath);
            }
        }
        public void RegisterWebPage(Type type)
        {
            ((IApplicationPartRegistry)this).RegisterWebPage(type, string.Empty);
        }
        public virtual void RegisterWebPage(Type type, string rootVirtualPath)
        {
            PageVirtualPathAttribute pageVirtualPathAttribute = type.GetCustomAttributes(typeof(PageVirtualPathAttribute), false).Cast<PageVirtualPathAttribute>().SingleOrDefault<PageVirtualPathAttribute>();
            if (pageVirtualPathAttribute != null)
            {
                string rootRelativeVirtualPath = DictionaryBasedApplicationPartRegistry.GetRootRelativeVirtualPath(rootVirtualPath ?? "", pageVirtualPathAttribute.VirtualPath);
                this.registeredPaths[rootRelativeVirtualPath.ToLower()] = type;
            }
        }
        private static string GetRootRelativeVirtualPath(string rootVirtualPath, string pageVirtualPath)
        {
            string text = pageVirtualPath;
            if (text.StartsWith("~/", StringComparison.Ordinal))
            {
                text = text.Substring(2);
            }
            if (!rootVirtualPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                rootVirtualPath += "/";
            }
            text = VirtualPathUtility.Combine(rootVirtualPath, text);
            if (text.StartsWith("~"))
            {
                return text;
            }
            if (text.StartsWith("/"))
            {
                return "~" + text;
            }
            return "~/" + text;
        }
    }
}
