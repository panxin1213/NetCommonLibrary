using System;
using System.Web.Hosting;

namespace Common.Library.Caching {
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseCacheManager : ICacheManager {
        protected const string CacheKeyPrefix = "Elysian_";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        protected string GetFullCacheKey(string cacheKey) {
            return string.Concat(CacheKeyPrefix, HostingEnvironment.SiteName, "_", cacheKey);
        }

        public abstract T Get<T>(string cacheKey, Func<T> func) where T : class;
        public abstract T Get<T>(string cacheKey, Func<T> func, int? time = null) where T : class;
        public abstract void Remove(params string[] cacheKeys);
        public abstract void RemoveByPattern(string cacheKeyPattern);
        public abstract void RemoveAll();
    }
}
