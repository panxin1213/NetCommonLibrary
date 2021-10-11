using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace System.IO
{
    /// <summary>
    /// 文件扩展
    /// </summary>
    public static class FileEx
    {
        /// <summary>
        /// 安全移动文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="destFile"></param>
        /// <returns></returns>
        public static bool MoveFile(string file, string destFile)
        {

            //检查目录安全
            if (file.IndexOf("..") != -1 && destFile.IndexOf("..") != -1)
                return false;

            //检查后缀一致
            if (Path.GetExtension(file) != Path.GetExtension(destFile))
                return false;

            
                if (!DirectoryEx.CreateFolder(destFile))
                {
                    return false;
                }
                File.Delete(destFile);
                File.Move(file, destFile);
                return true;
            

        }
        /// <summary>
        /// 删除过期文件
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="dayAfter">超过天数</param>
        public static void DeleteExpiredFiles(string dir, int dayAfter)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime < DateTime.Now.AddDays(-dayAfter))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch { }
                }
            }
        }

    }
}
