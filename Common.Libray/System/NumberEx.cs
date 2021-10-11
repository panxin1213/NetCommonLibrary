
namespace System
{
    /// <summary>
    /// 数九寒天相关扩展
    /// </summary>
    public static class NumberEx
    {
        /// <summary>
        /// 字符转Short
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static short ToShort(this string str, short defalut = 0)
        {
            short r;
            if (!Int16.TryParse(str.Trim(), out r))
            {
                r = defalut;
            }
            return r;
        }
        /// <summary>
        /// 字符转Int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static int ToInt(this string str, int defalut = 0)
        {
            if (String.IsNullOrEmpty(str))
            {
                return defalut;
            }

            int r;
            if (!Int32.TryParse(str.Trim(), out r))
            {
                r = defalut;
            }
            return r;
        }
        /// <summary>
        /// 字符转Long
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static long ToLong(this string str, long defalut = 0)
        {
            long r;
            if (!Int64.TryParse(str.Trim(), out r))
            {
                r = defalut;
            }
            return r;
        }
        /// <summary>
        /// 字符转Float
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static float ToFloat(this string str, float defalut = 0)
        {
            float r;
            if (!Single.TryParse(str.Trim(), out r))
            {
                r = defalut;
            }
            return r;
        }
        /// <summary>
        /// 字符转Double
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static double ToDouble(this string str, double defalut = 0.00)
        {
            double r;
            if (!Double.TryParse(str.Trim(), out r))
            {
                r = defalut;
            }
            return r;
        }
        /// <summary>
        /// 字符转Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str, decimal defalut = 0)
        {
            decimal r;
            if (!Decimal.TryParse(str.Trim(), out r))
            {
                r = defalut;
            }
            return r;
        }
        /// <summary>
        /// 数字转布尔
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool ToBool(this int i)
        {
            return i != 0;
        }
        /// <summary>
        /// 可空数字转布尔
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool ToBool(this int? i)
        {
            if (i.HasValue)
                return i.Value.ToBool();
            return false;
        }

    }
}
