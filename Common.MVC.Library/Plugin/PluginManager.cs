using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Library.Log;

namespace Common.Library.Plugin
{
    /// <summary>
    /// 插件管理类
    /// </summary>
    public static class PluginManager {
        internal static readonly DirectoryInfo ModulesDirectory;
        internal static readonly DirectoryInfo SitesDirectory;
        internal static readonly DirectoryInfo WebRootBinDirectory;

        private static readonly IDictionary<string, PluginDescriptor> PluginDescriptors =
            new ConcurrentDictionary<string, PluginDescriptor>();
        private static readonly IDictionary<string, ConcurrentDictionary<string, Type>> ControllerTypes =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, Type>>();

        // 插件类型判断
        private static readonly Func<Type, bool> IsPluginPredicate = i => typeof(IPlugin).IsAssignableFrom(i) && i.IsClass && !i.IsAbstract;
        private static List<Assembly> _assemblies = new List<Assembly>();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static PluginManager() {
            var modulesDirectoryPath =
                HostingEnvironment.MapPath(string.Concat("~/", ConfigurationManager.AppSettings["ModulesDirectoryName"]));
            var sitesDirectoryPath =
                HostingEnvironment.MapPath(string.Concat("~/", ConfigurationManager.AppSettings["SitesDirectoryName"]));
            if (modulesDirectoryPath != null) ModulesDirectory = new DirectoryInfo(modulesDirectoryPath);
            if (sitesDirectoryPath != null) SitesDirectory = new DirectoryInfo(sitesDirectoryPath);
            WebRootBinDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/bin"));
        }

        /// <summary>
        /// 插件初始化
        /// </summary>
        public static void Initialize() {
#if DEBUG
            //CopyPluginAssembliesToBin();
#endif
            LoadAssemblies();

            RegisterDefaultRoutes();
            InitializeControllerTypes();
        }

        /// <summary>
        /// 注册默认路由
        /// </summary>
        private static void RegisterDefaultRoutes() {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
                , new {id=@"\d*" }
            );
        }

        /// <summary>
        /// 删除默认路由
        /// </summary>
        private static void RemoveDefaultRoutes() {
            RouteTable.Routes.Remove(RouteTable.Routes["Default"]);
        }

        private static void CopyPluginAssembliesToBin() {
            var pluginDirectories = new[] {ModulesDirectory, SitesDirectory};

            foreach (var pluginDirectory in pluginDirectories) {
                var binaryDirectories = pluginDirectory.EnumerateDirectories("bin", SearchOption.AllDirectories);
                foreach (var binaryDirectory in binaryDirectories) {
                    var assemblyFiles = binaryDirectory.EnumerateFiles("*.dll", SearchOption.AllDirectories);
                    foreach (var assemblyFile in assemblyFiles) {
                        var destinationPath = Path.Combine(WebRootBinDirectory.FullName, assemblyFile.Name);
                        if (File.Exists(destinationPath)) {
                            if (File.GetLastWriteTimeUtc(destinationPath).Ticks < File.GetLastWriteTimeUtc(assemblyFile.FullName).Ticks) {
                                File.Copy(assemblyFile.FullName, destinationPath, true);
                            }
                        }
                        else {
                            File.Copy(assemblyFile.FullName, destinationPath);
                        }
                    }
                    // 复制.pdb文件到bin目录，调试使用
                    var pdbFiles = binaryDirectory.EnumerateFiles("*.pdb", SearchOption.AllDirectories);
                    foreach (var pdbFile in pdbFiles) {
                        var destinationPath = Path.Combine(WebRootBinDirectory.FullName, pdbFile.Name);
                        if (File.Exists(destinationPath)) {
                            if (File.GetLastWriteTimeUtc(destinationPath).Ticks < File.GetLastWriteTimeUtc(pdbFile.FullName).Ticks) {
                                File.Copy(pdbFile.FullName, destinationPath, true);
                            }
                        }
                        else {
                            File.Copy(pdbFile.FullName, destinationPath);
                        }
                    }
                }
            }
        }

        private static void LoadAssemblies() {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (var file in WebRootBinDirectory.EnumerateFiles("*.dll")) {
                var assemblyName = AssemblyName.GetAssemblyName(file.FullName);
                if (!_assemblies.Any(i => i.FullName.Equals(assemblyName.FullName)))
                    _assemblies.Add(Assembly.Load(assemblyName));
            }

//#if DEBUG
//            var pluginDirectories = new[] {ModulesDirectory, SitesDirectory};

//            foreach (var pluginDirectory in pluginDirectories) {
//                try {
//                    var binaryDirectories = pluginDirectory.EnumerateDirectories("bin", SearchOption.AllDirectories);
//                    foreach (var binaryDirectory in binaryDirectories) {
//                        var assemblyFiles = binaryDirectory.EnumerateFiles("*.dll", SearchOption.AllDirectories);
//                        _assemblies.AddRange(assemblyFiles.Select(i => Assembly.LoadFile(i.FullName)));
//                    }
//                }
//                catch (ReflectionTypeLoadException exception) {
//                    Logger.Error(typeof (PluginManager),
//                        string.Concat(exception.Message, ". ",
//                            string.Join(";", exception.LoaderExceptions.Select(i => i.Message))), exception);
//                }
//            }
//#endif

            LoadDescriptors();
        }

