namespace ChinaBM.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using System.IO;

    /// <summary>
    /// 图片处理工具
    /// </summary>
    public class ImageKit
    {
        private Image image;

        public int Width
        {
            get
            {
                return image.Width;
            }
        }

        public int Height
        {
            get
            {
                return image.Height;
            }
        }

        public void ImageDispose()
        {
            if (image != null)
            {
                image.Dispose();
            }
        }

        /// <summary>
        /// 文件路径加载原图
        /// </summary>
        /// <param name="filapath">文件路径及名称</param>
        public ImageKit(string filapath)
        {
            image = Image.FromFile(filapath, false);
        }

        /// <summary>
        /// 字符流加载图片
        /// </summary>
        /// <param name="stream">流</param>
        public ImageKit(Stream stream)
        {
            image = Image.FromStream(stream, true, true);
        }

        #region SaveImage
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="savepath">保存路径</param>
        /// <returns></returns>
        public bool SaveImage(string savepath, int thisWidth, int thisHeight, bool adaptive)
        {
            try
            {
                if (thisWidth == 0)
                {
                    thisWidth = image.Width;
                }

                if (thisHeight == 0)
                {
                    thisHeight = image.Height;
                }

                if (!File.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath.Substring(0, savepath.LastIndexOf("\\")));
                }
                Bitmap bitmap = NarrowImage(thisWidth, thisHeight, adaptive);
                return TrySaveAsJPEG(bitmap, savepath, 90);
            }
            catch
            {
                return false;
            }
        }

        public bool SaveImage(string savepath, int thisWidth, int thisHeight)
        {
            return SaveImage(savepath, thisWidth, thisHeight, true);
        }

        #endregion

        #region 返回按指定宽度的缩略图

        /// <summary>
        /// 返回按指定宽度的缩略图
        /// </summary>
        /// <param name="thisWidth">宽度</param>
        /// <param name="thisHeight">高度</param>
        /// <returns></returns>
        public Bitmap NarrowImage(int thisWidth, int thisHeight, bool adaptive)
        {
            double imageWidth, imageHeight;
            imageWidth = image.Width;
            imageHeight = image.Height;

            if (adaptive)
            {
                if (thisWidth < imageWidth)
                {
                    imageHeight = imageHeight * ((double)thisWidth / imageWidth);
                    imageWidth = (double)thisWidth;
                }
                if (thisHeight < imageHeight)
                {
                    imageWidth = imageWidth * ((double)thisHeight / imageHeight);
                    imageHeight = (double)thisHeight;
                }
            }
            else
            {
                imageWidth = (double)thisWidth;
                imageHeight = (double)thisHeight;
            }

            Size mySize = new Size((int)imageWidth, (int)imageHeight);
            Image bitmap = new Bitmap(mySize.Width, mySize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置保真模式为高度保真

            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphics.Clear(Color.Transparent);
            graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);

            graphics.Dispose();

            return new Bitmap(bitmap);
        }

        #endregion

        #region 图片处理静态方法

        /**/
        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        /**/
        /// <summary>
        /// 保存为JPEG格式，支持压缩质量选项
        /// </summary>
        /// <param name="bmp">初始图片</param>
        /// <param name="FileName">保存文件名</param>
        /// <param name="Qty">质量比例,数字越大越清晰</param>
        /// <returns></returns>
        public static bool TrySaveAsJPEG(Bitmap bmp, string FileName, int Qty)
        {
            try
            {
                EncoderParameter p;
                EncoderParameters ps;

                ps = new EncoderParameters(1);

                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
                ps.Param[0] = p;

                bmp.Save(FileName, GetImageCodecInfo("image/jpeg"), ps);
                bmp.Dispose();
                return true;
            }
            catch
            {
                bmp.Save(FileName, ImageFormat.Jpeg);
                bmp.Dispose();
                return false;
            }

        }
        /// <summary>
        /// 取得文件支持格式编码
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        #endregion

    }
}
