using Core.Base;
using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web
{
    public class ImageServerHelper
    {
        private static List<string> ImageServer = BaseConfig.Current.Url.ImageServers;
        public static string Get(string url)
        {
            if (url == null) url = String.Empty;
            return ImageServer[Math.Abs(url.GetHashCode() % ImageServer.Count)];
        }
    }
}
