﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ChinaBM.Common
{
    class ThumbKit
    {
        private Image _srcImage;
        private string _srcFileName;

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="fileName">原始图片路径</param>
        public bool SetImage(string fileName)
        {
            _srcFileName = HttpKit.GetMapPath(fileName);
            try
            {
                _srcImage = Image.FromFile(_srcFileName);
            }
            catch
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 回调
        /// </summary>
        /// <returns></returns>
        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// 生成缩略图,返回缩略图的Image对象
        /// </summary>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>缩略图的Image对象</returns>
        public Image GetImage(int width, int height)
        {
            Image.GetThumbnailImageAbort callb = ThumbnailCallback;
            Image img = _srcImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
            return img;
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SaveThumbnailImage(int width, int height)
        {
            switch (Path.GetExtension(_srcFileName).ToLower())
            {
                case ".png":
                    SaveImage(width, height, ImageFormat.Png);
                    break;
                case ".gif":
                    SaveImage(width, height, ImageFormat.Gif);
                    break;
                default:
                    SaveImage(width, height, ImageFormat.Jpeg);
                    break;
            }
        }

        /// <summary>
        /// 生成缩略图并保存
        /// </summary>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <param name="imgformat">保存的图像格式</param>
        /// <returns>缩略图的Image对象</returns>
        public void SaveImage(int width, int height, ImageFormat imgformat)
        {
            if (imgformat != ImageFormat.Gif && (_srcImage.Width > width) || (_srcImage.Height > height))
            {
                Image.GetThumbnailImageAbort callb = ThumbnailCallback;
                Image img = _srcImage.GetThumbnailImage(width, height, callb, IntPtr.Zero);
                _srcImage.Dispose();
                img.Save(_srcFileName, imgformat);
                img.Dispose();
            }
        }

        #region Helper

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image">Image 对象</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="ici">指定格式的编解码参数</param>
        private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            //设置 原图片 对象的 EncoderParameters 对象
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)100));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] codecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in codecInfo)
            {
                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }

        /// <summary>
        /// 计算新尺寸
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
        /// <returns></returns>
        private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal decimalMaxWidth = maxWidth;
            decimal decimalMaxHeight = maxHeight;
            decimal aspectRatio = decimalMaxWidth / decimalMaxHeight;

            int newWidth, newHeight;
            decimal originalWidth = width;
            decimal originalHeight = height;

            if (originalWidth > decimalMaxWidth || originalHeight > decimalMaxHeight)
            {
                decimal factor;
                // determine the largest factor 
                if (originalWidth / originalHeight > aspectRatio)
                {
                    factor = originalWidth / decimalMaxWidth;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / decimalMaxHeight;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        #endregion

        /// <summary>
        /// 制作小正方形
        /// </summary>
        /// <param name="fileName">原图的文件路径</param>
        /// <param name="newFileName">新地址</param>
        /// <param name="newSize">长度或宽度</param>
        public static void MakeSquareImage(string fileName, string newFileName, int newSize)
        {
            Image image = Image.FromFile(fileName);

            const int i = 0;
            int width = image.Width;
            int height = image.Height;
            if (width > height)
                height = i;
            else
                width = i;

            Bitmap b = new Bitmap(newSize, newSize);

            try
            {
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;

                //清除整个绘图面并以透明背景色填充
                g.Clear(Color.Transparent);
                g.DrawImage(image, new Rectangle(0, 0, newSize, newSize),
                            width < height
                                ? new Rectangle(0, (height - width)/2, width, width)
                                : new Rectangle((width - height)/2, 0, height, height), GraphicsUnit.Pixel);

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(fileName).ToString().ToLower()));
            }
            finally
            {
                image.Dispose();
                b.Dispose();
            }
        }

        /// <summary>
        /// 制作缩略图
        /// </summary>
        /// <param name="fileName">原图路径</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            Image original = Image.FromFile(fileName);
            Size newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);
            Image displayImage = new Bitmap(original, newSize);

            try
            {
                displayImage.Save(newFileName, GetFormat(fileName));
            }
            finally
            {
                original.Dispose();
            }
        }
    }
}
