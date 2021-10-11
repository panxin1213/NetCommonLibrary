using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public enum DateDiffType
    {
        /// <summary>
        /// 年
        /// </summary>
        Year,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 日
        /// </summary>
        Day,
        /// <summary>
        /// 周
        /// </summary>
        Week,
        /// <summary>
        /// 小时
        /// </summary>
        Hour,
        /// <summary>
        /// 分
        /// </summary>
        Minute,
        /// <summary>
        /// 秒
        /// </summary>
        Second,
        /// <summary>
        /// 毫秒
        /// </summary>
        Millisecond
    }

    /// <summary>
    /// DateTime.Datediffs返回此对象
    /// </summary>
    public class DateDiff
    {
        /// <summary>
        /// 年 共+-多少
        /// </summary>
        public int TotalYears
        {
            get;
            internal set;
        }
        /// <summary>
        /// 月 共+-多少
        /// </summary>
        public int TotalMonths
        {
            get;
            internal set;
        }
        /// <summary>
        /// 周  共+-多少
        /// </summary>
        public int TotalWeeks
        {
            get;
            internal set;
        }
        /// <summary>
        /// 天 共+-多少
        /// </summary>
        public int TotalDays
        {
            get;
            internal set;
        }
        /// <summary>
        /// 小时 共+-多少
        /// </summary>
        public int TotalHours
        {
            get;
            internal set;
        }
        /// <summary>
        /// 分钟 共+-多少
        /// </summary>
        public long TotalMinutes
        {
            get;
            internal set;
        }
        /// <summary>
        /// 秒 共+-多少
        /// </summary>
        public long TotalSeconds
        {
            get;
            internal set;
        }
        /// <summary>
        /// 毫秒 共+-多少
        /// </summary>
        public long TotalMilliseconds
        {
            get;
            internal set;
        }


        /// <summary>
        /// 差多少年 倒计时/已用时 小时
        /// </summary>
        public int Year
        {
            get;

            internal set;
        }
        /// <summary>
        /// 暂未实现 倒计时/已用时 月
        /// </summary>
        public int Month
        {
            get;
            internal set;
        }
        /// <summary>
        /// 暂未实现 倒计时/已用时 天
        /// </summary>
        public int Day
        {
            get;
            internal set;
        }
        /// <summary>
        /// 暂未实现 倒计时/已用时 小时
        /// </summary>
        public int Hour
        {
            get;
            internal set;
        }
        /// <summary>
        /// 暂未实现 倒计时/已用时 分
        /// </summary>
        public long Minute
        {
            get;
            internal set;
        }
        /// <summary>
        /// 暂未实现 倒计时/已用时 秒
        /// </summary>
        public long Second
        {
            get;
            internal set;
        }
        /// <summary>
        /// 暂未实现 倒计时/已用时 毫秒
        /// </summary>
        public long Millisecond
        {
            get;
            internal set;
        }
    }
    /// <summary>
    /// DateTime重写
    /// </summary>
    public static class DateTimeEx
    {
        /*
        /// <summary>
        /// 返回时间差
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static TimeSpan DateDiff(this DateTime DateTime1, DateTime DateTime2)
        {

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            return ts1.Subtract(ts2);
        }*/
        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="begin">开始时间</param>
        /// <param name="type">要返回的时间差类型</param>
        /// <param name="end">结束时间，不指定则结束时间为DateTime.Now</param>
        /// <returns>如果end大于begin,则返回负数</returns>
        public static long DateDiff(this DateTime begin, DateDiffType type, DateTime? end = null)
        {
            DateTime endDateTime;
            if (!end.HasValue)
                endDateTime = DateTime.Now;
            else
                endDateTime = end.Value;

            switch (type)
            {
                case DateDiffType.Year:
                    return (begin.Year - endDateTime.Year);
                case DateDiffType.Month:
                    return (begin.Month - endDateTime.Month) + (12 * (begin.Year - endDateTime.Year));
                case DateDiffType.Week:
                    return (long)Math.Floor((begin - endDateTime).TotalDays) / 7;
                case DateDiffType.Day:
                    return (long)(begin - endDateTime).TotalDays;
                case DateDiffType.Hour:
                    return (long)(begin - endDateTime).TotalHours;
                case DateDiffType.Minute:
                    return (long)(begin - endDateTime).TotalMinutes;
                case DateDiffType.Second:
                    return (long)(begin - endDateTime).TotalSeconds;
                case DateDiffType.Millisecond:
                    return (long)(begin - endDateTime).TotalMilliseconds;
            }
            return 0;
        }
        /// <summary>
        /// 返回 详细DateDiff
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns>如果end大于begin,则返回负数</returns>
        public static DateDiff DateDiffs(this DateTime begin, DateTime end)
        {

            var r = new DateDiff();
            TimeSpan span = begin - end;

            r.TotalYears = r.Year = (begin.Year - end.Year);

            r.TotalMonths = (begin.Month - end.Month) + (12 * r.Year);

            r.TotalWeeks = (int)Math.Floor(span.TotalDays) / 7;


            r.TotalDays = (int)Math.Floor(span.TotalDays);
            r.TotalHours = (int)Math.Floor(span.TotalHours);
            r.TotalMinutes = (long)Math.Floor(span.TotalMinutes);
            r.TotalSeconds = (long)Math.Floor(span.TotalSeconds);
            r.TotalMilliseconds = (long)Math.Floor(span.TotalMilliseconds);




            return r;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static DateTime DateDifff(this DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            var tcs = ts1.Subtract(ts2).Ticks;
            System.Web.HttpContext.Current.Response.Write(Math.Abs(tcs));
            return new DateTime(Math.Abs(tcs));
        }
        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /**/
        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /**/
        /// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /**/
        /// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfPrdviousMonth(this DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 获取2个日期的间隔天数
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="datetime2"></param>
        /// <returns>返回t1和t2之间的时间天数</returns>
        public static int Days2(this DateTime datetime, DateTime datetime2)
        {
            datetime = DateTime.Parse(datetime.ToString("yyyy-MM-dd"));
            return datetime.Subtract(DateTime.Parse(datetime2.ToString("yyyy-MM-dd"))).Days + 1;
        }


        /// <summary>
        /// 返回工作日时间
        /// </summary>
        /// <param name="time">传入时间</param>
        /// <param name="isup">(true礼拜六向后跳2天,false礼拜六向前跳1天)</param>
        /// <param name="exsix">排除星期六</param>
        /// <returns></returns>
        public static DateTime WorkDay(this DateTime time, bool isup = true, bool exsix = false)
        {
            if (time.DayOfWeek == DayOfWeek.Saturday)
            {
                if (!exsix)
                {
                    time = time.AddDays(isup ? 2 : -1);
                }
            }
            else if (time.DayOfWeek == DayOfWeek.Sunday)
            {
                time = time.AddDays(1);
            }

            return time;
        }

        /// <summary>
        /// 返回工作日时间
        /// </summary>
        /// <param name="time">传入时间</param>
        /// <param name="isup">(true礼拜六向后跳2天,false礼拜六向前跳1天)</param>
        /// <param name="appendnums">跳过天数和</param>
        /// <param name="exsix">排除星期六</param>
        /// <returns></returns>
        public static DateTime WorkDay(this DateTime time, ref int appendnums, bool isup = true, bool exsix = false)
        {
            if (time.DayOfWeek == DayOfWeek.Saturday)
            {
                if (!exsix)
                {
                    time = time.AddDays(isup ? 2 : -1);
                    appendnums += isup ? 2 : -1;
                }
            }
            else if (time.DayOfWeek == DayOfWeek.Sunday)
            {
                time = time.AddDays(1);
                appendnums += 1;
            }

            return time;
        }


        /// <summary>
        /// 返回时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTicks(this DateTime time)
        {
            return (time.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000;
        }
    }
}
