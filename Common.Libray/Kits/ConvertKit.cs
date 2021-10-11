namespace ChinaBM.Common
{
    using System;

    /// <summary>
    ///  数据类型工具
    /// </summary>
    public static class ConvertKit
    {
        #region Convert 类型转换
        /// <summary>
        ///  类型转换
        /// </summary>
        /// <typeparam name="TValue">目标类型</typeparam>
        /// <param name="value">被转换的值</param>
        /// <param name="defaultValue">转换失败时返回默认值</param>
        /// <returns></returns>
        public static TValue Convert<TValue>(object value, TValue defaultValue) 
        {
            TValue newValue;
            try
            {
                if (value != null && value is TValue)
                {
                    newValue = (TValue)value;
                }
                else
                {
                    if (typeof(TValue) == typeof(DateTime?))
                    {
                        value = System.Convert.ToDateTime(value);
                    }
                    else if (typeof(TValue) == typeof(bool?))
                    {
                        value = System.Convert.ToBoolean(value);
                    }
                    else if (typeof(TValue) == typeof(int?))
                    {
                        value = System.Convert.ToInt32(value);
                    }
                    else
                    {
                        value = System.Convert.ChangeType(value, typeof(TValue));
                    }
                    newValue = (TValue)value;
                    if (newValue == null)
                    {
                        return defaultValue;
                    }
                }
            }
            catch (Exception)
            {
                newValue = defaultValue;
            }
            return newValue;  
        }

        #endregion

        #region Convert Convert 类型转换(Nullable)
        /// <summary>
        ///  类型转换(Nullable)
        /// </summary>
        /// <typeparam name="TValue">目标类型</typeparam>
        /// <param name="value">被转换的值</param>
        /// <param name="defaultValue">转换失败时返回默认值</param>
        /// <returns></returns>
        public static TValue? Convert<TValue>(object value, TValue? defaultValue) where TValue : struct 
        {   
            TValue? newValue;
            try
            {
                if (value != null && value is TValue)
                {
                    newValue = (TValue)value;
                }
                else
                {
                    value = System.Convert.ChangeType(value, typeof(TValue));
                    newValue = (TValue)value;
                }
            }
            catch (Exception)
            {
                newValue = defaultValue;
            }
            return newValue;  
        }
        #endregion

        #region LongToInt32 将long型数值转换为Int32类型
        /// <summary>
        /// 将long型数值转换为Int32类型
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int SafeInt32(object target)
        {
            if (target == null)
            {
                return 0;
            }
            string numString = target.ToString();
            if (ValidateKit.IsNumeric(numString))
            {
                if (numString.Length > 9)
                {
                    if (numString.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    return int.MaxValue;
                }
                return Int32.Parse(numString);
            }
            return 0;
        }
        #endregion
        
    }
}
