using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 可比较类型扩展
    /// </summary>
    public static class ComparableEx
    {
        /// <summary>
        /// 比较并交换（如果o1大于o2）
        /// 最小值放前面的参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static void CompareAndSwap<T>(ref T min, ref T max)
            where T : IComparable<T>
        {
            var compara = min as IComparable<T>;
            if (compara.CompareTo(max) > 0)
            {
                var t = max;
                max = min;
                min = t;
            }
        }
    }
}
