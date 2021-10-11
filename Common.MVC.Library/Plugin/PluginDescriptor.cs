using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Common.Library.Plugin
{
    /// <summary>
    /// 插件描述类
    /// </summary>
    public class PluginDescriptor
    {
        private readonly IDictionary<string, Dictionary<string, Type>> _controllerTypes =
            new Dictionary<string, Dictionary<string, Type>>();
        private readonly Regex _regex = new Regex(@"Areas\.(\w+)\.Controllers$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public IPlugin Plugin { get; private set; }
        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="site"></param>
        /// <param name="assembly"></param>
        public PluginDescriptor(IPlugin site, Assembly assembly) {
            Plugin = site;
            Assembly = assembly;

            foreach (var type in assembly.GetTypes())
            {
                AddControllerType(type);
            }
        }

        private void AddControllerType(Type type) {
            if (type.GetInterface(typeof (IController).Name) != null && type.Name.Contains("Controller") && type.IsClass &&
                !type.IsAbstract) {
                if (type.Namespace != null) {
                    string key = type.Name.ToLower();
                    string areaKey;
                    if (_regex.IsMatch(type.Namespace)) {
                        areaKey = _regex.Match(type.Namespace).Groups[1].Value.ToLower();
                    }
                    else {
                        areaKey = string.IsNullOrEmpty(Plugin.AreaName) ? "www" : Plugin.AreaName.ToLower();
                    }
                    if (!_controllerTypes.ContainsKey(areaKey))
                        _controllerTypes.Add(areaKey, new Dictionary<string, Type>());
                    _controllerTypes[areaKey].Add(key, type);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAreas() {
            return _controllerTypes.Select(i => i.Key);
        }

        /// <summary>
        /// 根据区域名获取控制器类
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetControllerTypes(string area) {
            if (!string.IsNullOrEmpty(area))
            {
                var key = area.ToLower();
                if (_controllerTypes.ContainsKey(key))
                    return _controllerTypes[key].Select(i => i.Value);
            }
            return Enumerable.Empty<Type>();
        }

        /// <summary>
        /// 判断插件程序集中是否存在指定的控制器类型
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public bool HasControllerType(Type controllerType) {
            return _controllerTypes.Any(types => types.Value.Any(i => i.Value == controllerType));
        }
    }
}