        private static void LoadDescriptors() {
            try {
                var pluginAssemblies = _assemblies.Where(i => 
                    {
                        if (_assemblies.IndexOf(i) == 30)
                        {

                        }
                        var r = i.GetTypes().Any(IsPluginPredicate);
                        return r;
                    }).ToList();
                var moduleAssemblies =
                    pluginAssemblies.Where(
                        i => typeof (IModule).IsAssignableFrom(i.GetTypes().Single(IsPluginPredicate)));
                foreach (var assembly in moduleAssemblies) {
                    LoadDescriptor(assembly);
                }
                var siteAssemblies =
                    pluginAssemblies.Where(i => typeof (ISite).IsAssignableFrom(i.GetTypes().Single(IsPluginPredicate)));
                foreach (var assembly in siteAssemblies) {
                    LoadDescriptor(assembly);
                }
            }
            catch (ReflectionTypeLoadException exception) {
                Logger.Error(typeof (PluginManager),
                    string.Concat(exception.Message + " ass_count:" + _assemblies.Count, ". ",
                        string.Join(";", exception.LoaderExceptions.Select(i => i.Message))), exception);
                throw;
            }
        }

        private static void LoadDescriptor(Assembly pluginAssembly) {
            var pluginType = pluginAssembly.GetTypes().SingleOrDefault(IsPluginPredicate);
            if (pluginType != null) {
                var descriptor = LoadDescriptor(pluginType);
                if (descriptor != null && !PluginDescriptors.ContainsKey(descriptor.Plugin.Name)) {
                    Unload(descriptor);
                    Initialize(descriptor);
                }
            }
        }

        /// <summary>
        /// 返回插件描述对象
        /// </summary>
        /// <param name="pluginType"></param>
        /// <returns></returns>
        private static PluginDescriptor LoadDescriptor(Type pluginType) {
            var plugin = (IPlugin)Activator.CreateInstance(pluginType);

            return new PluginDescriptor(plugin, plugin.GetType().Assembly);
        }

        /// <summary>
        /// 单个模块初始化方法
        /// </summary>
        /// <param name="pluginDescriptor"></param>
        public static void Initialize(PluginDescriptor pluginDescriptor)
        {
            string key = pluginDescriptor.Plugin.Name;
            if (!string.IsNullOrEmpty(key) && !PluginDescriptors.ContainsKey(key))
            {
                pluginDescriptor.Plugin.Initialize();
                PluginDescriptors.Add(key, pluginDescriptor);
            }
        }

        /// <summary>
        /// 模块卸载方法
        /// </summary>
        public static void Unload()
        {
            foreach (var descriptor in GetDescriptors())
            {
                descriptor.Plugin.Unload();
            }

            PluginDescriptors.Clear();

            RemoveDefaultRoutes();
        }

        /// <summary>
        /// 单个模块卸载方法
        /// </summary>
        /// <param name="pluginDescriptor"></param>
        public static void Unload(PluginDescriptor pluginDescriptor)
        {
            pluginDescriptor.Plugin.Unload();
            PluginDescriptors.Remove(pluginDescriptor.Plugin.Name);
        }

        /// <summary>
        /// 获取所有的插件描述信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PluginDescriptor> GetDescriptors()
        {
            return PluginDescriptors.Select(i => i.Value);
        }

        /// <summary>
        /// 获取所有的插件程序集
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssemblies()
        {
            return PluginDescriptors.Select(i => i.Value.Assembly);
        }

        /// <summary>
        /// 根据指定的控制器类型获取插件的描述对象
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        internal static PluginDescriptor GetDescriptor(Type controllerType)
        {
            return PluginDescriptors.Select(i => i.Value).SingleOrDefault(i => i.HasControllerType(controllerType));
        }

        /// <summary>
        /// 根据指定的插件名称获取插件的描述对象
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        internal static PluginDescriptor GetDescriptor(string pluginName) {
            return
                PluginDescriptors.Select(i => i.Value)
                    .SingleOrDefault(i => i.Plugin.Name.Equals(pluginName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// 根据控制器名称和区域名获取控制器类型
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        internal static Type GetControllerType(string controllerName, string area) {
            var areaKey = area.ToLower();
            if (ControllerTypes.ContainsKey(areaKey))
            {
                var types = ControllerTypes[areaKey];
                var typeKey = string.Concat(controllerName, "Controller").ToLower();
                if (types.ContainsKey(typeKey))
                    return types[typeKey];
            }
            return null;
        }

        /// <summary>
        /// 根据区域名称获取控制器类型
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static IDictionary<string, Type> GetControllerTypes(string area) {
            var areaKey = area.ToLower();
            if (ControllerTypes.ContainsKey(areaKey)) {
                return ControllerTypes[areaKey];
            }
            return null;
        }

        /// <summary>
        /// 将所有插件的控制器类型保存到字典中
        /// </summary>
        private static void InitializeControllerTypes() {
            if (PluginDescriptors.Count > 0)
            {
                // 由于一个Area分布在多个程序集里面，所以需要循环程序集取出每个Area里面的控制器类
                var areas = PluginDescriptors.Select(i => i.Value.GetAreas()).Aggregate((a, b) => a.Union(b)).ToArray();
                foreach (string area in areas)
                {
                    if (!ControllerTypes.ContainsKey(area))
                        ControllerTypes.Add(area, new ConcurrentDictionary<string, Type>());

                    foreach (var descriptor in PluginDescriptors)
                    {
                        var types = descriptor.Value.GetControllerTypes(area);
                        foreach (var type in types)
                        {
                            ControllerTypes[area].TryAdd(type.Name.ToLower(), type);
                        }
                    }
                }
            }
        }
    }
}
