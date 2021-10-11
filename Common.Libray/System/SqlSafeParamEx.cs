using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
namespace System.Text
{
    /// <summary>
    /// 
    /// </summary>
    public static class  SqlSafeParamEx
    {
        #region 字符串扩展
        private static readonly Regex Reg_SafeNum = new Regex(@"\D+", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex Reg_SafeNums = new Regex(@"[^\d,]+", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex Reg_SafeNums_Format = new Regex(@"^,+|,+$|(,),+", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// 返回安全的数字 以用于SQL查询字符串构造
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSqlSafeNum(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "0";
            string r = Reg_SafeNum.Replace(s, "");
            //System.Web.HttpContext.Current.Response.Write("dddd"+string.IsNullOrWhiteSpace(r));
            if (string.IsNullOrWhiteSpace(r)) return "0";
            return r;
        }
        /// <summary>
        /// 返回安全的数字组合 以用于SQL查询字符串构造 1,324asdf,23 => 1,324,23 ,sdfkjsdlkfj =>0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSqlSafeNums(this String s)
        {
            if (String.IsNullOrWhiteSpace(s)) return "0";
            string r = Reg_SafeNums_Format.Replace(Reg_SafeNums.Replace(s, ""), "$1");
            if (string.IsNullOrWhiteSpace(r)) return "0";
            return r;
        }
        /// <summary>
        /// 返回安全的字符 以用于SQL查询字符串构造
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSqlSafeStr(this String s)
        {
            return (s ?? "").Replace("'", "''");
        }
        #endregion


        #region 数组扩展
        /// <summary>
        /// 返回安全的SQL字符串组 以用于SQL查询字符串构造 aaa'sd,sdfsdf => aaa''sd,sdfsdf
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ToSqlSafeStrs(this IEnumerable<string> arr)
        {
            if (arr == null) return "''";
            var r = arr.Where(a=>a!=null).Select(a=>a.ToSqlSafeStr());
            return String.Join("','", r);
        }

        /// <summary>
        /// 返回安全的SQL数字组 以用于SQL查询字符串构造
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ToSqlSafeNums(this IEnumerable<string> arr)
        {
            
            if (arr == null) return "0";

            var r = arr.Where(a => a != null).Select(a =>
            {
                
                long x;
                if (Int64.TryParse(a, out x))
                {
                    return x;
                }
                return 0;
            });

            return String.Join(",", r);
        }
        /// <summary>
        /// 返回安全的SQL数字组 以用于SQL查询字符串构造
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ToSqlSafeNums(this IEnumerable<int> arr)
        {
            if (arr == null) return "0";
            if (!arr.Any()) arr = new int[] { 0 };
            return String.Join(",", arr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ToSqlSafeNums(this IEnumerable<int?> arr)
        {
            if (arr == null) return "0";
            if (!arr.Any()) arr = new int?[] { 0 };
            return String.Join(",", arr);
        }
        /// <summary>
        /// 返回安全的SQL数字组 以用于SQL查询字符串构造
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ToSqlSafeNums(this IEnumerable<long> arr)
        {
            if (arr == null) return "0";
            if (!arr.Any()) arr = new long[] { 0 };
            return String.Join(",", arr);
        }
        #endregion
    }
}
