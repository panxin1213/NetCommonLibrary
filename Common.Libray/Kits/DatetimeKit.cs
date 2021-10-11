namespace ChinaBM.Common
{
    using System;

    public static class DatetimeKit
    {
        private static readonly string[] ChineseMonths = new string[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
        
        private static readonly string[] EnglishMonths = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        private static readonly string[] ChineseWeekDays = new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

        private static readonly string[] EnglishWeekDays = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        #region GetChineseMonth 根据阿拉伯数字返回月份的中文名称
        /// <summary>
        ///  根据阿拉伯数字返回月份的中文名称
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetChineseMonth(int month)
        {
            if (month < 0 || month >11)
            {
                return string.Empty;
            }
            return ChineseMonths[month];  
        }
        #endregion

        #region GetEnglishMonth 根据阿拉伯数字返回月份的英文名称
        /// <summary>
        ///  根据阿拉伯数字返回月份的英文名称
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetEnglishMonth(int month)
        {
            if (month < 0 || month > 11)
            {
                return string.Empty;
            }
            return EnglishMonths[month];
        }
        #endregion

        #region GetChineseWeekDay 根据阿拉伯数字返回中文WeekDay
        /// <summary>
        ///  根据阿拉伯数字返回中文WeekDay
        /// </summary>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public static string GetChineseWeekDay(int weekDay)
        {
            if (weekDay < 0 || weekDay > 6)
            {
                return string.Empty;
            }
            return ChineseWeekDays[weekDay];
        }
        #endregion

        #region GetEnglishWeekDay 根据阿拉伯数字返回英文WeekDay
        /// <summary>
        ///  根据阿拉伯数字返回英文WeekDay
        /// </summary>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public static string GetEnglishWeekDay(int weekDay)
        {
            if (weekDay < 0 || weekDay > 6)
            {
                return string.Empty;
            }
            return EnglishWeekDays[weekDay];
        }
        #endregion

        #region ToStandardDate 返回标准日期格式字符串
        /// <summary>
        ///  返回标准日期格式字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToStandardDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        #endregion

        #region ToStandardDate 返回标准日期格式字符串
        /// <summary>
        ///  返回指定日期格式
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="defaultDate">目标时间</param>
        /// <returns></returns>
        public static string ToStandardDate(object target, string defaultDate)
        {
            string resultString;
            if (target.Equals(null) || target.ToString().Equals(string.Empty))
            {
                return defaultDate;
            }
            try
            {
                resultString = Convert.ToDateTime(target).ToString("yyyy-MM-dd").Replace("1900-01-01", defaultDate);
            }
            catch
            {
                return defaultDate;
            }
            return resultString;
        }
        #endregion

        #region ToStandardTime 返回标准时间格式字符串
        /// <summary>
        ///  返回标准时间格式字符串
        /// </summary>
        /// <param name="dateTime">目标时间</param>
        /// <returns></returns>
        public static string ToStandardTime(DateTime dateTime)
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        #region ToStandardTime 返回标准时间格式字符串
        /// <summary>
        ///  返回标准时间格式字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static string ToStandardTime(object target, string defaultTime)
        {
            string resultString;
            if (target.Equals(null) || target.ToString().Equals(string.Empty))
            {
                return defaultTime;
            }
            try
            {
                resultString = Convert.ToDateTime(target).ToString("HH:mm:ss").Replace("00:00:00", defaultTime);
            }
            catch
            {
                return defaultTime;
            }
            return resultString;
        }
        #endregion

        #region ToStandardDateTime 返回标准日期时间格式字符串
        /// <summary>
        ///  返回标准日期时间格式字符串
        /// </summary>
        /// <param name="dateTime">目标时间</param>
        /// <returns></returns>
        public static string ToStandardDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region ToStandardDatetime 返回标准日期时间格式字符串
        /// <summary>
        ///  返回标准日期时间格式字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string ToStandardDatetime(object target, string defaultString)
        {
            string resultString;
            if (target.Equals(null) || target.ToString().Equals(string.Empty))
            {
                return defaultString;
            }
            try
            {
                resultString = Convert.ToDateTime(target).ToString("yyyy-MM-dd HH:mm:ss").Replace("1900-01-01 00:00:00", defaultString);
            }
            catch
            {
                return defaultString;
            }
            return resultString;
        }

        #endregion

        #region ToFullDatetime 返回完整的日期时间格式字符串
        /// <summary>
        ///  返回完整的日期时间格式字符串
        /// </summary>
        /// <param name="dateTime">目标时间</param>
        /// <returns></returns>
        public static string ToFullDatetime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }
        #endregion

        #region ConvertToUnixTimeStamp 转换时间为Unix时间戳
        /// <summary>
        /// 转换时间为Unix时间戳
        /// </summary>
        /// <param name="dateTime">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static double ConvertToUnixTimeStamp(DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan timeSpan = dateTime - origin;
            return Math.Floor(timeSpan.TotalSeconds);
        }
        #endregion
    }
}
