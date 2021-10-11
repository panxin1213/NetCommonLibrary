using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Collections.Generic
{
    /// <summary>
    /// Enumerable扩展
    /// </summary>
    public static class EnumerableEx
    {
        /// <summary>
        /// IEnumerable安全转义，不报错
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TTarget> SafeCast<TTarget>(this IEnumerable source)
        {
            return source == null ? null : source.Cast<TTarget>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T SafeGet<T>(this IEnumerable<T> source, int index)
        {
            var x = source.Count();
            if (x==0 || index > x-1 )
            {
                return default(T);
            }
            return source.ElementAt(index);
        }
    }
    
}
