using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace Common.Library.Kits
{
    public static class ZipKit
    {

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FilePath">需要压缩文件路径集合</param>
        /// <param name="FileName">压缩后文件名</param>
        /// <param name="SavePath">压缩后存放路径</param>
        /// <param name="PassWord">压缩密码，null为无密码</param>
        /// <returns>异常消息，成功返回null</returns>
        public static string SetZipFile(string[] FilePath, string FileName, string SavePath, string PassWord)
        {
            try
            {
                using (ZipFile zipfile = new ZipFile(SavePath + "\\" + FileName + ".zip", Encoding.Default))
                {
                    if (PassWord != string.Empty)
                        zipfile.Password = PassWord;
                    zipfile.AddFiles(FilePath);

                    zipfile.Save();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 生成压缩文件流
        /// </summary>
        /// <param name="text">要压缩的内容</param>
        /// <param name="FileName">压缩的内容文件名</param>
        /// <param name="FilePath">压缩的路径</param>
        /// <param name="passWord">解压密码</param>
        /// <returns></returns>
        public static MemoryStream SetStreamFile(string text=null, string FileName = null, string FilePath = null, string passWord = null)
        {
            MemoryStream st = new MemoryStream();
            try
            {
                using (ZipFile zip = new ZipFile(FileName,Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(passWord)) zip.Password = passWord;
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        zip.AddDirectory(FilePath);
                    }
                    if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(FileName))
                    {
                        zip.AddEntry(FileName, text, Encoding.UTF8);
                    }
                    zip.Save(st);
                    st.Position = 0;//索引位置设置为0
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return st;
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="FilePath">需要压缩文件夹路径</param>
        /// <param name="FileName">压缩后文件名</param>
        /// <param name="SavePath">压缩后存放路径</param>
        /// <param name="PassWord">压缩密码，null为无密码</param>
        /// <returns>异常消息，成功返回null</returns>
        public static string SetZipFile(string FilePath, string FileName, string SavePath, string PassWord)
        {
            try
            {
                using (ZipFile zipfile = new ZipFile(SavePath + "\\" + FileName + ".zip", Encoding.Default))
                {
                    if (PassWord != string.Empty)
                        zipfile.Password = PassWord;
                    zipfile.AddDirectory(FilePath);
                    zipfile.Save();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="FilePath">zip文件路径</param>
        /// <param name="ReleasePath">解压路径</param>
        /// <param name="FileName">需要解压的文件名</param>
        /// <param name="PassWord">解压密码 null为无密码</param>
        /// <returns>异常消息，成功返回null</returns>
        public static string ReleaseFile(string FilePath, string ReleasePath, string[] FileName, string PassWord)
        {
            try
            {
                using (ZipFile zipfile = ZipFile.Read(FilePath))
                {
                    if (PassWord != string.Empty)
                        zipfile.Password = PassWord;
                    foreach (string filename in FileName)
                        zipfile[filename].Extract(ReleasePath);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 解压全部文件
        /// </summary>
        /// <param name="FilePath">zip文件路径</param>
        /// <param name="ReleasePath">解压路径</param>
        /// <param name="PassWord">解压密码，null为无密码</param>
        /// <returns>异常消息，成功返回null</returns>
        public static string ReleaseAllFile(string FilePath, string ReleasePath, string PassWord)
        {
            try
            {
                using (ZipFile zipfile = ZipFile.Read(FilePath))
                {
                    if (PassWord != string.Empty)
                        zipfile.Password = PassWord;
                    foreach (ZipEntry zipentry in zipfile)
                    {
                        zipentry.Extract(ReleasePath);
                    }


                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 从zip文件中移除文件
        /// </summary>
        /// <param name="FilePath">zip文件路径</param>
        /// <param name="FileName">要移除的文件名集合</param>
        /// <returns>异常消息，成功返回null</returns>
        public static string DelFile(string FilePath, string[] FileName)
        {
            try
            {
                using (ZipFile zipfile = ZipFile.Read(FilePath))
                {
                    foreach (string filename in FileName)
                        zipfile.RemoveEntry(filename);
                    zipfile.Save();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 向zip文件内添加文件
        /// </summary>
        /// <param name="FilePath">zip文件路径</param>
        /// <param name="AddFilePath">要添加的文件路径集合</param>
        /// <returns></returns>
        public static string AddFile(string FilePath, string[] AddFilePath)
        {
            try
            {
                using (ZipFile zipfile = ZipFile.Read(FilePath))
                {
                    zipfile.AddFiles(AddFilePath);
                    zipfile.Save();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
    }
}
