using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using Common.Library.Plugin;

namespace Common.Library.Mvc
{
    /// <summary>
    /// 自定义视图引擎
    /// </summary>
    public class PluginRazorViewEngine : RazorViewEngine {
        private static readonly string ModulesDirectoryName = ConfigurationManager.AppSettings["ModulesDirectoryName"];

        private readonly string[] _areaViewLocationFormats = {
            "~/{dir}/{module}/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/{dir}/{module}/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/{dir}/{module}/Views/{1}/{0}.cshtml",
            "~/{dir}/{module}/Views/Shared/{0}.cshtml",
            "~/{dir}/{2}/Views/{1}/{0}.cshtml",
            "~/{dir}/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Sites/{site}/Views/{1}/{0}.cshtml",
            "~/Sites/{site}/Views/Shared/{0}.cshtml",
            //20140813 以下条件ms在多个views文件夹中找到相同页cshtml时 下面 FindPartialView 出错 “索引(从零开始)必须大于或等于零,且小于参数列表的大小”
            "~/Sites/{module}/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Sites/{module}/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{module}/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{module}/Areas/{2}/Views/Shared/{0}.cshtml"
        };

        private readonly string[] _viewLocationFormats = {
            "~/{dir}/{module}/Views/{1}/{0}.cshtml",
            "~/{dir}/{module}/Views/Shared/{0}.cshtml",
            "~/Areas/{site}/Views/{1}/{0}.cshtml",
            "~/Areas/{site}/Views/Shared/{0}.cshtml",
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Sites/{site}/Views/{1}/{0}.cshtml",
            "~/Sites/{site}/Views/Shared/{0}.cshtml",
            //20140813 以下条件ms在多个views文件夹中找到相同页cshtml时 下面 FindPartialView 出错 “索引(从零开始)必须大于或等于零,且小于参数列表的大小”
            //"~/Sites/{module}/Areas/{2}/Views/{1}/{0}.cshtml",
            //"~/Sites/{module}/Areas/{2}/Views/Shared/{0}.cshtml"
                                                         };

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName,
            bool useCache) {
            var descriptor = PluginManager.GetDescriptor(controllerContext.Controller.GetType());
            if (descriptor != null) {
                UpdatePath(descriptor);
                CodeGeneration(descriptor);
            }

            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
            string masterName, bool useCache) {
            var descriptor = PluginManager.GetDescriptor(controllerContext.Controller.GetType());
            if (descriptor != null) {
                UpdatePath(descriptor);
                CodeGeneration(descriptor);
            }

            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        private void UpdatePath(PluginDescriptor descriptor) {
            Func<string, string> formatLocation =
                i =>
                    i.Replace("{dir}", ModulesDirectoryName)
                        .Replace("{module}", descriptor.Plugin.Name)
                        .Replace("{site}", descriptor.Plugin.Name);

            var areaViewLocationFormats = _areaViewLocationFormats.Select(formatLocation).ToArray();
            var viewLocationFormats = _viewLocationFormats.Select(formatLocation).ToArray();

            AreaViewLocationFormats = areaViewLocationFormats;
            AreaMasterLocationFormats = areaViewLocationFormats;
            AreaPartialViewLocationFormats = areaViewLocationFormats;
            ViewLocationFormats = viewLocationFormats;
            MasterLocationFormats = viewLocationFormats;
            PartialViewLocationFormats = viewLocationFormats;
        }

        private void CodeGeneration(PluginDescriptor descriptor) {
            RazorBuildProvider.CodeGenerationStarted += (sender, args) => {
                var provider = (RazorBuildProvider) sender;
                //var assemblies = BuildManager.GetReferencedAssemblies().OfType<Assembly>();
                //if (
                //    !assemblies.Any(
                //        i => i.FullName.Equals(descriptor.Assembly.FullName, StringComparison.CurrentCultureIgnoreCase)))
                    provider.AssemblyBuilder.AddAssemblyReference(descriptor.Assembly);
            };
        }
    }
}
