using System.Collections.Generic;
using System.IO;

namespace Common.Library.Plugin
{
    /// <summary>
    /// 插件监视器
    /// </summary>
    public class PluginWatcher
    {
        private static bool _enable;

        private static readonly IList<FileSystemWatcher> FileSystemWatchers = new List<FileSystemWatcher>();

        /// <summary>
        /// 
        /// </summary>
        public static bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                foreach (var fileSystemWatcher in FileSystemWatchers) {
                    fileSystemWatcher.EnableRaisingEvents = _enable;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            Enable = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            Enable = false;
        }
    }
}
