using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using Common.Library.Log;

namespace System.Drawing
{
    public enum ThumbMode
    {
        /// <summary>
        /// 指定高宽缩放（补白）  
        /// </summary>
        WidthAndHeight,
        /// <summary>
        /// 高宽不超过，按比例
        /// </summary>
        WidthOrHeight,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        Width,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        Height,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut,
        /// <summary>
        /// 不做任何操作
        /// </summary>
        No,
    }
    /// <summary>
    /// 缩略图生成
    /// </summary>
    public class ImageThumb
    {
        private static readonly ImageCodecInfo EncodeInfo = ImageCodecInfo.GetImageEncoders().SingleOrDefault(a => a.FormatDescription.Equals("JPEG"));
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbMode mode = ThumbMode.Cut, bool ismark = true)
        {
            try
            {
                using (StreamReader stream = new StreamReader(originalImagePath))
                {
                    MakeThumbnail(stream.BaseStream, thumbnailPath, width, height, mode, ismark);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + originalImagePath, e);
            }
        }

        public static void MakeThumbnail(Stream stream, string thumbnailPath, int width, int height, ThumbMode mode = ThumbMode.Cut, bool ismark = true)
        {
            MakeThumbnail(stream, thumbnailPath, width, height, 0, 0, mode, ismark);
        }


