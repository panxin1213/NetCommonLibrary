using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Text
{
    /// <summary>
    /// StringBuilder 扩展
    /// </summary>
    public static class StringBuilderEx
    {
        /// <summary>
        /// 如果condition 有值 则添加
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="append"></param>
        public static void AppendIfHasValue(this StringBuilder sb, DateTime? value, string append)
        {
            if (value.HasValue)
            {
                sb.Append(append);
            }
        }
       
        /// <summary>
        /// 如果condition 有值 则添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="append"></param>
        public static void AppendIfHasValue<T>(this StringBuilder sb, T? value, string append) where T : struct
        {
            if (value.HasValue)
            {
                sb.Append(append);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="append"></param>
        public static void AppendIfHasValue(this StringBuilder sb, IEnumerable<string> value, string append)
        {
            if (value != null && value.Any())
            {
                sb.Append(append);
            }
        }
        /// <summary>
        /// 数组 in 查询，大于0才有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="append"></param>
        public static void AppendIfHasValue<T>(this StringBuilder sb, IEnumerable<T> value, string append) where T: IComparable
        {

            if (value != null && value.Any(a => a != null && a.CompareTo(0) > 0))
            {
                sb.Append(append);
            }
        }
        
        /// <summary>
        /// 如果condition 有值 则添加
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <param name="append"></param>
        public static void AppendIfHasValue(this StringBuilder sb, string value, string append)
        {
            if (!string.IsNullOrEmpty(value))
            {
                sb.Append(append);
            }
        }

        /// <summary>
        /// 如果condition=true 则添加
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="condition"></param>
        /// <param name="append"></param>
        public static void AppendIf(this StringBuilder sb, bool condition, string append)
        {
            if (condition)
            {
                sb.Append(append);
            }
        }
        /// <summary>
        /// 如果condition=true 则添加
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="condition"></param>
        /// <param name="append"></param>
        /// <param name="args"></param>
        public static void AppendFormatIf(this StringBuilder sb, bool condition, string append,params object[] args) 
        {
            if (condition)
            {
                sb.AppendFormat(append,args);
            }
        }


   
    }
}
