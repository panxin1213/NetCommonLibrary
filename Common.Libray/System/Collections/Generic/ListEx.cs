using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// list扩展
    /// </summary>
    public static class ListEx
    {
        /// <summary>
        /// 返回随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l"></param>
        /// <returns></returns>
        public static T Random<T>(this IList<T> l)
        {
            return l[new Random(Guid.NewGuid().GetHashCode()).Next(l.Count)];
        }
    }
}