        public static void MakeThumbnail(Stream stream, string thumbnailPath, int width, int height, int startX, int startY, ThumbMode mode = ThumbMode.Cut, bool ismark = true)
        {

            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(stream);
            int towidth = width;
            int toheight = height;


            int x = 0; //原始图片取X点开始
            int y = 0;
            int t_x = 0;//从目标图上X点开始画
            int t_y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            if (towidth > 1500) towidth = 1000;
            if (toheight > 1500) toheight = 1000;
            if (towidth > ow) towidth = ow;
            if (toheight > oh) toheight = oh;

            if (toheight != 0 && toheight < 20) toheight = 20;
            if (towidth != 0 && towidth < 20) towidth = 20;


            if (towidth == 0 && toheight == 0)
            {
                towidth = ow;
                toheight = oh;
                mode = ThumbMode.WidthAndHeight;
            }
            else
            {
                if (towidth == 0) //指定高，宽按比例
                {
                    mode = ThumbMode.Height;
                }
                if (toheight == 0) //指定宽，高按比例
                {
                    mode = ThumbMode.Width;
                }
            }
            if (mode == ThumbMode.WidthOrHeight)
            {
                if (oh > ow)
                {
                    mode = ThumbMode.Height;
                }
                else
                {
                    mode = ThumbMode.Width;
                }
            }

            if (oh < toheight && ow < towidth)
            {
                toheight = oh;
                towidth = ow;
                mode = ThumbMode.WidthAndHeight;
            }
            int new_height = toheight;
            int new_width = towidth;

            switch (mode)
            {
                case ThumbMode.WidthAndHeight://指定高宽缩放（自适应，补白）
                    if (((double)ow / oh) > ((double)towidth / toheight)) //当图片太宽的时候    
                    {
                        new_height = (int)(oh * ((double)towidth / ow));
                        //towidth = (float)towidth;
                        //此时x坐标不用修改    
                        t_y = (int)(((float)toheight - new_height) / 2);

                    }
                    else
                    {
                        new_width = (int)(ow * ((double)toheight / oh));  //太高的时候   
                        //toheight = toheight;    
                        //此时y坐标不用修改    
                        t_x = (int)(((double)towidth - new_width) / 2);

                    }

                    break;

                case ThumbMode.Width://指定宽，高按比例
                    toheight = new_height = oh * towidth / ow;
                    break;
                case ThumbMode.Height://指定高，宽按比例
                    towidth = new_width = ow * toheight / oh;
                    break;
                case ThumbMode.Cut://指定高宽裁减（不变形）           
                    if ((double)ow / oh > (double)towidth / toheight) //宽高比
                    {
                        //oh = originalImage.Height;
                        ow = (int)(oh * (double)towidth / toheight);
                        y = startY;
                        x = (originalImage.Width - ow) / 2;

                    }
                    else
                    {
                        //ow = originalImage.Width;
                        oh = (int)(ow * (double)toheight / towidth);
                        x = startX;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                case ThumbMode.No:
                    {
                        ow = towidth;
                        oh = toheight;
                        y = startY;
                        x = startX;
                    }
                    break;
                default:
                    break;
            }
            //生成图像的质量
            long Quality = 95;
            //if (towidth < 400 && toheight < 400)
            //    Quality = 80L;
            //if (towidth < 200 && toheight < 200)
            //   Quality = 75L;
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality;
            //设置高质量插值法
            //sman 20140624 此设置造成图片边框
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分

            //var filename = thumbnailPath.IndexOf("\\") == -1 ? 0 : thumbnailPath.Substring(thumbnailPath.LastIndexOf(@"\") + 1).Substring(0, 8).ToInt();

            //if (ismark && (thumbnailPath.IndexOf(@"\ImageThumb\up\") > -1 || (filename != 0 && filename > 20131112)) && new_width > ConfigurationManager.AppSettings["MinAddMarkWidth"].ToInt(300) && new_height > ConfigurationManager.AppSettings["MinAddMarkWidth"].ToInt(300))
            //{
            //    try
            //    {
            //        using (Graphics og = Graphics.FromImage(originalImage))
            //        {
            //            Image markImage = System.Drawing.Image.FromFile(ConfigurationManager.AppSettings["MarkPath"]);
            //            og.DrawImage(markImage, new Rectangle(originalImage.Width - markImage.Width, originalImage.Height - markImage.Height,
            //            markImage.Width, markImage.Height), 0, 0, markImage.Width, markImage.Height, GraphicsUnit.Pixel);
            //            og.Dispose();
            //        }
            //    }
            //    catch
            //    {

            //    }
            //}
            g.FillRectangle(Brushes.White, 0, 0, new_width, new_height);
            g.DrawImage(originalImage, new Rectangle(t_x, t_y, new_width, new_height),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                //bitmap.Save(
                //    thumbnailPath,ImageFormat.Jpeg
                //);
                bitmap.Save(
                    thumbnailPath, EncodeInfo, new EncoderParameters()
                    {
                        Param = new EncoderParameter[]
                        { 
                            new EncoderParameter(Imaging.Encoder.Quality,Quality),

                        }
                    }
                );

                #region 水印


                //if (ismark && new_width >= ConfigurationManager.AppSettings["MinAddMarkWidth"].ToInt(300) && new_height > ConfigurationManager.AppSettings["MinAddMarkWidth"].ToInt(300))
                //{
                //    try
                //    {
                //        System.Drawing.Image image = System.Drawing.Image.FromFile(thumbnailPath);
                //        Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                //        using (Graphics gs = Graphics.FromImage(b))
                //        {
                //            gs.Clear(Color.White);
                //            gs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //            gs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //            gs.CompositingQuality = Drawing2D.CompositingQuality.HighQuality;
                //            gs.DrawImage(image, 0, 0, image.Width, image.Height);

                //            Image watermark = new Bitmap(ConfigurationManager.AppSettings["MarkPath"]); //水印图
                //            ImageAttributes imageAttributes = new ImageAttributes();
                //            ColorMap colorMap = new ColorMap();
                //            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                //            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                //            ColorMap[] remapTable = { colorMap };
                //            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                //            float[][] colorMatrixElements = {
                //                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                //                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                //                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                //                new float[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                //                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                //            };
                //            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                //            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                //            int xpos = 0;
                //            int ypos = 0;
                //            int WatermarkWidth = watermark.Width;
                //            int WatermarkHeight = watermark.Height;
                //            xpos = image.Width - WatermarkWidth - 10;
                //            ypos = image.Height - WatermarkHeight - 10;
                //            gs.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0,
                //                watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
                //            watermark.Dispose();
                //            imageAttributes.Dispose();
                //            image.Dispose();

                //            b.Save(thumbnailPath, EncodeInfo, new EncoderParameters()
                //            {
                //                Param = new EncoderParameter[]
                //                    { 
                //                        new EncoderParameter(Imaging.Encoder.Quality,Quality),

                //                    }
                //            }
                //            );
                //            b.Dispose();
                //        }

                //    }
                //    catch (Exception exception)
                //    {
                //        Logger.Error(typeof(ImageThumb), exception.Message, exception);
                //    }
                //}


                #endregion
            }
            catch (System.Exception e)
            {
                Logger.Error(typeof(ImageThumb), e.Message, e);
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }
}
