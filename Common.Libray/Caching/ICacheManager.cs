using System;

namespace Common.Library.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICacheManager {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string cacheKey, Func<T> func) where T : class;

        T Get<T>(string cacheKey, Func<T> func,int ?time=null) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKeys"></param>
        void Remove(params string[] cacheKeys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKeyPattern"></param>
        void RemoveByPattern(string cacheKeyPattern);

        /// <summary>
        /// 移除全部
        /// </summary>
        void RemoveAll();
    }
}
