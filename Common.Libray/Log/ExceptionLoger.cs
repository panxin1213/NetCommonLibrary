using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
namespace Common.Library.Log
{
    /// <summary>
    /// 异常日志记录
    /// appSettings 里添加 ExceptionLogPath 节 设置日志目录 默认 /log/
    /// appSettings 里添加 ExceptionLogFileMax 节 设置日志大小（超出备份再新建） 单位 MB 默认 10
    /// appSettings 里添加 ExceptionLogEnabled 节 设置是否开启
    /// </summary>
    public class ExceptionLoger
    {
        
        private static string LogPath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ExceptionLogPath"] ?? "/Log/");
        private static int LogFileMax = (Convert.ToInt32(ConfigurationManager.AppSettings["ExceptionLogFileMax"] ?? "10") * 1024 * 1024); //mb
        private static bool LogEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ExceptionLogEnabled"]);
        /// <summary>
        /// 添加一个日志
        /// </summary>
        /// <param name="msg">消息正文</param>
        public static void Add(string msg) 
        {
            if (!LogEnabled) return;
            try
            {
                File.AppendAllText(CheckFileSize(), DateTime.Now + "\t" + msg + "\t" + System.Web.HttpContext.Current.Request.RawUrl + "\r\n");
            }
            catch { }
        }
        private static string CheckFileSize() {
            var filepath = LogPath + "Current.txt";
            try
            {
                if (new FileInfo(filepath).Length >= LogFileMax)
                {
                    File.Move(filepath, LogPath + DateTime.Now + ".txt");
                }
            }
            catch { }
            return filepath;
        }
    }
}
