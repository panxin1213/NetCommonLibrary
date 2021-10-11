using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace System.IO
{
    /// <summary>
    /// 目录扩展
    /// </summary>
    public class DirectoryEx
    {
        /// <summary>
        /// 创建多级文件夹
        /// </summary>
        /// <param name="path"></param>
        public static bool CreateFolder(string path)
        {

            char[] c = new char[] { '/', '\\' };
            if (path.IndexOf(".") == -1) path = path + "/";
            int i = path.IndexOfAny(c, 0);
            while (i != -1)
            {
                string p = path.Substring(0, i);
                bool err = false;
                try
                {
                    DirectoryInfo d = new DirectoryInfo(p);

                    if (!d.Exists) d.Create();
                }
                catch
                {
                    err = true;
                }
                i = path.IndexOfAny(c, i + 1);
                if (i == -1 && err) //如果到最后一级还是错误的。返回错误，因为可能上级无权限检查
                    return false;
            }
            return true;
        }
    }
}
