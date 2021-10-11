using System;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Common.Library.Log;
using System.Linq;
using System.Collections.Generic;

namespace Common.Library.Caching
{
    /// <summary>
    /// A simple wrapper for System.Web.Caching.Cache
    /// </summary>
    public class CacheManager
    {
        private const string CacheKeyPrefix = "Elysian_";

        private static Cache Cache
        {
            get
            {
                if (HttpContext.Current == null)
                    return HttpRuntime.Cache;
                return HttpContext.Current.Cache;
            }
        }

        /// <summary>
        /// 根据键名从缓存中获取内容
        /// </summary>
        /// <param name="cacheKey">缓存键名</param>
        /// <param name="func">如果缓存不存在，用于生成缓存对象的方法</param>
        /// <returns></returns>
        public static object Get(string cacheKey, Func<object> func)
        {
            return Get(cacheKey, func, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(string cacheKey, Func<T> func) where T : class
        {
            //return Get(cacheKey, func, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
            return Get(cacheKey, func, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);//暂时关闭缓存
        }

        /// <summary>
        /// 根据键名从缓存中获取内容
        /// </summary>
        /// <param name="cacheKey">缓存键名</param>
        /// <param name="func">如果缓存不存在，用于生成缓存对象的方法</param>
        /// <param name="dependency">缓存依赖项</param>
        /// <returns></returns>
        public static object Get(string cacheKey, Func<object> func, CacheDependency dependency)
        {
            return Get(cacheKey, func, dependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 根据键名从缓存中获取内容
        /// </summary>
        /// <param name="cacheKey">缓存键名</param>
        /// <param name="func">如果缓存不存在，用于生成缓存对象的方法</param>
        /// <param name="dependency">缓存依赖项</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="slidingExpiration">相对过期时间</param>
        /// <returns></returns>
        public static object Get(string cacheKey, Func<object> func, CacheDependency dependency, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (func == null)
                return null;
            cacheKey = GetFullCacheKey(cacheKey);
            object cacheContent = Cache.Get(cacheKey);
            if (cacheContent == null)
            {
                try
                {
                    cacheContent = func();
                    if (cacheContent != null)
                    {
                        Cache.Insert(cacheKey, cacheContent, dependency, absoluteExpiration, slidingExpiration);
                        Logger.Debug(typeof(CacheManager),
                            string.Format("New Caching Item: key={0}, value={1}", cacheKey, cacheContent));
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(typeof(CacheManager), exception.Message, exception);
                    return null;
                }
            }
            return cacheContent;
        }
        /// <summary>
        /// 泛型缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <param name="dependency"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public static T Get<T>(string cacheKey, Func<T> func, CacheDependency dependency, DateTime absoluteExpiration, TimeSpan slidingExpiration) where T : class
        {
            if (func == null)
                return default(T);
            cacheKey = GetFullCacheKey(cacheKey);
            var cacheContent = (T)Cache.Get(cacheKey);
            if (cacheContent == null)
            {
                try
                {
                    cacheContent = func();
                    if (cacheContent != null)
                    {
                        Cache.Insert(cacheKey, cacheContent, dependency, absoluteExpiration, slidingExpiration);
                        Logger.Debug(typeof (CacheManager),
                            string.Format("New Caching Item: key={0}, value={1}", cacheKey, cacheContent));
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(typeof(CacheManager), exception.Message, exception);
                    return default(T);
                }
            }
            return cacheContent;
        }

        public static void Remove(string cacheKey)
        {
            cacheKey = GetFullCacheKey(cacheKey);
            Cache.Remove(cacheKey);
        }

        /// <summary>
        /// 删除全部当前站的缓存
        /// </summary>
        public static void RemoveAll()
        {
            RemoveByPrefix(null);
        }

        /// <summary>
        /// 删除本站已参数的字符串开头的缓存
        /// </summary>
        /// <param name="prefix"></param>
        public static void RemoveByPrefix(string prefix)
        {
            var cacheKey = String.IsNullOrEmpty(prefix) ? CacheKeyPrefix : GetFullCacheKey(prefix);

            foreach (var item in Cache.Cast<System.Collections.DictionaryEntry>().Where(a => a.Key.ToSafeString().StartsWith(cacheKey)).Select(a => a.Key))
            {
                Cache.Remove(item.ToSafeString());
            }
        }

        /// <summary>
        /// 删除本站该类型的缓存
        /// </summary>
        /// <param name="type"></param>
        public static void RemoveByType(IEnumerable<Type> types)
        {
            if (types != null)
            {
                foreach (var type in types)
                    RemoveByPrefix(type.FullName);
            }
        }


        private static string GetFullCacheKey(string cacheKey)
        {
            return string.Concat(CacheKeyPrefix, HostingEnvironment.SiteName, "_", cacheKey);
        }
    }
}
