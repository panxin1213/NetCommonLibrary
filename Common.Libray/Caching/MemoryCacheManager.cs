using System;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using Common.Library.Log;

namespace Common.Library.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryCacheManager : BaseCacheManager
    {
        private ObjectCache Cache {
            get { return MemoryCache.Default; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T Get<T>(string cacheKey, Func<T> func) {
            return Get<T>(cacheKey, func, null);
        }

        public override T Get<T>(string cacheKey, Func<T> func, int? time = null) {
            cacheKey = GetFullCacheKey(cacheKey);
            var cacheObject = Cache[cacheKey] as T;
            if (cacheObject == null) {
                try {
                    cacheObject = func();
                    if (cacheObject != null) {
                        Cache.Add(cacheKey, cacheObject,time.HasValue&&time.Value>0?(DateTime.Now+TimeSpan.FromMilliseconds(time.Value)): ObjectCache.InfiniteAbsoluteExpiration);
                        Logger.Debug(this,
                            string.Format("New caching item created: key={0}, value={1}.", cacheKey, cacheObject));
                    }
                } catch (Exception exception) {
                    Logger.Error(this, exception.Message, exception);
                    return null;
                }
            }
            return cacheObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKeys"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Remove(params string[] cacheKeys) {
            if (cacheKeys == null || cacheKeys.Length == 0) return;
            foreach (var cacheKey in cacheKeys) {
                string fullCacheKey = GetFullCacheKey(cacheKey);
                Cache.Remove(fullCacheKey);
                Logger.Debug(this, string.Format("Cache item {0} deleted.", fullCacheKey));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKeyPattern"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void RemoveByPattern(string cacheKeyPattern) {
            var regex = new Regex(cacheKeyPattern,
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var list = (from pair in Cache where regex.IsMatch(pair.Key) select pair.Key).ToList();
            foreach (var key in list) {
                Cache.Remove(key);
                Logger.Debug(this, string.Format("Cache item {0} deleted.", key));
            }
        }


        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public override void RemoveAll()
        {
            foreach (var item in Cache.Where(a => a.Key.StartsWith(CacheKeyPrefix)))
            {
                Cache.Remove(item.Key);
            }
        }
    }
}
