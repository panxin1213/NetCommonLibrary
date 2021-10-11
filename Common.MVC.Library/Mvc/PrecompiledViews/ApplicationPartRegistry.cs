using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.MVC.Library.Mvc.PrecompiledViews
{
    public static class ApplicationPartRegistry
    {
        public static IApplicationPartRegistry Instance
        {
            get;
            set;
        }
        static ApplicationPartRegistry()
        {
            ApplicationPartRegistry.Instance = new DictionaryBasedApplicationPartRegistry();
        }
        public static Type GetCompiledType(string virtualPath)
        {
            return ApplicationPartRegistry.Instance.GetCompiledType(virtualPath);
        }
        public static void Register(Assembly applicationPart)
        {
            ApplicationPartRegistry.Register(applicationPart, null);
        }
        public static void Register(Assembly applicationPart, string rootVirtualPath)
        {
            ApplicationPartRegistry.Instance.Register(applicationPart, rootVirtualPath);
        }
        public static void RegisterWebPage(Type type)
        {
            ApplicationPartRegistry.RegisterWebPage(type, null);
        }
        public static void RegisterWebPage(Type type, string rootVirtualPath)
        {
            ApplicationPartRegistry.Instance.RegisterWebPage(type, rootVirtualPath);
        }
    }
}
