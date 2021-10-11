using System;
using System.Collections.Generic;
using log4net;

namespace Common.Library.Log
{
    /// <summary>
    /// A simple wrapper for log4net
    /// </summary>
    public static class Logger
    {
        private static readonly Dictionary<Type, ILog> Loggers = new Dictionary<Type, ILog>();
        private static readonly object Locker = new object();

        private static ILog GetLogger(Type sourceType)
        {
            lock (Locker)
            {
                if (Loggers.ContainsKey(sourceType))
                {
                    return Loggers[sourceType];
                }

                var logger = LogManager.GetLogger(sourceType);
                Loggers.Add(sourceType,logger);
                return logger;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object source, object message, Exception exception = null)
        {
            GetLogger(source.GetType()).Info(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(Type sourceType, object message, Exception exception = null)
        {
            GetLogger(sourceType).Info(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(object source, object message, Exception exception = null)
        {
            GetLogger(source.GetType()).Debug(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(Type sourceType, object message, Exception exception = null)
        {
            GetLogger(sourceType).Debug(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(object source, object message, Exception exception = null)
        {
            GetLogger(source.GetType()).Warn(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(Type sourceType, object message, Exception exception = null)
        {
            GetLogger(sourceType).Warn(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object source, object message, Exception exception = null)
        {
            var url = "";
            try
            {
                url = System.Web.HttpContext.Current.Request.Url.ToString();
            }
            catch
            {

            }

            GetLogger(source.GetType()).Error(message + (!String.IsNullOrEmpty(url) ? string.Format("  url:{0}  ", url) : ""), exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(Type sourceType, object message, Exception exception = null)
        {
            GetLogger(sourceType).Error(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(object source, object message, Exception exception = null)
        {
            GetLogger(source.GetType()).Fatal(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(Type sourceType, object message, Exception exception = null)
        {
            GetLogger(sourceType).Fatal(message, exception);
        }
    }
}
