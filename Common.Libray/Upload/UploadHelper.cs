using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.IO;
using System.Web.Routing;
namespace System.Web.Upload
{
    /// <summary>
    /// 上传文件辅助类
    /// </summary>
    public static class UploadHelper
    {
        /// <summary>
        /// 相对路径转换成服务器绝对路径
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToServerPath(this String str)
        {

            return HttpContext.Current.Server.MapPath(str);
        }
        /// <summary>
        /// string 是否是上传目录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidUploadPath(this String str)
        {
            if (String.IsNullOrWhiteSpace(str) || str.IndexOf("..") > -1)
                return false;
            return str.IndexOf(UploadConfig.UploadPath) == 0;
        }
        /// <summary>
        /// string是否是上传临时目录
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidUploadTempPath(this String str)
        {
            if (String.IsNullOrWhiteSpace(str) || str.IndexOf("..") > -1)
                return false;
            return str.IndexOf(UploadConfig.UploadTempPath) == 0;
        }
        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Delete(string path)
        {
            if (path.IsValidUploadPath())
            {
                try
                {
                    File.Delete(path.ToServerPath());
                }
                catch { }
            }
            return false;
        }
        /// <summary>
        /// 移动临时目录到目标目录,并删除旧文件
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="path"></param>
        /// <param name="oldpath"></param>
        /// <returns></returns>
        public static bool MoveTo(string temp, string path, string oldpath)
        {
            if (!temp.IsValidUploadTempPath())
            {
                return true;
            }
            var r = FileEx.MoveFile(temp.ToServerPath(), path.ToServerPath());
            if (oldpath.IsValidUploadPath() && r)
                try
                {
                    File.Delete(oldpath.ToServerPath());
                }
                catch { }
            return r;
        }

        /// <summary>
        /// 是否有文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool HasFile(this HttpPostedFile file)
        {
            return file != null && file.ContentLength > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="AllowType"></param>
        /// <param name="MaxSize"></param>
        /// <returns></returns>
        private static IList<string> ValidFile(HttpPostedFileBase file, UploadConfig config)
        {

            var allowExt = UploadConfig.GetAllowFileExt(config.AllowType);

            var r = new List<string>();
            if (config.Validate != null)
            {
                var validateMsg = config.Validate();
                if (!String.IsNullOrEmpty(validateMsg))
                {
                    r.Add(validateMsg);
                    return r;
                }
                //config.Validate.Compile();
            }

            if (file == null)
            {
                r.Add("没有文件");
                return r;
            }
            FileInfo f;
            try
            {
                f = new FileInfo(file.FileName);
            }
            catch
            {
                return r;
            }
            //文件类型
            if (String.IsNullOrEmpty(f.Extension) || !allowExt.ContainsKey(f.Extension.ToLower()))
            {
                r.Add(String.Format("文件类型只支持 {0}", String.Join(",", allowExt.Keys)));
            }
            if (file.ContentLength > (config.MaxLength * 1024))
            {
                r.Add(String.Format("文件不能大于 {0} kb", config.MaxLength));
            }
            return r;
        }
        public static UploadMessage SaveFile(this HttpPostedFileBase file, UploadConfig config)
        {

            var result = UploadHelper.ValidFile(file, config);

            var r = new UploadMessage
            {
                Errors = result
            };
            if (result.Count == 0)
            {


                string fullpath = UploadConfig.UploadPath + config.GetPath() + Path.GetExtension(file.FileName);
                //if (File.Exists(fullpath))
                //    throw new Exception("文件已经存在");
                //try
                {
                    var wulipath = fullpath.ToServerPath();
                    var imgpath = ConfigurationManager.AppSettings["ImagePath"] ?? "";

                    if (!String.IsNullOrWhiteSpace(imgpath))
                    {
                        wulipath = wulipath.Replace("/".ToServerPath(), imgpath);
                    }

                    DirectoryEx.CreateFolder(wulipath);
                    file.SaveAs(wulipath);
                    r.FilePath = fullpath;
                    config.UploadCallBack(file, ref r);
                    if (r.FilePath != fullpath)
                    {
                        File.Delete(wulipath);
                    }
                }
                //catch (Exception e)
                //{
                //    result.Add("错误:" + e.Message);
                //    dal.Delete(new string[] { fullpath });
                //}

            }
            return r;
        }

    }
}
