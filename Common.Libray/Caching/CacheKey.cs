using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    /// <summary>
    /// 缓存Key
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 根据type,和参数集合返回缓存key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string GetCacheKey(Type type, params string[] pars)
        {
            return MixCacheName(type.FullName, pars);
        }

        /// <summary>
        /// 泛型获取缓存key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string GetCacheKey<T>(params string[] pars)
        {
            return GetCacheKey(typeof(T), pars);
        }



        /// <summary>
        /// 组合缓存name
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string MixCacheName(string key, params object[] pars)
        {
            if (pars == null || pars.Length == 0)
                return key;

            key = key + "_" + string.Join("_", pars.Select((a, b) =>
            {
                return "{" + b + "}";
            }));

            return string.Format(key, pars);
        }
    }
}
